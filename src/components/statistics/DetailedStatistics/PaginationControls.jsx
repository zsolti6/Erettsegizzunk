import React from 'react';

export const PaginationControls = ({ currentPage, pageCount, onPageChange }) => {
  return (
    <div className="d-flex justify-content-center align-items-center mb-3">
      <button
        className="btn btn-secondary mx-2"
        onClick={() => onPageChange(currentPage - 1)}
        disabled={currentPage === 1}
      >
        &lt; Előző
      </button>

      {Array.from({ length: pageCount }, (_, i) => (
        <button
          key={i + 1}
          className={`btn ${currentPage === i + 1 ? "btn-primary" : "btn-secondary"} mx-1`}
          onClick={() => onPageChange(i + 1)}
        >
          {i + 1}
        </button>
      ))}

      <button
        className="btn btn-secondary mx-2"
        onClick={() => onPageChange(currentPage + 1)}
        disabled={currentPage === pageCount}
      >
        Következő &gt;
      </button>
    </div>
  );
};