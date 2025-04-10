import React, { useContext } from 'react';
import { View, TouchableOpacity, Text, StyleSheet, Dimensions } from 'react-native';
import { MaterialIcons } from '@expo/vector-icons';
import { ThemeContext } from '../../ThemeContext';

export const PaginationControls = ({ currentPage, pageCount, onPageChange }) => {
  const { theme } = useContext(ThemeContext);
  const styles = getStyles(theme);

  const getVisiblePages = () => {
    const visiblePages = [];
    if (currentPage > 2) visiblePages.push(1);
    if (currentPage > 3) visiblePages.push('...');
    for (let i = Math.max(1, currentPage - 1); i <= Math.min(pageCount, currentPage + 1); i++) {
      visiblePages.push(i);
    }
    if (currentPage < pageCount - 2) visiblePages.push('...');
    if (currentPage < pageCount - 1) visiblePages.push(pageCount);
    return visiblePages;
  };
  const visiblePages = getVisiblePages();
  return (
    <View style={styles.container}>
      {/* Page Numbers */}
      <View style={styles.pagesContainer}>
        {visiblePages.map((page, index) => (
          page === '...' ? (
            <Text key={`ellipsis-${index}`} style={styles.ellipsis}>
              ...
            </Text>
          ) : (
            <TouchableOpacity
              key={page}
              style={[
                styles.pageButton,
                currentPage === page && styles.activePageButton
              ]}
              onPress={() => onPageChange(page)}
              activeOpacity={0.7}
            >
              <Text style={currentPage === page ? styles.activePageText : styles.pageText}>
                {page}
              </Text>
            </TouchableOpacity>
          )
        ))}
      </View>

      <View style={styles.navButtonsContainer}>
        {/* Previous Button */}
        <TouchableOpacity
          style={[styles.navButton, currentPage === 1 && styles.disabledButton]}
          onPress={() => onPageChange(currentPage - 1)}
          disabled={currentPage === 1}
          activeOpacity={0.7}
        >
          <MaterialIcons name="chevron-left" size={24} color={currentPage === 1 ? '#aaa' : '#333'} />
          <Text style={styles.navText}>Előző</Text>
        </TouchableOpacity>


        {/* Next Button */}
        <TouchableOpacity
          style={[styles.navButton, currentPage === pageCount && styles.disabledButton]}
          onPress={() => onPageChange(currentPage + 1)}
          disabled={currentPage === pageCount}
          activeOpacity={0.7}
        >
          <Text style={styles.navText}>Következő</Text>
          <MaterialIcons name="chevron-right" size={24} color={currentPage === pageCount ? '#aaa' : '#333'} />
        </TouchableOpacity>
      </View>
    </View>
  );
};

const getStyles = (theme) =>
  StyleSheet.create({
    container: {
      flexDirection: 'row',
      justifyContent: 'center',
      alignItems: 'center',
      marginVertical: 16,
      flexWrap: 'wrap',
    },
    navButton: {
      flexDirection: 'row',
      alignItems: 'center',
      paddingHorizontal: 12,
      paddingVertical: 8,
      borderRadius: 4,
      backgroundColor: theme,
      marginHorizontal: 4,
    },
    disabledButton: {
      opacity: 0.5,
    },
    navText: {
      marginHorizontal: 2,
      color: '#fff',
    },
    pagesContainer: {
      flexDirection: 'row',
      alignItems: 'center',
    },
    pageButton: {
      minWidth: 36,
      height: 36,
      justifyContent: 'center',
      alignItems: 'center',
      marginHorizontal: 2,
      borderRadius: 4,
      backgroundColor: theme,
    },
    activePageButton: {
      backgroundColor: '#888',
    },
    pageText: {
      color: 'white',
    },
    activePageText: {
      color: '#333',
    },
    navButtonsContainer: {
      justifyContent: 'center',
      flexDirection: 'row',
      marginTop: 15,
      paddingHorizontal: 5,
      width: '100%',
    },
    ellipsis: {
      paddingHorizontal: 8,
      color: '#333',
    },
  });