import React, { useEffect, useState } from "react";
import "../../css/SubPage.css";
import axios from "axios";
import { PieChart, Pie, Cell, Tooltip, Legend, LineChart, Line, XAxis, YAxis, CartesianGrid, ResponsiveContainer } from "recharts";
import { BASE_URL } from '../../config';
import Select from 'react-select';
import { FaFilter, FaSearch, FaTimes } from 'react-icons/fa';

const COLORS = ["#0088FE", "#FFBB28", "#FF0000"];
const COLORSsmall = ["#00FF00", "#FF0000"];

// Filter options data
const subjectOptions = [
  { value: 'matematika', label: 'Matematika' },
  { value: 'magyar', label: 'Magyar nyelv és irodalom' },
  { value: 'tortenelem', label: 'Történelem' },
  { value: 'angol', label: 'Angol nyelv' },
  { value: 'informatika', label: 'Informatika' }
];

const difficultyOptions = [
  { value: 'kozp', label: 'Középszint' },
  { value: 'emelt', label: 'Emelt szint' }
];

const yearOptions = [
  { value: '2023', label: '2023' },
  { value: '2022', label: '2022' },
  { value: '2021', label: '2021' },
  { value: '2020', label: '2020' }
];

export const StatisticsComponent = ({ user }) => {
  const [errorMessage, setErrorMessage] = useState("");
  const [loading, setLoading] = useState(true);
  const [userStats, setUserStats] = useState([]);
  const [fillingByDate, setFillingByDate] = useState([]);

  useEffect(() => {
    if (user) {
      const fetchInitialData = async () => {
        try {
          setLoading(true);
          
          const [statsResponse, fillingResponse] = await Promise.all([
            axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-taskFilloutCount`, {
              userId: user.id,
              token: user.token
            }),
            axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-filling-byDate`, {
              userId: user.id,
              token: user.token
            })
          ]);

          setUserStats(Object.entries(statsResponse.data).map(([name, count]) => ({ name, count })));
          setFillingByDate(Object.entries(fillingResponse.data).map(([date, value]) => ({ date, value })));
        } catch (error) {
          console.error("Error fetching statistics:", error);
          setErrorMessage("Hiba történt az adatok betöltése során.");
        } finally {
          setLoading(false);
        }
      };

      fetchInitialData();
    }
  }, [user]);

  if (!user) {
    return (
      <div className="page-wrapper">
        <div className="text-center mt-5">
          <h1 style={{ marginTop: "5rem" }}>Bejelentkezés szükséges</h1>
          <p className="lead text-white mt-3">
            Jelentkezz be a statisztikák megtekintéséhez.
          </p>
          <div className="mt-4">
            <button
              className="btn color-bg3 btn-lg border-0"
              onClick={() => window.location.href = "/belepes"}
            >
              Bejelentkezés
            </button>
          </div>
          <div className="mt-5">
            <div className="row justify-content-center">
              {renderFeatureCards()}
            </div>
          </div>
        </div>
      </div>
    );
  }

  if (loading) {
    return (
      <div className="d-flex justify-content-center align-items-center vh-100">
        <div className="spinner-container">
          <div className="spinner"></div>
        </div>
      </div>
    );
  }

  if (errorMessage) {
    return (
      <div className="d-flex justify-content-center align-items-center vh-100">
        <div className="alert alert-danger text-center">
          {errorMessage}
        </div>
      </div>
    );
  }

  return (
    <div className="mt-4 top-padding w-100 p-5">
      <div className="row g-3 mb-4">
        <div className="col-12 col-md-6">
          <div className="card h-100 color-bg2">
            <div className="card-body">
              <h3 className="card-title text-center mb-0 text-white">Feladatok Statisztikája</h3>
              <StatisticsPieChart data={userStats} />
            </div>
          </div>
        </div>
        <div className="col-12 col-md-6">
          <div className="card h-100 color-bg2">
            <div className="card-body">
              <h3 className="card-title text-center mb-5 text-white">Kitöltések Dátum Szerint</h3>
              <LineGraph data={fillingByDate} />
            </div>
          </div>
        </div>
      </div>

      <div className="row">
        <div className="col-12">
          <div className="card taskCard color-bg2">
            <div className="card-body">
              <DetailedStatistics user={user} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

const DetailedStatistics = ({ user }) => {
  const [data, setData] = useState([]);
  const [pageCount, setPageCount] = useState(1);
  const [currentPage, setCurrentPage] = useState(1);
  const [expanded, setExpanded] = useState(null);
  const [loadingDetails, setLoadingDetails] = useState(true);
  const [showFilters, setShowFilters] = useState(false);
  const [searchText, setSearchText] = useState('');
  const [selectedSubjects, setSelectedSubjects] = useState([]);
  const [selectedDifficulty, setSelectedDifficulty] = useState(null);
  const [selectedYear, setSelectedYear] = useState(null);

  useEffect(() => {
    const fetchDetailedData = async () => {
      try {
        setLoadingDetails(true);
        
        const [pageCountResponse, detailedResponse] = await Promise.all([
          axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-statisztika-oldalDarab`, {
            userId: user.id,
            token: user.token,
            permission: 1
          }),
          axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-statitstics-detailed`, {
            userId: user.id,
            token: user.token,
            oldal: currentPage - 1,
            searchText,
            subjects: selectedSubjects.map(s => s.value),
            difficulty: selectedDifficulty?.value,
            year: selectedYear?.value
          })
        ]);

        setPageCount(pageCountResponse.data || 1);
        setData(detailedResponse.data);
      } catch (error) {
        console.error("Error fetching detailed statistics:", error);
      } finally {
        setLoadingDetails(false);
      }
    };

    fetchDetailedData();
  }, [user, currentPage, searchText, selectedSubjects, selectedDifficulty, selectedYear]);

  const toggleExpand = (index) => {
    setExpanded(expanded === index ? null : index);
  };

  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= pageCount) {
      setCurrentPage(newPage);
    }
  };

  const handleSearch = () => {
    // Reset to first page when applying new filters
    setCurrentPage(1);
  };

  const removeSubject = (subjectToRemove) => {
    setSelectedSubjects(selectedSubjects.filter(subject => subject.value !== subjectToRemove.value));
  };

  const clearFilters = () => {
    setSearchText('');
    setSelectedSubjects([]);
    setSelectedDifficulty(null);
    setSelectedYear(null);
    setCurrentPage(1);
  };

  if (loadingDetails) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ minHeight: '200px' }}>
        <div className="spinner-container">
          <div className="spinner"></div>
        </div>
      </div>
    );
  }

  return (
    <div className="container-fluid">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h3 className="card-title text-center">Részletes Statisztikák</h3>
        <button 
          className="btn btn-primary"
          onClick={() => setShowFilters(!showFilters)}
        >
          <FaFilter className="me-2" />
          {showFilters ? 'Szűrők elrejtése' : 'Szűrők megjelenítése'}
        </button>
      </div>

      {showFilters && (
        <div className="filter-controls mb-4 p-3 border rounded bg-light">
          <div className="row g-3">
            <div className="col-md-12">
              <label className="form-label">Keresés szöveg alapján</label>
              <div className="input-group">
                <input
                  type="text"
                  className="form-control"
                  value={searchText}
                  onChange={(e) => setSearchText(e.target.value)}
                  placeholder="Keresés feladatleírásban..."
                />
                {searchText && (
                  <button 
                    className="btn btn-outline-secondary" 
                    onClick={() => setSearchText('')}
                  >
                    <FaTimes />
                  </button>
                )}
              </div>
            </div>

            <div className="col-md-4">
              <label className="form-label">Tantárgy</label>
              <Select
                options={subjectOptions}
                isMulti
                value={selectedSubjects}
                onChange={setSelectedSubjects}
                placeholder="Válassz tantárgyat..."
                className="basic-multi-select"
                classNamePrefix="select"
                noOptionsMessage={() => "Nincs találat"}
              />
            </div>

            <div className="col-md-4">
              <label className="form-label">Nehézség</label>
              <Select
                options={difficultyOptions}
                value={selectedDifficulty}
                onChange={setSelectedDifficulty}
                placeholder="Válassz nehézséget..."
                isClearable
              />
            </div>

            <div className="col-md-4">
              <label className="form-label">Év</label>
              <Select
                options={yearOptions}
                value={selectedYear}
                onChange={setSelectedYear}
                placeholder="Válassz évet..."
                isClearable
              />
            </div>

            <div className="col-md-6">
              <button 
                className="btn btn-success w-100"
                onClick={handleSearch}
              >
                <FaSearch className="me-2" />
                Szűrés alkalmazása
              </button>
            </div>

            <div className="col-md-6">
              <button 
                className="btn btn-outline-secondary w-100"
                onClick={clearFilters}
              >
                Szűrők törlése
              </button>
            </div>
          </div>

          {(selectedSubjects.length > 0 || selectedDifficulty || selectedYear) && (
            <div className="selected-filters mt-3">
              <h6>Aktív szűrők:</h6>
              <div className="d-flex flex-wrap gap-2">
                {selectedSubjects.map(subject => (
                  <span key={subject.value} className="badge bg-primary">
                    {subject.label}
                    <button 
                      type="button" 
                      className="btn-close btn-close-white ms-2"
                      onClick={() => removeSubject(subject)}
                      aria-label="Remove"
                    />
                  </span>
                ))}
                {selectedDifficulty && (
                  <span className="badge bg-secondary">
                    {selectedDifficulty.label}
                    <button 
                      type="button" 
                      className="btn-close btn-close-white ms-2"
                      onClick={() => setSelectedDifficulty(null)}
                      aria-label="Remove"
                    />
                  </span>
                )}
                {selectedYear && (
                  <span className="badge bg-info">
                    {selectedYear.label}
                    <button 
                      type="button" 
                      className="btn-close btn-close-white ms-2"
                      onClick={() => setSelectedYear(null)}
                      aria-label="Remove"
                    />
                  </span>
                )}
              </div>
            </div>
          )}
        </div>
      )}

      <div className="d-flex justify-content-center align-items-center mb-3">
        <button
          className="btn btn-secondary mx-2"
          onClick={() => handlePageChange(currentPage - 1)}
          disabled={currentPage === 1}
        >
          &lt; Előző
        </button>

        {Array.from({ length: pageCount }, (_, i) => (
          <button
            key={i + 1}
            className={`btn ${currentPage === i + 1 ? "btn-primary" : "btn-secondary"} mx-1`}
            onClick={() => handlePageChange(i + 1)}
          >
            {i + 1}
          </button>
        ))}

        <button
          className="btn btn-secondary mx-2"
          onClick={() => handlePageChange(currentPage + 1)}
          disabled={currentPage === pageCount}
        >
          Következő &gt;
        </button>
      </div>

      {data.map((item, index) => {
        const correct = item.joRossz[0];
        const incorrect = item.joRossz[1];
        const total = correct + incorrect;
        const percentage = total > 0 ? ((correct / total) * 100).toFixed(1) : 0;

        return (
          <div key={index} className="statisticsCard mb-3 color-bg3">
            <div
              className="card-header d-flex justify-content-between align-items-center cursor-pointer color-bg3 detailedTaskCardHeader"
              onClick={() => toggleExpand(index)}
            >
              <div className="col-6 col-md-4 col-lg-4 text-truncate">{item.task.description}</div>
              <div className="col-6 col-md-2 col-lg-4 text-center d-none d-md-block">
                <b>Sikeres:</b> {correct}
              </div>
              <div className="col-6 col-md-2 col-lg-2 text-center d-none d-md-block">
                <b>Sikertelen:</b> {incorrect}
              </div>
              <div className="col-2 col-md-2 text-end">
                <span className="arrow">{expanded === index ? "▲" : "▼"}</span>
              </div>
            </div>

            {expanded === index && (
              <div className="card-body">
                <div className="row">
                  <div className="col-12 col-md-6 mb-3 mb-md-0">
                    <div className="col-12 col-md-12 mb-3 mb-md-0">
                      <div className="fw-bold">Feladat leírása:</div>
                      <div>{item.task.description}</div>
                    </div>
                    <br />
                    <div className="col-12 col-md-12 mb-3 mb-md-0">
                      <div className="fw-bold">Feladat szövege:</div>
                      <div>{item.task.text}</div>
                    </div>
                  </div>

                  <div className="col-12 col-md-2 mb-3 mb-md-0">
                    <div className="mb-2">
                      <div className="fw-bold text-center">Témák:</div>
                      <div className="text-center">
                        {item.task.themes.map(x => x.name).join(", ") || "Nincs téma"}
                      </div>
                    </div>
                    <div>
                      <div className="fw-bold text-center">Tantárgy:</div>
                      <div className="text-center">{item.task.subject.name}</div>
                    </div>
                  </div>

                  <div className="col-12 col-md-2 mb-3 mb-md-0">
                    <div className="mb-2">
                      <div className="fw-bold text-center">Utolsó kitöltés dátuma:</div>
                      <div className="text-center">
                        {new Date(item.utolsoKitoltesDatum).toLocaleDateString("hu-HU")}
                      </div>
                    </div>
                    <div>
                      <div className="fw-bold text-center">Utolsó eredmény:</div>
                      <div className="text-center">{item.utolsoSikeres ? "✅" : "❌"}</div>
                    </div>
                  </div>

                  <div className="col-12 col-md-2 d-flex flex-column justify-content-center align-items-center">
                    <PieChart width={100} height={100}>
                      <Pie
                        data={[
                          { name: "Helyes", value: correct },
                          { name: "Helytelen", value: incorrect },
                        ]}
                        dataKey="value"
                        cx="50%"
                        cy="50%"
                        outerRadius={40}
                        label={false}
                      >
                        {[...Array(2)].map((_, i) => (
                          <Cell key={i} fill={COLORSsmall[i]} />
                        ))}
                      </Pie>
                      <Tooltip />
                    </PieChart>
                    <div className="mt-2">{percentage}%</div>
                  </div>
                </div>
              </div>
            )}
          </div>
        );
      })}
    </div>
  );
};

const renderFeatureCards = () => {
  const features = [
    {
      icon: "fas fa-chart-pie",
      title: "Statisztikák",
      description: "Kövesd a feladatok megoldásának eredményeit"
    },
    {
      icon: "fas fa-tasks",
      title: "Feladatok",
      description: "Gyakorolj különböző témakörökben"
    },
    {
      icon: "fas fa-calendar-alt",
      title: "Idővonal",
      description: "Nézd meg a kitöltéseid időrendben"
    }
  ];

  return features.map((feature, index) => (
    <div className="col-md-4 mb-4" key={index}>
      <div className="card color-bg2 h-100">
        <div className="card-body text-center">
          <i className={`${feature.icon} fa-3x text-white mb-3`}></i>
          <h4 className="card-title text-white">{feature.title}</h4>
          <p className="card-text text-white">{feature.description}</p>
        </div>
      </div>
    </div>
  ));
};

const StatisticsPieChart = ({ data }) => {
  return (
    <ResponsiveContainer width="100%" height={400}>
      <PieChart>
        <Pie 
          data={data} 
          dataKey="count" 
          nameKey="name" 
          cx="50%" 
          cy="50%" 
          outerRadius={125} 
          label
        >
          {data.map((entry, index) => (
            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
          ))}
        </Pie>
        <Legend />
        <Tooltip />
      </PieChart>
    </ResponsiveContainer>
  );
};

const LineGraph = ({ data }) => {
  return (
    <ResponsiveContainer width="100%" height="75%">
      <LineChart data={data}>
        <CartesianGrid strokeDasharray="3 3"/>
        <XAxis 
          dataKey="date"
          label={{ value: "Dátum", position: "insideBottom", offset: 5, dy: 10, style: { fill: 'white' } }} 
          tick={{ fill: "white"}}
        />
        <YAxis 
          label={{ value: "Kitöltött feladatlapok", angle: -90, position: "insideLeft", offset: 30, dy: 75, dx: -10, style: { fill: 'white' } }} 
          tick={{ fill: "white"}}
        />
        <Tooltip />
        <Line type="monotone" dataKey="value" stroke="#8884d8" strokeWidth={2} />
      </LineChart>
    </ResponsiveContainer>
  );
};