import React from "react";
import { useMediaQuery } from "react-responsive";
import { FaChevronLeft, FaChevronRight } from "react-icons/fa";

export const PaginationControls = ({
  currentPage,
  pageCount,
  onPageChange,
}) => {
  const isMobile = useMediaQuery({ query: "(max-width: 768px)" });

  const getVisiblePages = () => {
    if (isMobile) {
      const visiblePages = [];
      if (currentPage > 2) visiblePages.push(1);
      if (currentPage > 3) visiblePages.push("...");
      for (
        let i = Math.max(1, currentPage - 1);
        i <= Math.min(pageCount, currentPage + 1);
        i++
      ) {
        visiblePages.push(i);
      }
      if (currentPage < pageCount - 2) visiblePages.push("...");
      if (currentPage < pageCount - 1) visiblePages.push(pageCount);
      return visiblePages;
    }
    return Array.from({ length: pageCount }, (_, i) => i + 1);
  };

  const visiblePages = getVisiblePages();

  return (
    <div className="d-flex justify-content-center align-items-center mb-3 flex-wrap">
      <button
        className={`btn btn-secondary ${isMobile ? "mx-1" : "mx-2"} mb-1`}
        onClick={() => onPageChange(currentPage - 1)}
        disabled={currentPage === 1}
      >
        <FaChevronLeft />
        {!isMobile && " Előző"}
      </button>

      {visiblePages.map((page, index) =>
        page === "..." ? (
          <span
            key={`ellipsis-${index}`}
            className={`px-2 ${
              isMobile ? "mx-1" : "mx-1"
            } d-flex align-items-center`}
          >
            ...
          </span>
        ) : (
          <button
            key={page}
            className={`btn ${
              currentPage === page ? "btn-primary" : "btn-secondary"
            } ${isMobile ? "mx-1" : "mx-1"} mb-1`}
            onClick={() => onPageChange(page)}
          >
            {page}
          </button>
        )
      )}

      <button
        className={`btn btn-secondary ${isMobile ? "mx-1" : "mx-2"} mb-1`}
        onClick={() => onPageChange(currentPage + 1)}
        disabled={currentPage === pageCount}
      >
        {!isMobile && "Következő "}
        <FaChevronRight />
      </button>
    </div>
  );
};