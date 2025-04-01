import React, { useEffect, useState } from 'react';
import { FilterControls } from './FilterControls';
import { StatisticsCard } from './StatisticsCard';
import { PaginationControls } from './PaginationControls';
import axios from 'axios';
import { BASE_URL } from '../../../config';
import { useMediaQuery } from 'react-responsive';

export const DetailedStatistics = ({ user }) => {
  const [data, setData] = useState([]);
  const [pageCount, setPageCount] = useState(1);
  const [currentPage, setCurrentPage] = useState(1);
  const [expandedCardId, setExpandedCardId] = useState(null);
  const [showFilters, setShowFilters] = useState(false);
  const isMobile = useMediaQuery({ query: '(max-width: 768px)' });

  const [filters, setFilters] = useState({
    searchText: '',
    subjects: null,
    difficulty: null,
    themes: null
  });

  useEffect(() => {
    const body = {
      userId: user.id,
      token: user.token,
      permission: 1
    }
    axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-statisztika-oldalDarab`, body).then((response) => {
      setPageCount(response.data);
    });
  }, []);

  const fetchData = async (appliedFilters) => {
    try {
      const body = {
        userId: user.id,
        token: user.token,
        oldal: currentPage - 1
      };

      if (appliedFilters.searchText || appliedFilters.subjects || appliedFilters.difficulty || appliedFilters.themes) {
        body.themeId = appliedFilters?.themes.value || 0;
        body.Szoveg = appliedFilters.searchText || "";
        body.subjectId = appliedFilters?.subjects.value || 0;
        body.levelId = appliedFilters?.difficulty.value || 0;
      }

      const response = await axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-statitstics-detailed`, body);
      setData(response.data.filteredTasks);
      setPageCount(response.data.oldalDarab);
    } catch (error) {
      console.log("Error fetching data:", error);
    }
  };

  useEffect(() => {
    fetchData(filters);
  }, [currentPage, user]);

  const handleApplyFilters = () => {
    setCurrentPage(1); // Reset to the first page
    fetchData(filters); // Fetch data with the applied filters
  };

  const paginationProps = { currentPage, pageCount, onPageChange: setCurrentPage };

  return (
    <div className={`container-fluid ${isMobile ? 'px-2' : 'px-3'}`}>
      <FilterControls
        filters={filters}
        onFilterChange={setFilters}
        onApplyFilters={handleApplyFilters} // Pass the apply filters handler
        showFilters={showFilters}
        setShowFilters={setShowFilters}
        isMobile={isMobile}
      />

      <PaginationControls {...paginationProps} />

      <div className={`row ${isMobile ? 'g-2' : 'g-0'}`}>
        {data.map(item => (
          <div key={item.task.id} className="col-12">
            <StatisticsCard 
              item={item}
              isExpanded={expandedCardId === item.task.id}
              onToggleExpand={() => setExpandedCardId(prev => (prev === item.task.id ? null : item.task.id))}
              isMobile={isMobile}
            />
          </div>
        ))}
      </div>

      <PaginationControls {...paginationProps} />
    </div>
  );
};