import React, { useState, useEffect } from "react";
import axios from "axios";
import { useLocation, useNavigate } from "react-router-dom";
import Navbar from "./Navbar";
import Sidenav from "./SideNav";
import ExerciseWindow from "./ExerciseWindow";
import "bootstrap-icons/font/bootstrap-icons.css";
import "../css/taskStyle.css";

function ExerciseComponent() {
  const [exercises, setExercises] = useState([]);
  const [activeIndex, setActiveIndex] = useState(0);
  const [taskValues, setTaskValues] = useState({});
  const navigate = useNavigate();

  const location = useLocation();
  const { subject, difficulty } = location.state || {
    subject: "történelem",
    difficulty: "közép",
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const postData = { tantargy: subject, szint: difficulty };
        
        const response = await axios.post(
          "https://localhost:7066/erettsegizzunk/Feladatok/get-random-feladatok",
          postData
        );
        console.log(response.data);
        
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
            values: task.type.name === "textbox" ? [""] : Array(task.isCorrect.split(";").length).fill("0"),
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
    <div style={{ height: "92vh" }}>
      <Navbar />
      <div style={{ display: "flex", height: "100%" }}>
        <Sidenav tasks={exercises} setActiveComponent={setActiveIndex} activeIndex={activeIndex} />
        <div style={{ padding: "20px", flex: 1, display: "flex", alignItems: "center", backgroundColor: "blue", height: "100%", zIndex: 10, marginLeft: "26vh"}}>
          <div style={{ marginRight: "10px" }} id="previous">
            {activeIndex > 0 && (
              <button className="btn btn-primary" onClick={() => setActiveIndex(activeIndex - 1)}>
                <i className="bi bi-arrow-left"></i>
              </button>
            )}
          </div>
          {exercises.length > 0 && (
            <ExerciseWindow
              tasks={exercises}
              activeTask={exercises[activeIndex]}
              taskValues={taskValues}
              updateTaskValues={updateTaskValues}
            />
          )}
          <div style={{ marginLeft: "10px" }} id="next">
            {activeIndex < exercises.length - 1 ? (
              <button className="btn btn-primary" onClick={() => setActiveIndex(activeIndex + 1)}>
                <i className="bi bi-arrow-right"></i>
              </button>
            ) : (
              <button
                id="taskDone"
                className="btn btn-success"
                onClick={() => navigate("/exercise/stats", { state: { taskValues, exercises } })}
              >
                <i className="bi bi-check-circle"></i>
              </button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

export default ExerciseComponent;
