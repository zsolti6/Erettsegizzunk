import React, { useEffect, useState } from "react";
import "../css/SubPage.css";
import axios from "axios";
import { PieChart, Pie, Cell, Tooltip, Legend, LineChart, Line, XAxis, YAxis, CartesianGrid } from "recharts";
import { BASE_URL } from '../config';

const COLORS = ["#0088FE", "#FFBB28", "#FF0000"];
const COLORSsmall = ["#00FF00", "#FF0000"];

export const StatisticsComponent = ({ user }) => {
  if (!user) {
    return (
      <div className="text-center mt-5">
        <h1 style={{ marginTop: "5rem" }}>Bejelentkezés szükséges ennek a funkciónak a használatához</h1>
      </div>
    );
  }

  return (
    <div>
      <UserStatisticsChart id="pie" user={user} />
      <FillingByDateChart id="graph" user={user} />
      <ListDetailedStatistics id="list" user={user}/>
    </div>
  );
};

const UserStatisticsChart = ({ user }) => {
  const [data, setData] = useState([]);

  useEffect(() => {
    const fetchUserStatistics = async () => {
      try {
        const body = {
          userId: user.id,
          token: user.token
        };
        const response = await axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-taskFilloutCount`, body);
        const formattedData = Object.entries(response.data).map(([name, count]) => ({ name, count }));
        setData(formattedData);
      } catch (error) {
        console.error("Error fetching user statistics:", error);
      }
    };

    fetchUserStatistics();
  }, [user]);

  return <StatisticsPieChart data={data} />;
};

const FillingByDateChart = ({ user }) => {
  const [data, setData] = useState([]);

  useEffect(() => {
    const fetchFillingByDate = async () => {
      try {
        const body = { userId: user.id, token: user.token };
        const response = await axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-filling-byDate`, body);
        const formattedData = Object.entries(response.data).map(([date, value]) => ({ date, value }));
        setData(formattedData);
      } catch (error) {
        console.error("Error fetching filling by date:", error);
      }
    };

    fetchFillingByDate();
  }, [user]);

  return <LineGraph data={data} />;
};

const ListDetailedStatistics = ({ user }) => {
  const [data, setData] = useState([]);
  const [expanded, setExpanded] = useState(null);

  useEffect(() => {
    const fetchDetailedStatistics = async () => {
      try {
        const body = { userId: user.id, token: user.token, mettol: 0 };
        const response = await axios.post(
          `${BASE_URL}/erettsegizzunk/UserStatistics/get-statitstics-detailed`,
          body
        );
        setData(response.data);
      } catch (error) {
        console.error("Error fetching detailed statistics:", error);
      }
    };

    fetchDetailedStatistics();
  }, [user]);

  const toggleExpand = (index) => {
    setExpanded(expanded === index ? null : index);
  };

  return (
    <div className="container mt-4">
      {data.map((item, index) => {
        const correct = item.joRossz[0];
        const incorrect = item.joRossz[1];
        const total = correct + incorrect;
        const percentage = total > 0 ? ((correct / total) * 100).toFixed(1) : 0;

        return (
          <div key={index} className="card mb-3">
            {/* Header Row */}
            <div
              className="card-header d-flex justify-content-between align-items-center cursor-pointer"
              onClick={() => toggleExpand(index)}
            >
              <div className="col-12 col-md-6 text-truncate">{item.task.description}</div>
              <div className="col-6 col-md-2 text-center"><b>Sikeres:</b> {correct}</div>
              <div className="col-6 col-md-2 text-center"><b>Sikertelen:</b> {incorrect}</div>
              <div className="col-12 col-md-2 text-end">
                <span className="arrow">{expanded === index ? "▲" : "▼"}</span>
              </div>
            </div>

            {/* Collapsible Section */}
            {expanded === index && (
              <div className="card-body">
                <div className="row">
                  {/* Column 1: Full Task Description */}
                  <div className="col-12 col-md-6 mb-3 mb-md-0">
                    <div className="fw-bold">Feladat leírása:</div>
                    <div>{item.task.description}</div>
                  </div>

                  {/* Column 2: Themes and Subject */}
                  <div className="col-12 col-md-2 mb-3 mb-md-0">
                    <div className="mb-2">
                      <div className="fw-bold text-center">Témák:</div>
                      <div className="text-center">{item.task.subjectId}</div>
                    </div>
                    <div>
                      <div className="fw-bold text-center">Tantárgy:</div>
                      <div className="text-center">{item.task.subject.name}</div>
                    </div>
                  </div>

                  {/* Column 3: Date and Last Fill */}
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

                  {/* Column 4: Pie Chart */}
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


const StatisticsPieChart = ({ data }) => {
  return (
    <PieChart width={400} height={400}>
      <Pie data={data} dataKey="count" nameKey="name" cx="50%" cy="60%" outerRadius={100} label>
        {data.map((entry, index) => (
          <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
        ))}
      </Pie>
      <Legend />
      <Tooltip />
    </PieChart>
  );
};

const LineGraph = ({ data }) => {
  return (
    <LineChart width={600} height={300} data={data}>
      <CartesianGrid strokeDasharray="3 3" />
      <XAxis dataKey="date" />
      <YAxis />
      <Tooltip />
      <Legend />
      <Line type="monotone" dataKey="value" stroke="#8884d8" strokeWidth={2} />
    </LineChart>
  );
};