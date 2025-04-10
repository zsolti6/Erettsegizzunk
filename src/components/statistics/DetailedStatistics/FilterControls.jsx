import React, { useEffect, useState } from "react";
import Select from "react-select";
import { FaFilter, FaSearch, FaTimes } from "react-icons/fa";
import axios from "axios";
import { BASE_URL } from "../../../config";
import { MessageModal } from "../../common/MessageModal"; // Import the reusable MessageModal component

export const FilterControls = ({
  filters,
  onFilterChange,
  onApplyFilters,
  showFilters,
  setShowFilters,
}) => {
  const [subjectOptions, setSubjectOptions] = useState([]);
  const [difficultyOptions, setDifficultyOptions] = useState([]);
  const [themeOptions, setThemeOptions] = useState([]);
  const [messageModal, setMessageModal] = useState({
    show: false,
    type: "",
    message: "",
  }); // State for modal

  // Fetch subject options from the API
  useEffect(() => {
    const fetchSubjects = async () => {
      try {
        const response = await axios.get(
          `${BASE_URL}/erettsegizzunk/Tantargyak/get-tantargyak`
        );
        const options = response.data.map((subject) => ({
          value: subject.id,
          label: subject.name,
        }));
        setSubjectOptions(options);
      } catch (error) {
        setMessageModal({
          show: true,
          type: "error",
          message: "Hiba történt a tantárgyak betöltése során.",
        });
      }
    };

    fetchSubjects();
  }, []);

  // Fetch difficulty options from the API
  useEffect(() => {
    const fetchDifficulties = async () => {
      try {
        const response = await axios.get(
          `${BASE_URL}/erettsegizzunk/Levels/get-szintek`
        );
        const options = response.data.map((level) => ({
          value: level.id, // Assuming the API returns an `id` field
          label: level.name, // Assuming the API returns a `name` field
        }));
        setDifficultyOptions(options);
      } catch (error) {
        setMessageModal({
          show: true,
          type: "error",
          message: "Hiba történt a nehézségi szintek betöltése során.",
        });
      }
    };

    fetchDifficulties();
  }, []);

  const handleSearchTextChange = (e) => {
    onFilterChange({ ...filters, searchText: e.target.value });
  };

  const handleSubjectsChange = async (selected) => {
    filters.themes = null;
    onFilterChange({ ...filters, subjects: selected });
    if (selected) {
      try {
        const response = await axios.get(
          `${BASE_URL}/erettsegizzunk/Themes/get-temak-feladatonkent`
        );
        const themesForSubject = response.data[selected.label];

        if (themesForSubject) {
          const options = themesForSubject.map((themeObj) => ({
            value: themeObj.theme.id,
            label: themeObj.theme.name,
          }));
          setThemeOptions(options);
        } else {
          setThemeOptions([]);
        }
      } catch (error) {
        setMessageModal({
          show: true,
          type: "error",
          message: "Hiba történt a témák betöltése során.",
        });
        setThemeOptions([]);
      }
    } else {
      // Clear the theme options and reset the theme filter
      setThemeOptions([]);
      filters.subjects = null;
      onFilterChange({ ...filters, theme: null });
    }
  };

  const handleDifficultyChange = (selected) => {
    onFilterChange({ ...filters, difficulty: selected });
  };

  const handleThemeChange = (selected) => {
    onFilterChange({ ...filters, themes: selected });
  };

  const clearFilters = () => {
    onFilterChange({
      searchText: "",
      subjects: null,
      difficulty: null,
      themes: null,
    });
    setThemeOptions([]);
  };

  return (
    <>
      <button
        className="btn btn-primary mb-2"
        onClick={() => setShowFilters(!showFilters)}
      >
        <FaFilter className="me-2" />
        {showFilters ? "Szűrők elrejtése" : "Szűrők megjelenítése"}
      </button>

      {showFilters && (
        <div className="filter-controls mb-4 p-3 color-bg2 text-white">
          <div className="row g-3">
            <div className="col-md-12">
              <label className="form-label">Keresés szöveg alapján</label>
              <div className="input-group">
                <input
                  type="text"
                  className="form-control"
                  value={filters.searchText}
                  onChange={handleSearchTextChange}
                  placeholder="Keresés feladatleírásban..."
                />
                {filters.searchText && (
                  <button
                    className="btn btn-outline-secondary"
                    onClick={() =>
                      handleSearchTextChange({ target: { value: "" } })
                    }
                  >
                    <FaTimes />
                  </button>
                )}
              </div>
            </div>

            <div className="col-md-4">
              <label className="form-label">Tantárgy</label>
              <Select
                className="text-black"
                options={subjectOptions}
                value={filters.subjects}
                onChange={handleSubjectsChange}
                placeholder="Válassz tantárgyat..."
                isClearable
                noOptionsMessage={() => "Nincs elérhető tantárgy"}
              />
            </div>

            <div className="col-md-4">
              <label className="form-label">Nehézség</label>
              <Select
                className="text-black bg-white"
                options={difficultyOptions}
                value={filters.difficulty}
                onChange={handleDifficultyChange}
                placeholder="Válassz nehézséget..."
                isClearable
                noOptionsMessage={() => "Nincs elérhető nehézség"}
              />
            </div>

            <div className="col-md-4">
              <label className="form-label">Téma</label>
              <Select
                className="text-black"
                options={themeOptions}
                value={filters.themes}
                onChange={handleThemeChange}
                placeholder="Válassz témát..."
                isClearable
                noOptionsMessage={() => "Nincs elérhető téma"}
              />
            </div>

            <div className="col-md-6">
              <button
                className="btn btn-success w-100"
                onClick={() => {
                  onApplyFilters(); // Trigger the apply filters callback
                }}
              >
                <FaSearch className="me-2" />
                Szűrés alkalmazása
              </button>
            </div>

            <div className="col-md-6">
              <button className="btn btn-danger w-100" onClick={clearFilters}>
                Szűrők törlése
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Reusable Message Modal */}
      <MessageModal
        show={messageModal.show}
        type={messageModal.type}
        message={messageModal.message}
        onClose={() => setMessageModal({ ...messageModal, show: false })}
      />
    </>
  );
};
