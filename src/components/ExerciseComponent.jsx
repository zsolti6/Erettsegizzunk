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
  const [isOpen, setIsOpen] = useState(false); // Sidebar visibility
  const navigate = useNavigate();
  const location = useLocation();
  const { subject, difficulty, subjectId } = location.state || {
    subject: "",
    difficulty: "",
    subjectId: 0,
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const postData = { tantargy: subject, szint: difficulty };
        const response = await axios.post(
          `${BASE_URL}/erettsegizzunk/Feladatok/get-random-feladatok`,
          postData
        );
        const tasksWithIds = response.data.map((task, index) => ({
          ...task,
          taskId: index + 1,
        }));
        setExercises(tasksWithIds);

        const initialValues = {};
        tasksWithIds.forEach((task) => {
          initialValues[task.id] = {
            taskId: task.taskId,
            isCorrect: task.isCorrect,
            answers: task.answers,
            values:
              task.type.name === "textbox"
                ? [""]
                : Array(task.isCorrect.split(";").length).fill("0"),
          };
        });
        setTaskValues(initialValues);
      } catch (error) {
        console.error("Error during POST request:", error);
      }
    };

    fetchData();
  }, [subject, difficulty]);

  const updateTaskValues = (taskId, newValues) => {
    setTaskValues((prev) => ({
      ...prev,
      [taskId]: {
        ...prev[taskId],
        values: Array.isArray(newValues) ? newValues : [newValues],
      },
    }));
  };

  return (
    <div className="d-flex flex-column" style={{ minHeight: "92vh", paddingTop: "60px" }}>
      {/* Sidebar Toggle Button (Visible on Mobile) */}
      <button
        className="btn btn-primary d-lg-none m-2"
        onClick={() => setIsOpen(!isOpen)}
        style={{ width: "fit-content", zIndex: 1000, position: "fixed", top: "70px", right: "10px" }}
      >
        <i className={`bi bi-${isOpen ? "x" : "list"}`}></i>
      </button>

      <div className="d-flex flex-grow-1">
        {/* Sidebar */}
        <div
          className={`sidenav bg-light ${isOpen ? "open" : "d-none d-lg-block"}`}
          style={{ width: "250px", transition: "transform 0.3s ease", paddingTop: "60px" }}
        >
          <div className="d-flex justify-content-end p-2">
            <button className="btn btn-sm btn-secondary d-lg-none" onClick={() => setIsOpen(!isOpen)}>
              <i className="bi bi-x"></i>
            </button>
          </div>
          <Sidenav
            tasks={exercises}
            isOpen={isOpen}
            setIsOpen={setIsOpen}
            setActiveComponent={setActiveIndex}
            activeIndex={activeIndex}
          />
        </div>

        {/* Main Content */}
        <div
          className="flex-grow-1 p-3"
          style={{
            marginLeft: isOpen ? "250px" : "0",
            transition: "margin-left 0.3s ease",
          }}
        >
          <div className="d-flex justify-content-center align-items-center flex-column">
            {/* Exercise Window */}
            {exercises.length > 0 && (
              <ExerciseWindow
                tasks={exercises}
                activeTask={exercises[activeIndex]}
                taskValues={taskValues}
                updateTaskValues={updateTaskValues}
              />
            )}

            {/* Navigation Buttons (Stacked on large, inline on small screens) */}
            <div className="d-flex justify-content-center gap-2 flex-wrap mt-3">
              {activeIndex > 0 && (
                <button className="btn btn-primary" onClick={() => setActiveIndex(activeIndex - 1)}>
                  Előző feladat
                </button>
              )}

              {activeIndex < exercises.length - 1 ? (
                <button className="btn btn-primary" onClick={() => setActiveIndex(activeIndex + 1)}>
                  Következő feladat
                </button>
              ) : (
                <button
                  className="btn btn-success"
                  onClick={() =>
                    navigate("/gyakorlas/statisztika", { state: { taskValues, exercises, subjectId } })
                  }
                >
                  Feladat leadása
                </button>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
