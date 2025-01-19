import React, { useState, useEffect } from "react";
import axios from "axios";
import { useLocation, useNavigate } from "react-router-dom";
import Navbar from "./Navbar";
import Sidenav from "./SideNav";
import ExerciseWindow from "./ExerciseWindow";

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

        const tasksWithIds = response.data.map((task, index) => ({
          ...task,
          taskId: index + 1,
        }));

        setExercises(tasksWithIds);

        const initialValues = {};
        tasksWithIds.forEach((task) => {
          initialValues[task.id] = {
            taskId: task.taskId,
            helyese: task.helyese,
            megoldasok: task.megoldasok,
            values: task.tipus.nev === "textbox" ? [""] : Array(task.helyese.split(";").length).fill("0"),
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
      <div style={{ display: "flex" }}>
        <Sidenav tasks={exercises} setActiveComponent={setActiveIndex} activeIndex={activeIndex} />
        <div style={{ padding: "20px", flex: 1 }}>
          {exercises.length > 0 && (
            <ExerciseWindow
              tasks={exercises}
              activeTask={exercises[activeIndex]}
              taskValues={taskValues}
              updateTaskValues={updateTaskValues}
            />
          )}
          <div>
            {activeIndex > 0 && <button onClick={() => setActiveIndex(activeIndex - 1)}>Előző feladat</button>}
            {activeIndex < exercises.length - 1 ? (
              <button onClick={() => setActiveIndex(activeIndex + 1)}>Következő feladat</button>
            ) : (
              <button onClick={() => navigate("/exercise/stats", { state: { taskValues } })}>Feladatlap befejezése</button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

export default ExerciseComponent;
