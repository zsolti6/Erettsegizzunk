import { useEffect, useContext, useCallback } from 'react';
import { ImageBackground, View, Text, StyleSheet, FlatList, Alert, TouchableOpacity } from 'react-native';
import { useRoute, useNavigation } from '@react-navigation/native';
import axios from 'axios';
const BASE_URL = "https://erettsegizzunk.onrender.com";
import { ThemeContext } from './ThemeContext';

export default TestStatisticsScreen = () => {
  const route = useRoute();
  const navigation = useNavigation();
  const { taskValues, exercises } = route.params || {};
  const { user } = route.params.user || {};
  const { theme } = useContext(ThemeContext);
  const styles = getStyles(theme);
  const sortedTaskValues = exercises?.map(ex => taskValues[ex.id]) || [];

  const getCorrectAnswers = useCallback((exercise) => {
    if (!exercise?.isCorrect) return "-";
    const answers = exercise.answers?.split(";") || [];
    const correctPatterns = exercise.isCorrect.split("|");
    const allCorrect = correctPatterns.flatMap(pattern => {
      const cleanPattern = pattern.includes('_') 
        ? pattern.split('_')[1] 
        : pattern;
      return cleanPattern.split(";")
        .map((val, i) => val.trim() === "1" ? answers[i] : null)
        .filter(Boolean);
    });
    return allCorrect.join(", ") || "-";
  }, []);

  const getUserAnswers = useCallback((task, exercise) => {
    if (exercise?.type?.name === "textbox") {
      return exercise.isCorrect.split("|").map((_, textboxIndex) => {
        const userAnswers = task.values && task.values[textboxIndex] 
          ? task.values[textboxIndex].split(",").map(ans => ans.trim())
          : [];
        return userAnswers.join(", ") || "-";
      }).join(", ");
    }
    const answers = exercise.answers?.split(";") || [];
    const userValues = Array.isArray(task.values) ? task.values : [];
    const userAnswers = userValues
      .map((val, i) => val === "1" ? answers[i] : null)
      .filter(Boolean);
    return userAnswers.length > 0 
      ? userAnswers.join(", ") 
      : "Nem válaszoltál";
  }, []);

  const isCorrect = useCallback((task, exercise) => {
    if (!exercise || !task) return false;
    if (exercise.type?.name === "textbox") {
      const correctAnswers = getCorrectAnswers(exercise).split(", ");
      const userAnswers = getUserAnswers(task, exercise).split(", ");
      if (exercise.isCorrect.split(";").every(val => val === "1")) {
        return userAnswers.some(userAns => 
          correctAnswers.some(correctAns => correctAns.trim() === userAns.trim())
        );
      }
      return correctAnswers.length === userAnswers.length && 
        correctAnswers.every((ans, i) => ans.trim() === userAnswers[i].trim());
    }
    const correctPattern = exercise.isCorrect?.split(";") || [];
    const userPattern = Array.isArray(task.values) 
      ? task.values 
      : Object.values(task.values || {}).map(v => v === "1" ? "1" : "0");
    if (correctPattern.every(val => val === "1")) {
      return userPattern.some((val, i) => val === "1");
    }
    return correctPattern.length === userPattern.length && 
      correctPattern.every((val, i) => val === (userPattern[i] || "0"));
  }, [getCorrectAnswers, getUserAnswers]);

  const getCorrectAnswersText = useCallback((exercise) => {
    if (!exercise?.isCorrect) return "-";
    if (!exercise.isCorrect.includes('|') && !exercise.isCorrect.includes('_')) {
        const answers = exercise.answers?.split(";") || [];
        const correctIndexes = exercise.isCorrect.split(";")
            .map((val, i) => val.trim() === "1" ? answers[i] : null)
            .filter(Boolean);
        return correctIndexes.join(", ") || "-";
    }
    const answers = exercise.answers?.split(";") || [];
    const correctPatterns = exercise.isCorrect.split("|");
    const formattedResults = correctPatterns.map(pattern => {
        if (pattern.includes('_')) {
            const [prefix, values] = pattern.split('_');
            const correctItems = values.split(";")
                .map((val, i) => val.trim() === "1" ? answers[i] : null)
                .filter(Boolean)
                .join(", ");
            return `${prefix.trim()}${correctItems}`;
        } else {
            const correctItems = pattern.split(";")
                .map((val, i) => val.trim() === "1" ? answers[i] : null)
                .filter(Boolean)
                .join(", ");
            return correctItems;
        }
    }).filter(part => part.trim() !== "");
    return formattedResults.join(", ") || "-";
  }, []);

  useEffect(() => {
    const sendStatistics = async () => {
      if (!user || user.isGuest) {
        return Alert.alert(
          "Hiba", 
          `Nem található user: ${JSON.stringify(user)}`
        );
      }
      const taskResults = {};
      exercises.forEach(exercise => {
        const task = taskValues[exercise.id];
        taskResults[parseInt(exercise.id)] = isCorrect(task, exercise);
      });
      const postData = {
        token: user.token,
        userId: user.id,
        taskIds: taskResults
      };
      try {
        await axios.post(
          `${BASE_URL}/erettsegizzunk/UserStatistics/post-user-statistics`, 
          postData,
          {
            headers: {
              'Content-Type': 'application/json',
              'Accept': 'application/json'
            }
          }
        );
      } catch (error) {
        Alert.alert("Hiba történt", 
          `Nem sikerült elküldeni az eredményeket: ${error.message}`
        );
      }
    };
    sendStatistics();
  }, [user, taskValues, exercises, isCorrect]);

  const renderItem = ({ item: task, index }) => {
    const exercise = exercises.find(ex => ex.id === task?.taskId) || {};
    const correct = isCorrect(task, exercise);
    return (
      <View style={[styles.taskContainer, correct ? styles.correctTask : styles.incorrectTask]}>
        <View style={styles.taskHeader}>
          <Text style={styles.taskId}>{index + 1}</Text>
          <Text style={styles.taskEvaluation}>
            {correct ? '✅' : '❌'}
          </Text>
        </View>
        <Text style={styles.description}>{exercise.description}</Text>
        <Text style={styles.text}>{exercise.text}</Text>
        <View style={styles.answerSection}>
          <Text style={styles.answerLabel}>Helyes válasz:</Text>
          <Text style={styles.correctAnswer}>{getCorrectAnswersText(exercise)}</Text>
        </View>
        <View style={styles.answerSection}>
          <Text style={styles.answerLabel}>Te válaszod:</Text>
          <Text style={styles.userAnswer}>{getUserAnswers(task, exercise)}</Text>
        </View>
      </View>
    );
  };

  const renderFooter = () => (
    <TouchableOpacity 
      style={[styles.backButton, { backgroundColor: theme }]}
      onPress={() => navigation.navigate('MainMenu')}
    >
      <Text style={styles.backButtonText}>Vissza a főoldalra</Text>
    </TouchableOpacity>
  );

  return (
    <View style={styles.container}>
      <ImageBackground source={require('../background.jpg')} style={styles.background} resizeMode="cover" >
        <View style={styles.innerContainer}>
          <Text style={styles.header}>Feladatok összegzése</Text>
          <FlatList
            data={sortedTaskValues}
            renderItem={renderItem}
            keyExtractor={(item, index) => index.toString()}
            contentContainerStyle={styles.listContent}
            ListFooterComponent={renderFooter}
          />
        </View>
      </ImageBackground>
    </View>
  );
};

const getStyles = (theme) => StyleSheet.create({
  container: {
    flex: 1,
  },
  header: {
    color: 'white',
    fontSize: 24,
    fontWeight: 'bold',
    textAlign: 'center',
    marginBottom: 20,
    marginTop: 10,
  },
  innerContainer: {
    padding: 10,
  },
  listContent: {
    paddingBottom: 20,
  },
  taskContainer: {
    backgroundColor: theme,
    borderRadius: 10,
    padding: 15,
    marginBottom: 15,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.3,
    shadowRadius: 3,
    elevation: 5,
  },
  correctTask: {
    borderLeftWidth: 5,
    borderLeftColor: '#2ecc71',
  },
  incorrectTask: {
    borderLeftWidth: 5,
    borderLeftColor: '#e74c3c',
  },
  taskHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 10,
  },
  taskId: {
    color: 'white',
    fontSize: 24,
    fontWeight: 'bold',
  },
  taskEvaluation: {
    fontSize: 24,
  },
  description: {
    color: 'white',
    fontWeight: 'bold',
    fontSize: 16,
    marginBottom: 5,
  },
  text: {
    color: 'white',
    marginBottom: 15,
  },
  answerSection: {
    marginBottom: 10,
  },
  answerLabel: {
    color: '#bdc3c7',
    fontSize: 14,
    marginBottom: 3,
  },
  correctAnswer: {
    color: '#2ecc71',
    fontSize: 16,
  },
  userAnswer: {
    color: '#f39c12',
    fontSize: 16,
  },
  background: {
    flex: 1,
    width: '100%',
    height: '100%',
  },
  backButton: {
    padding: 15,
    borderRadius: 10,
    alignItems: 'center',
    justifyContent: 'center',
    marginTop: 10,
    marginBottom: 50,
  },
  backButtonText: {
    color: 'white',
    fontSize: 18,
    fontWeight: 'bold',
  },
});