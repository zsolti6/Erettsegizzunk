import React, { useState, useEffect } from "react";
import axios from "axios";
import { useLocation, useNavigate } from "react-router-dom";
import { Sidenav } from "./SideNav";
import { ExerciseWindow } from "./ExerciseWindow";
import "bootstrap-icons/font/bootstrap-icons.css";
import "../css/taskStyle.css";
import { BASE_URL } from "../config";

export const ExerciseComponent = () => {
  const [exercises, setExercises] = useState([]);
  const [activeIndex, setActiveIndex] = useState(0);
  const [taskValues, setTaskValues] = useState({});
  const [isOpen, setIsOpen] = useState(false);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const location = useLocation();
  const { subject, difficulty, subjectId, savedExercises, savedTaskValues } = location.state || {};

  useEffect(() => {
    const fetchData = async () => {
      if (savedExercises && savedTaskValues) {
        setExercises(savedExercises);
        setTaskValues(savedTaskValues);
        setLoading(false);
      } else {
        try {
          const response = await axios.post(`${BASE_URL}/erettsegizzunk/Feladatok/get-random-feladatok`, {
            tantargy: subject,
            szint: difficulty,
          });

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
        } catch (error) {
          console.error("Error fetching exercises:", error);
        } finally {
          setLoading(false);
        }
      }
    };

    fetchData();
  }, [subject, difficulty, savedExercises, savedTaskValues]);

  useEffect(() => {
    const saveToLocalStorage = () => {
      localStorage.setItem("exercises", JSON.stringify(exercises));
      localStorage.setItem("taskValues", JSON.stringify(taskValues));
    };

    window.addEventListener("beforeunload", saveToLocalStorage);
    return () => {
      window.removeEventListener("beforeunload", saveToLocalStorage);
    };
  }, [exercises, taskValues]);

  useEffect(() => {
    const saveToLocalStorage = () => {
      localStorage.setItem("exercises", JSON.stringify(exercises));
      localStorage.setItem("taskValues", JSON.stringify(taskValues));
    };

    saveToLocalStorage();
  }, [exercises, taskValues]);

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

  const getCorrectAnswers = (task) => {
    const isCorrectArray = task.isCorrect.split(";");
    const answersArray = task.answers.split(";");
    return answersArray.filter((_, index) => isCorrectArray[index] === "1").join(", ");
  };

  const getUserAnswers = (task) => {
    if (task.isCorrect === "1;") return task.values;
    const guessArray = task.values;
    const answersArray = task.answers.split(";");
    return answersArray.filter((_, index) => guessArray[index] === "1").join(", ") || "Nem válaszoltál";
  };

  const sendStatistics = async () => {
    const user = getUser();
    if (user) {
      const taskCorrects = Object.values(taskValues).reduce((acc, task) => {
        const exercise = exercises.find((ex) => ex.taskId === task.taskId);
        if (!exercise) return acc;

        acc[exercise.id] = getCorrectAnswers(task) === getUserAnswers(task);
        return acc;
      }, {});

      try {
        await axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/post-user-statistics`, {
          userId: user.id,
          token: user.token,
          taskIds: taskCorrects,
        });
        console.log("Statistics sent successfully");
      } catch (error) {
        console.error("Error sending statistics:", error);
      }
    }

    // Clear local storage after sending statistics
    localStorage.removeItem("exercises");
    localStorage.removeItem("taskValues");

    // Navigate to the statistics page
    navigate("/gyakorlas/statisztika", { state: { taskValues, exercises, subjectId } });
  };

  return (
    <div className="d-flex" style={{ minHeight: "92vh", paddingTop: "60px" }}>
      <button
        className="btn btn-primary m-2"
        onClick={() => setIsOpen(!isOpen)}
        style={{
          position: "fixed",
          top: "70px",
          left: isOpen ? "260px" : "10px",
          zIndex: 1001,
          transition: "left 0.3s",
        }}
      >
        <i className={`bi bi-${isOpen ? "x" : "list"}`}></i>
      </button>

      <div
        className="sidenav bg-light"
        style={{
          width: "250px",
          position: "fixed",
          left: isOpen ? "0" : "-250px",
          transition: "left 0.3s",
          zIndex: 1000,
          height: "100vh",
        }}
      >
        <Sidenav tasks={exercises} isOpen={isOpen} setIsOpen={setIsOpen} setActiveComponent={setActiveIndex} activeIndex={activeIndex} taskValues={taskValues} />
      </div>

      <div
        className="flex-grow-1 p-3"
        style={{
          marginLeft: isOpen && window.innerWidth >= 992 ? "250px" : "0",
          transition: "margin-left 0.3s",
        }}
      >
        <div className="d-flex justify-content-center align-items-center flex-column">
          {loading ? (
            <div className="spinner-container">
              <div className="spinner"></div>
            </div>
          ) : (
            <>
              {exercises.length > 0 && (
                <ExerciseWindow tasks={exercises} activeTask={exercises[activeIndex]} taskValues={taskValues} updateTaskValues={updateTaskValues} />
              )}

              <div className="d-flex justify-content-center gap-2 flex-wrap mt-3">
                {activeIndex > 0 && <button className="btn btn-primary" onClick={() => setActiveIndex(activeIndex - 1)}>Előző feladat</button>}
                {activeIndex < exercises.length - 1 ? (
                  <button className="btn btn-primary" onClick={() => setActiveIndex(activeIndex + 1)}>Következő feladat</button>
                ) : (
                  <button
                    className="btn btn-success"
                    onClick={async () => {
                      await sendStatistics();
                    }}
                  >
                    Feladatok leadása
                  </button>
                )}
              </div>
            </>
          )}
        </div>
      </div>
    </div>
  );
};
