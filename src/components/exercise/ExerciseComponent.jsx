import React, { useEffect } from "react";
import { ExerciseWindow } from "./ExerciseWindow";
import { ExerciseControls } from "./ExerciseControls";
import { ExerciseSidebar } from "./ExerciseSidebar";
import { useExerciseData } from "./useExerciseData";
import "../../css/taskStyle.css";
import { useNavigate } from "react-router-dom";

export const ExerciseComponent = () => {
  const {
    exercises,
    activeIndex,
    taskValues,
    isOpen,
    loading,
    setActiveIndex,
    setIsOpen,
    updateTaskValues,
    sendStatistics,
  } = useExerciseData();

  return (
    <div className="d-flex" style={{ minHeight: "100vh", paddingTop: "30px" }}>
      <ExerciseSidebar
        isOpen={isOpen}
        setIsOpen={setIsOpen}
        exercises={exercises}
        activeIndex={activeIndex}
        setActiveIndex={setActiveIndex}
        taskValues={taskValues}
      />

      <div
        className="flex-grow-1 p-3"
        style={{
          marginLeft: isOpen && window.innerWidth >= 992 ? "250px" : "0",
          transition: "margin-left 0.3s",
        }}
      >
        <div
          className="d-flex justify-content-center align-items-center flex-column"
          style={{ minHeight: "100vh" }}
        >
          {loading ? (
            <div className="spinner-container">
              <div className="spinner"></div>
            </div>
          ) : (
            <div className="h-25">
              {exercises.length > 0 && (
                <ExerciseWindow
                  tasks={exercises}
                  activeTask={exercises[activeIndex]}
                  taskValues={taskValues}
                  updateTaskValues={updateTaskValues}
                />
              )}

              <ExerciseControls
                activeIndex={activeIndex}
                exercises={exercises}
                setActiveIndex={setActiveIndex}
                sendStatistics={sendStatistics}
              />
            </div>
          )}
        </div>
      </div>
    </div>
  );
};
