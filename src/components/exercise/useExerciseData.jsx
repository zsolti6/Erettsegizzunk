import { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { BASE_URL } from '../../config';
import { useLocalStorage } from './useLocalStorage';

export const useExerciseData = () => {
  const [exercises, setExercises] = useState([]);
  const [activeIndex, setActiveIndex] = useState(0);
  const [taskValues, setTaskValues] = useState({});
  const [isOpen, setIsOpen] = useState(true);
  const [loading, setLoading] = useState(true);
  const [messageModal, setMessageModal] = useState({ show: false, type: "", message: "" }); // State for modal
  const navigate = useNavigate();
  const location = useLocation();
  const { subject, difficulty, subjectId, savedExercises, savedTaskValues, themeIds } = location.state || {};
  const { saveToLocalStorage } = useLocalStorage(exercises, taskValues);

  useEffect(() => {
    const fetchData = async () => {
      if (savedExercises && savedTaskValues) {
        setExercises(savedExercises);
        setTaskValues(savedTaskValues);
        setLoading(false);
      } else {
        try {
          const requestData = {
            tantargy: subject,
            szint: difficulty,
          };

          if (themeIds && themeIds.length > 0) {
            requestData.Themes = themeIds;
          }

          const response = await axios.post(`${BASE_URL}/erettsegizzunk/Feladatok/get-random-feladatok`, requestData);

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
              type: task.type
            };
            return acc;
          }, {});
          setTaskValues(initialValues);
        } catch (error) {
          setMessageModal({
            show: true,
            type: "error",
            message: "Hiba történt a feladatok betöltése során.",
          });
        } finally {
          setLoading(false);
        }
      }
    };

    fetchData();
  }, [subject, difficulty, savedExercises, savedTaskValues, themeIds]);

  const updateTaskValues = (taskId, newValues) => {
    setTaskValues((prev) => ({
      ...prev,
      [taskId]: {
        ...prev[taskId],
        values: Array.isArray(newValues) ? newValues : [newValues],
      },
    }));
  };

  const getUser = () => {
    return localStorage.getItem("rememberMe") === "true"
      ? JSON.parse(localStorage.getItem("user"))
      : JSON.parse(sessionStorage.getItem("user"));
  };

  const sendStatistics = async () => {
    const user = getUser();
    if (user) {
      const taskCorrects = Object.values(taskValues).reduce((acc, task) => {
        const exercise = exercises.find((ex) => ex.taskId === task.taskId);
        if (!exercise) return acc;

        const correctAnswers = exercise.isCorrect.split(";");
        const answersArray = exercise.answers.split(";");
        const correct = answersArray.filter((_, index) => correctAnswers[index] === "1").join(", ");
        
        const userAnswers = task.values;
        const userResponse = answersArray.filter((_, index) => userAnswers[index] === "1").join(", ") || "Nem válaszoltál";

        acc[exercise.id] = correct === userResponse;
        return acc;
      }, {});

      try {
        await axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/post-user-statistics`, {
          userId: user.id,
          token: user.token,
          taskIds: taskCorrects,
        });
        setMessageModal({
          show: true,
          type: "success",
          message: "Statisztikák sikeresen mentve!",
        });
      } catch (error) {
        setMessageModal({
          show: true,
          type: "error",
          message: "Hiba történt a statisztikák mentése során.",
        });
      }
    }

    localStorage.removeItem("exercises");
    localStorage.removeItem("taskValues");

    navigate("/gyakorlas/statisztika", { state: { taskValues, exercises, subjectId } });
  };

  const closeModal = () => {
    setMessageModal({ ...messageModal, show: false });
  };

  return {
    exercises,
    activeIndex,
    taskValues,
    isOpen,
    loading,
    setActiveIndex,
    setIsOpen,
    updateTaskValues,
    sendStatistics,
    messageModal,
    setMessageModal,
    closeModal,
  };
};