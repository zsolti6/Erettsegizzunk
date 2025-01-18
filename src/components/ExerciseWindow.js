import React from "react";
import TaskComponent from "./TaskComponent";

function ExerciseWindow({
  activeTask,
  onPrevious,
  onNext,
  activeIndex,
  totalTasks,
  onCompletion,
}) {
  return (
    <div className="exercise">
      {activeTask ? (
        <div>
          <TaskComponent
            elem={activeTask}
            onNavigatePrevious={onPrevious}
            onNavigateNext={onNext}
            activeIndex={activeIndex}
            totalTasks={totalTasks}
            onCompletion={onCompletion}
          />
        </div>
      ) : (
        <p>Válassz feladatokat.</p>
      )}
    </div>
  );
}

export default ExerciseWindow;
