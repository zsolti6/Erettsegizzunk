import React, { useEffect, useState, useCallback } from "react";
import { View, ScrollView, Text, StyleSheet, Dimensions } from "react-native";
import axios from "axios";
import { FilterControls } from "./FilterControls";
import { StatisticsCard } from "./StatisticsCard";
import { PaginationControls } from "./PaginationControls";
const BASE_URL = "https://erettsegizzunk.onrender.com";

export const DetailedStatistics = ({ user }) => {
  const [data, setData] = useState([]);
  const [pageCount, setPageCount] = useState(1);
  const [currentPage, setCurrentPage] = useState(1);
  const [expandedCardId, setExpandedCardId] = useState(null);
  const [showFilters, setShowFilters] = useState(false);

  const [filters, setFilters] = useState({
    searchText: "",
    subjects: null,
    difficulty: null,
    themes: null,
  });

  const fetchData = useCallback(async (appliedFilters) => {
    try {
      const body = {
        userId: user.id,
        token: user.token,
        oldal: currentPage - 1,
      };

      if (
        appliedFilters.searchText ||
        appliedFilters.subjects ||
        appliedFilters.difficulty ||
        appliedFilters.themes
      ) {
        body.themeId = appliedFilters?.themes?.value || 0;
        body.szoveg = appliedFilters?.searchText || "";
        body.subjectId = appliedFilters?.subjects?.value || 0;
        body.levelId = appliedFilters?.difficulty?.value || 1;
      }

      const response = await axios.post(
        `${BASE_URL}/erettsegizzunk/UserStatistics/get-statitstics-detailed`,
        body
      );

      setData(response.data.filteredTasks || []);
      setPageCount(response.data.oldalDarab || 1);
    } catch (error) {
      Alert.alert("Hiba történt", 
          `Nem sikerült betölteni a statisztikát: ${error.message}`
        );
    }
  }, [currentPage, user]);

  useEffect(() => {
    fetchData(filters);
  }, [filters, fetchData]);

  const handleApplyFilters = () => {
    setCurrentPage(1);
    fetchData(filters);
  };

  return (
    <ScrollView style={styles.container}>
      <FilterControls
        filters={filters}
        onFilterChange={setFilters}
        onApplyFilters={handleApplyFilters}
        showFilters={showFilters}
        setShowFilters={setShowFilters}
      />

      <PaginationControls
        currentPage={currentPage}
        pageCount={pageCount}
        onPageChange={setCurrentPage}
      />

      <View style={styles.cardsContainer}>
        {data.map((item) => (
          <View key={item.task.id} style={styles.cardWrapper}>
            <StatisticsCard
              item={item}
              isExpanded={expandedCardId === item.task.id}
              onToggleExpand={() =>
                setExpandedCardId((prev) => (prev === item.task.id ? null : item.task.id))
              }
            />
          </View>
        ))}
      </View>

      <PaginationControls
        currentPage={currentPage}
        pageCount={pageCount}
        onPageChange={setCurrentPage}
      />
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  cardsContainer: {
    marginTop: 10,
  },
  cardWrapper: {
    marginBottom: 6,
  },
});