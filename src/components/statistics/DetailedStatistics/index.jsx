import React, { useEffect, useState } from 'react';
import { FilterControls } from './FilterControls';
import { StatisticsCard } from './StatisticsCard';
import { PaginationControls } from './PaginationControls';
import axios from 'axios';
import { BASE_URL } from '../../../config';

export const DetailedStatistics = ({ user }) => {
  const [data, setData] = useState([]);
  const [pageCount, setPageCount] = useState(1);
  const [currentPage, setCurrentPage] = useState(1);
  const [expanded, setExpanded] = useState(null);
  const [loadingDetails, setLoadingDetails] = useState(true);
  const [filters, setFilters] = useState({
    searchText: '',
    subjects: [],
    difficulty: null,
    year: null
  });

  useEffect(() => {
    const fetchDetailedData = async () => {
      try {
        setLoadingDetails(true);
        
        const body = {
          userId: user.id,
          token: user.token,
          oldal: currentPage - 1
        };

        // Add filters if they exist
        if (filters.searchText) body.searchText = filters.searchText;
        if (filters.subjects.length > 0) body.subjects = filters.subjects.map(s => s.value);
        if (filters.difficulty) body.difficulty = filters.difficulty.value;
        if (filters.year) body.year = filters.year.value;

        const [pageCountResponse, detailedResponse] = await Promise.all([
          axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-statisztika-oldalDarab`, {
            userId: user.id,
            token: user.token,
            permission: 1
          }),
          axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-statitstics-detailed`, body)
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
  }, [user, currentPage, filters]);

  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= pageCount) {
      setCurrentPage(newPage);
    }
  };

  const handleFilterChange = (newFilters) => {
    setFilters(newFilters);
    setCurrentPage(1); // Reset to first page when filters change
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
        <FilterControls 
          filters={filters}
          onFilterChange={handleFilterChange}
        />
      </div>

      <PaginationControls 
        currentPage={currentPage}
        pageCount={pageCount}
        onPageChange={handlePageChange}
      />

      {data.map((item, index) => (
        <StatisticsCard 
          key={index}
          item={item}
          isExpanded={expanded === index}
          onToggleExpand={() => setExpanded(expanded === index ? null : index)}
        />
      ))}

      <PaginationControls 
        currentPage={currentPage}
        pageCount={pageCount}
        onPageChange={handlePageChange}
      />
    </div>
  );
};