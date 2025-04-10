import { useEffect, useState, useContext } from 'react';
import { ImageBackground, View, Text, StyleSheet, FlatList, ActivityIndicator, TouchableOpacity, TextInput } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import axios from 'axios';
import { LinearGradient } from 'expo-linear-gradient';
import { ThemeContext } from './ThemeContext';
const BASE_URL = "https://erettsegizzunk.onrender.com";

export default function SelectorScreen(user) {
  const { theme } = useContext(ThemeContext);
  const styles = getStyles(theme);
  const [subjects, setSubjects] = useState([]);
  const navigation = useNavigation();
  const [selectedSubject, setSelectedSubject] = useState(null);
  const [difficulty, setDifficulty] = useState('közép');
  const [themes, setThemes] = useState([]);
  const [filteredThemes, setFilteredThemes] = useState([]);
  const [selectedThemes, setSelectedThemes] = useState([]);
  const [themeFilter, setThemeFilter] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const subjectsResponse = await axios.get(`${BASE_URL}/erettsegizzunk/Tantargyak/get-tantargyak`);
        const formattedSubjects = subjectsResponse.data.map((subject) => ({
          id: subject.id,
          name: subject.name,
        }));
        setSubjects(formattedSubjects);
        if (formattedSubjects.length > 0) {
          setSelectedSubject(formattedSubjects[0]);
        }

        const themesResponse = await axios.get(`${BASE_URL}/erettsegizzunk/Themes/get-temak-feladatonkent`);
        setThemes(themesResponse.data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, []);

  useEffect(() => {
    if (selectedSubject) {
      const subjectThemes = themes[selectedSubject.name.toLowerCase()] || [];
      setFilteredThemes(subjectThemes);
    }
  }, [selectedSubject, themes]);

  const handleThemeSelect = (themeId) => {
    setSelectedThemes((prev) =>
      prev.includes(themeId) ? prev.filter((id) => id !== themeId) : [...prev, themeId]
    );
  };

  const handleStartExercise = () => {
    navigation.navigate('Test', {
      subject: selectedSubject.name,
      difficulty: difficulty,
      subjectId: selectedSubject.id,
      themeIds: selectedThemes,
      user: user,
    });
  };

  if (loading) {
    return (
      <View style={styles.container}>
        <ActivityIndicator size="large" color="#0000ff" />
      </View>
    );
  }

  if (error) {
    return (
      <View style={styles.container}>
        <Text style={styles.errorText}>Error: {error}</Text>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <ImageBackground source={require('../background.jpg')} style={styles.background} resizeMode="cover" >
        <View style={styles.rowContainer}>
          <View style={styles.column}>
            <Text style={styles.title}>Tantárgyak</Text>
            {subjects.map((subject) => (
              <TouchableOpacity
                key={subject.id}
                style={[styles.option, selectedSubject?.id === subject.id && styles.selectedOption]}
                onPress={() => setSelectedSubject(subject)}
              >
                <Text style={styles.optionText, selectedSubject?.id === subject.id && styles.selectedOptionText}>{subject.name}</Text>
              </TouchableOpacity>
            ))}
          </View>
          <View style={styles.column}>
            <Text style={styles.title}>Szintek</Text>
            {['közép', 'emelt'].map((level) => (
              <TouchableOpacity
                key={level}
                style={[styles.option, difficulty === level && styles.selectedOption]}
                onPress={() => setDifficulty(level)}
              >
                <Text style={styles.optionText, difficulty === level && styles.selectedOptionText}>{level}</Text>
              </TouchableOpacity>
            ))}
          </View>
        </View>

        <View style={styles.temakView}>
          <Text style={styles.title}>Témák</Text>
          <TextInput
            style={styles.input}
            placeholder="Szűrés témák szerint"
            value={themeFilter}
            onChangeText={setThemeFilter}
          />

          <View style={{ maxHeight: 250, position: 'relative' }}>
            <FlatList
              data={filteredThemes.filter((theme) => theme.theme.name.toLowerCase().includes(themeFilter.toLowerCase()))}
              keyExtractor={(item) => item.theme.id.toString()}
              renderItem={({ item }) => (
                <TouchableOpacity
                  style={[styles.option, selectedThemes.includes(item.theme.id) && styles.selectedOption]}
                  onPress={() => handleThemeSelect(item.theme.id)}
                >
                  <Text style={styles.optionText, selectedThemes.includes(item.theme.id) && styles.selectedOptionText}>{item.theme.name} ({item.count})</Text>
                </TouchableOpacity>
              )}
              showsVerticalScrollIndicator={false}
            />

            {/* Gradient Shadow at the Bottom */}
            <LinearGradient
              colors={['transparent', 'rgba(0,0,0,0.1)']}
              style={styles.gradient}
            />

            {/* Scroll Indicator */}
            {filteredThemes.length > 5 && (
              <Text style={styles.scrollHint}>Görgess le további témákért ↓</Text>
            )}
          </View>
        </View>

        {/* Start Exercise Button */}
        <TouchableOpacity style={styles.startButton} onPress={handleStartExercise}>
          <Text style={styles.startButtonText}>Feladatlap megkezdése</Text>
        </TouchableOpacity>
      </ImageBackground>
    </View>
  );
}

const getStyles = (theme) =>
  StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
  },
  background: {
    flex: 1,
    width: '100%',
    height: '100%',
  },
  rowContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    top: 50,
  },
  column: {
    flex: 1,
    padding: 10,
    marginHorizontal: 10,
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 5,
    },
    shadowOpacity: 0.34,
    shadowRadius: 6.27,

    elevation: 10,
  },
  title: {
    textAlign: 'center',
    fontSize: 20,
    fontWeight: 'bold',
    marginBottom: 10,
    color: '#fff',
  },
  option: {
    padding: 10,
    marginVertical: 5,
    backgroundColor: "#fff",
    borderRadius: 5,
    alignItems: 'center',
  },
  selectedOption: {
    backgroundColor: theme,
  },
  optionText: {
    fontSize: 16,
    color: '#000',
  },
  selectedOptionText: {
    fontSize: 16,
    color: '#fff',
  },
  input: {
    borderWidth: 1,
    borderColor: '#ccc',
    padding: 10,
    borderRadius: 5,
    marginBottom: 10,
    backgroundColor: '#fff',
  },
  startButton: {
    bottom: 50,
    backgroundColor: theme,
    padding: 15,
    borderRadius: 5,
    alignItems: 'center',
    marginLeft: '2%',
    marginRight: '2%',
  },
  startButtonText: {
    color: '#fff',
    fontSize: 18,
    fontWeight: 'bold',
  },
  errorText: {
    color: 'red',
    fontSize: 18,
  },
  gradient: {
    position: 'absolute',
    bottom: 0,
    left: 0,
    right: 0,
    height: 10,
  },
  temakView:{
    flex: 1,
    marginTop: 40,
    padding: '3%',
  },
  scrollHint: {
    position: 'relative',
    bottom: 0,
    alignSelf: 'center',
    fontSize: 12,
    fontStyle: 'italic',
    color: '#fff',
  },
});