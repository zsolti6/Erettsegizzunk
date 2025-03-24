import React from 'react';
import Select from 'react-select';
import { FaFilter, FaSearch, FaTimes } from 'react-icons/fa';

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

export const FilterControls = ({ filters, onFilterChange }) => {
  const [showFilters, setShowFilters] = React.useState(false);

  const handleSearchTextChange = (e) => {
    onFilterChange({ ...filters, searchText: e.target.value });
  };

  const handleSubjectsChange = (selected) => {
    onFilterChange({ ...filters, subjects: selected });
  };

  const handleDifficultyChange = (selected) => {
    onFilterChange({ ...filters, difficulty: selected });
  };

  const handleYearChange = (selected) => {
    onFilterChange({ ...filters, year: selected });
  };

  const clearFilters = () => {
    onFilterChange({
      searchText: '',
      subjects: [],
      difficulty: null,
      year: null
    });
  };

  const removeSubject = (subjectToRemove) => {
    onFilterChange({
      ...filters,
      subjects: filters.subjects.filter(
        subject => subject.value !== subjectToRemove.value
      )
    });
  };

  return (
    <>
      <button 
        className="btn btn-primary"
        onClick={() => setShowFilters(!showFilters)}
      >
        <FaFilter className="me-2" />
        {showFilters ? 'Szűrők elrejtése' : 'Szűrők megjelenítése'}
      </button>

      {showFilters && (
        <div className="filter-controls mb-4 p-3 border rounded bg-light">
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
                    onClick={() => handleSearchTextChange({ target: { value: '' } })}
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
                value={filters.subjects}
                onChange={handleSubjectsChange}
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
                value={filters.difficulty}
                onChange={handleDifficultyChange}
                placeholder="Válassz nehézséget..."
                isClearable
              />
            </div>

            <div className="col-md-4">
              <label className="form-label">Év</label>
              <Select
                options={yearOptions}
                value={filters.year}
                onChange={handleYearChange}
                placeholder="Válassz évet..."
                isClearable
              />
            </div>

            <div className="col-md-6">
              <button 
                className="btn btn-success w-100"
                onClick={() => setShowFilters(false)}
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

          {(filters.subjects.length > 0 || filters.difficulty || filters.year) && (
            <div className="selected-filters mt-3">
              <h6>Aktív szűrők:</h6>
              <div className="d-flex flex-wrap gap-2">
                {filters.subjects.map(subject => (
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
                {filters.difficulty && (
                  <span className="badge bg-secondary">
                    {filters.difficulty.label}
                    <button 
                      type="button" 
                      className="btn-close btn-close-white ms-2"
                      onClick={() => handleDifficultyChange(null)}
                      aria-label="Remove"
                    />
                  </span>
                )}
                {filters.year && (
                  <span className="badge bg-info">
                    {filters.year.label}
                    <button 
                      type="button" 
                      className="btn-close btn-close-white ms-2"
                      onClick={() => handleYearChange(null)}
                      aria-label="Remove"
                    />
                  </span>
                )}
              </div>
            </div>
          )}
        </div>
      )}
    </>
  );
};