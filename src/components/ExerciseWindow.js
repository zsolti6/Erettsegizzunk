import React from "react";
import TaskComponent from "./TaskComponent";

function ExerciseWindow({ tasks, activeTask, taskValues, updateTaskValues }) {
  return (
    <div className="exercise">
      {activeTask ? (
        <TaskComponent
          elem={activeTask}
          values={taskValues[activeTask.id]?.values || []}
          updateValues={(newValues) => updateTaskValues(activeTask.id, newValues)}
        />
      ) : (
        <p>Válassz feladatot.</p>
      )}
    </div>
  );
}

export default ExerciseWindow;
