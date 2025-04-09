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

        const getCorrectAnswers = (task) => {
          if (task.type.name === "textbox") {
            if (task.isCorrect.split("|").length === 1) {
              if (task.isCorrect.includes(";")) {
                return { __html: task.answers };
              }
              return { __html: task.isCorrect || "-" };
            }
            return {
              __html: task.isCorrect
                .split("|")
                .map((textboxData) => {
                  const [text, values] = textboxData.split("_");
                  const validIndexes = values
                    .split(";")
                    .map((v, i) => (v === "1" ? i : -1))
                    .filter((i) => i !== -1);
                  const correctAnswers = validIndexes
                    .map((i) => task.answers.split(";")[i])
                    .join(", ");
                  return `${text}${correctAnswers}`;
                })
                .join(", "),
            };
          }
          const correctAnswers = task.answers
            .split(";")
            .filter((_, i) => task.isCorrect.split(";")[i] === "1")
            .join(", ");
          return { __html: correctAnswers || "-" };
        };

        const getUserAnswers = (task) => {
          if (task.type.name === "textbox") {
            if (task.values.length === 1) {
              return { __html: task.values[0] || "-" };
            }
            return {
              __html: task.isCorrect
                .split("|")
                .map((textboxData, textboxIndex) => {
                  const [text] = textboxData.split("_");
                  const userAnswers =
                    task.values[textboxIndex]?.split(",").map((ans) => ans.trim()) || [];
                  return `${text}${userAnswers.join(", ") || "-"}`;
                })
                .join(", "),
            };
          }
          const userAnswers = task.answers
            .split(";")
            .filter((_, i) => task.values[i] === "1")
            .join(", ");
          return { __html: userAnswers || "Nem válaszoltál" };
        };

        const correct = getCorrectAnswers(exercise).__html;
        const userResponse = getUserAnswers(task).__html;

        const isCorrect =
          task.type.name === "textbox" && correct.includes(";")
            ? correct
                .split(";")
                .some((correctAnswer) =>
                  correctAnswer.trim().toLowerCase().includes(userResponse.replace(/<\/?b>/g, "").trim().toLowerCase())
                )
            : correct.replace(/<\/?b>/g, "").toLowerCase() === userResponse.replace(/<\/?b>/g, "").toLowerCase();

        acc[exercise.id] = isCorrect;
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