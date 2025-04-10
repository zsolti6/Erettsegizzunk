import React, { useEffect, useState, useContext } from "react";
import { View, Text, TextInput, Button, TouchableOpacity, StyleSheet, ScrollView } from "react-native";
import RNPickerSelect from "react-native-picker-select";
import axios from "axios";
import { FontAwesome } from "@expo/vector-icons";
const BASE_URL = "https://erettsegizzunk.onrender.com";
import { ThemeContext } from '../../ThemeContext';

export const FilterControls = ({
  filters,
  onFilterChange,
  onApplyFilters,
  showFilters,
  setShowFilters
}) => {
  const [subjectOptions, setSubjectOptions] = useState([]);
  const [difficultyOptions, setDifficultyOptions] = useState([]);
  const [themeOptions, setThemeOptions] = useState([]);
  const { theme } = useContext(ThemeContext);
  const styles = getStyles(theme);

  useEffect(() => {
    const fetchSubjects = async () => {
      try {
        const response = await axios.get(`${BASE_URL}/erettsegizzunk/Tantargyak/get-tantargyak`);
        const options = response.data.map((subject) => ({
          label: subject.name,
          value: subject.id,
        }));
        setSubjectOptions(options);
      } catch (error) {
        lert.alert("Hiba történt", 
          `Nem sikerült lekérni a tantárgyakat: ${error.message}`
        );
      }
    };
    fetchSubjects();
  }, []);

  useEffect(() => {
    const fetchDifficulties = async () => {
      try {
        const response = await axios.get(`${BASE_URL}/erettsegizzunk/Levels/get-szintek`);
        const options = response.data.map((level) => ({
          label: level.name,
          value: level.id,
        }));
        setDifficultyOptions(options);
      } catch (error) {
        lert.alert("Hiba történt", 
          `Nem sikerült lekérni a tantárgyakat: ${error.message}`
        );
      }
    };
    fetchDifficulties();
  }, []);

  const handleSubjectsChange = async (value, label) => {
    filters.themes = null;
    const selected = { value, label };
    onFilterChange({ ...filters, subjects: selected });

    if (value) {
      try {
        const response = await axios.get(`${BASE_URL}/erettsegizzunk/Themes/get-temak-feladatonkent`);
        const themesForSubject = response.data[label];

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
        setThemeOptions([]);
      }
    } else {
      setThemeOptions([]);
      onFilterChange({ ...filters, themes: null });
    }
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
    <ScrollView style={styles.container}>
      <TouchableOpacity style={styles.toggleButton} onPress={() => setShowFilters(!showFilters)}>
        <FontAwesome name="filter" size={18} color="white" />
        <Text style={styles.toggleButtonText}>
          {showFilters ? " Szűrők elrejtése" : " Szűrők megjelenítése"}
        </Text>
      </TouchableOpacity>

      {showFilters && (
        <View style={styles.filterBox}>
          <Text style={styles.label}>Keresés szöveg alapján</Text>
          <TextInput
            style={styles.input}
            placeholder="Keresés feladatleírásban..."
            value={filters.searchText}
            onChangeText={(text) => onFilterChange({ ...filters, searchText: text })}
          />

          <Text style={styles.label}>Tantárgy</Text>
          <RNPickerSelect
            placeholder={{ label: "Válassz tantárgyat...", value: null }}
            items={subjectOptions}
            onValueChange={(value, index) =>
              handleSubjectsChange(value, subjectOptions[index - 1]?.label)
            }
            value={filters.subjects?.value || null}
          />

          <Text style={styles.label}>Nehézség</Text>
          <RNPickerSelect
            placeholder={{ label: "Válassz nehézséget...", value: null }}
            items={difficultyOptions}
            onValueChange={(value) =>
              onFilterChange({ ...filters, difficulty: difficultyOptions.find(d => d.value === value) })
            }
            value={filters.difficulty?.value || null}
          />

          <Text style={styles.label}>Téma</Text>
          <RNPickerSelect
            placeholder={{ label: "Válassz témát...", value: null }}
            items={themeOptions}
            onValueChange={(value) =>
              onFilterChange({ ...filters, themes: themeOptions.find(t => t.value === value) })
            }
            value={filters.themes?.value || null}
          />

          <View style={styles.buttonRow}>
            <Button title="Szűrés alkalmazása" onPress={onApplyFilters} color="#28a745" />
            <Button title="Szűrők törlése" onPress={clearFilters} color="#dc3545" />
          </View>
        </View>
      )}
    </ScrollView>
  );
};

const getStyles = (theme) => 
  StyleSheet.create({
    container: {
      padding: 10,
    },
    toggleButton: {
      flexDirection: "row",
      backgroundColor: theme,
      padding: 10,
      borderRadius: 8,
      alignItems: "center",
      marginBottom: 10,
    },
    toggleButtonText: {
      color: "white",
      marginLeft: 10,
      fontWeight: "bold",
    },
    filterBox: {
      backgroundColor: theme,
      padding: 15,
      borderRadius: 10,
    },
    label: {
      color: "white",
      marginTop: 10,
      marginBottom: 5,
    },
    input: {
      backgroundColor: "white",
      paddingHorizontal: 10,
      paddingVertical: 8,
      borderRadius: 5,
    },
    buttonRow: {
      marginTop: 20,
      gap: 10,
    },
  });