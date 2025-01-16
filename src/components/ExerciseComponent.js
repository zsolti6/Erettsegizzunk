import React, { useEffect, useState } from "react";
import "../css/SubPage.css";
import Sidenav from "./SideNav";
import Navbar from "./Navbar";
import ExerciseWindow from "./ExerciseWindow";
import axios from "axios";
import { useLocation } from "react-router-dom";

function ExerciseComponent() {
  const [exercises, setExercises] = useState([]);
  const [activeIndex, setActiveIndex] = useState(0);

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

        // Assign taskId to each task
        const tasksWithIds = response.data.map((task, index) => ({
          ...task,
          taskId: index + 1, // Assuming taskId is 1-based index
        }));

        setExercises(tasksWithIds);
        console.log("Fetched exercises with taskId:", tasksWithIds);
      } catch (error) {
        console.error("Error during POST request:", error);
      }
    };

    fetchData();
  }, [subject, difficulty]);

  const handlePrevious = () => {
    setActiveIndex((prevIndex) => Math.max(prevIndex - 1, 0));
  };

  const handleNext = () => {
    setActiveIndex((prevIndex) =>
      Math.min(prevIndex + 1, exercises.length - 1)
    );
  };

  const handleTaskSelection = (taskId) => {
    const index = exercises.findIndex((task) => task.id === taskId);
    console.log("Task selected at index:", index);
    if (index !== -1) {
      setActiveIndex(index);
    }
  };

  return (
    <div style={{ height: "92vh" }}>
      <Navbar />
      <div style={{ display: "flex" }}>
        <Sidenav tasks={exercises} setActiveComponent={handleTaskSelection} />
        <div style={{ padding: "20px", flex: 1 }}>
          <ExerciseWindow
            tasks={exercises}
            activeTask={exercises[activeIndex]}
            onPrevious={handlePrevious}
            onNext={handleNext}
            activeIndex={activeIndex}
            totalTasks={exercises.length}
          />
        </div>
      </div>
    </div>
  );
}

export default ExerciseComponent;
