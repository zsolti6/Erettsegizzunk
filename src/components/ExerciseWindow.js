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
        <p>Select a task to view details</p>
      )}
    </div>
  );
}

export default ExerciseWindow;
