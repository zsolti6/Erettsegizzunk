import React from "react";
import TaskComponent from "./TaskComponent";

function ExerciseWindow({ task }) {
  return (
    <div className="exercise">
      {/* Render the selected task */}
      {task ? (
        <div>
          <TaskComponent elem={task}/>
        </div>
      ) : (
        <p>Válassz feladatokat.</p>
      )}
    </div>
  );
}

export default ExerciseWindow;
