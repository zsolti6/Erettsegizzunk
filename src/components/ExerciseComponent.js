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
  const [feedback, setFeedback] = useState({});

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
    if (index !== -1) {
      setActiveIndex(index);
    }
  };

  const handleCompletion = (checkedState) => {
    const newFeedback = exercises.reduce((acc, task) => {
      const correctAnswers = task.helyese.split(";");
      const userAnswers = checkedState[task.id] || {};

      const isCorrect = correctAnswers.every((answer, idx) => {
        if (task.tipus.nev === "radio" || task.tipus.nev === "checkbox") {
          const userChecked = userAnswers[task.tipus.nev] || [];
          return userChecked.includes(String(idx)) === (answer === "1");
        }
        if (task.tipus.nev === "textbox") {
          const userTextboxes = userAnswers.textboxes || [];
          return userTextboxes[idx] === task.megoldasok.split(";")[idx];
        }
        return false;
      });

      acc[task.id] = isCorrect ? "correct" : "incorrect";
      return acc;
    }, {});

    setFeedback(newFeedback);
  };

  return (
    <div style={{ height: "92vh" }}>
      <Navbar />
      <div style={{ display: "flex" }}>
        <Sidenav
          tasks={exercises}
          setActiveComponent={handleTaskSelection}
          feedback={feedback}
          activeIndex={activeIndex}
        />
        <div style={{ padding: "20px", flex: 1 }}>
          <ExerciseWindow
            tasks={exercises}
            activeTask={exercises[activeIndex]}
            onPrevious={handlePrevious}
            onNext={handleNext}
            activeIndex={activeIndex}
            totalTasks={exercises.length}
            onCompletion={handleCompletion}
          />
        </div>
      </div>
    </div>
  );
}

export default ExerciseComponent;
