import { useRoute, useNavigation } from '@react-navigation/native';
import { useEffect, useState } from 'react';
import { Text, ActivityIndicator } from 'react-native';
import axios from 'axios';
const BASE_URL = "https://erettsegizzunk.onrender.com";

export default function TestScreen() {
  const route = useRoute();
  const navigation = useNavigation();
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [taskValues, setTaskValues] = useState({});
  const [exercises, setExercises] = useState([]);
  const { subject, difficulty, themeIds, user } = route.params || {};

  useEffect(() => {
    const fetchData = async () => {
      try {
        const requestData = {
          tantargy: subject,
          szint: difficulty,
        };
        if (themeIds && themeIds.length > 0) {
          requestData.Themes = themeIds;
        }
        const response = await axios.post(
          `${BASE_URL}/erettsegizzunk/Feladatok/get-random-feladatok`,
          requestData,
          { headers: { "Content-Type": "application/json" } }
        );

        const tasksWithIds = response.data.map((task, index) => ({
          ...task,
          taskId: index + 1,
        }));
        setExercises(tasksWithIds);

        const initialValues = tasksWithIds.reduce((acc, task) => {
            acc[task.id] = {
              taskId: task.taskId,
              isCorrect: task.isCorrect,
              answers: task.answers,
              values: task.type.name === "textbox" ? [""] : Array(task.isCorrect.split(";").length).fill("0"),
            };
            return acc;
          }, {});
        setTaskValues(initialValues);

        setData(response.data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, [subject, difficulty, themeIds]);

  useEffect(() => {
    if (data) {
      navigation.replace('CarouselScreen', { 
        data, 
        taskValues,
        user: user
      });
    }
  }, [data, navigation, taskValues, user]);

  if (loading) return <ActivityIndicator size="large" color="#007BFF" />;
  if (error) return <Text>{error}</Text>;

  return null;
}