import React from "react";
import "../css/Sidenav.css";

function Sidenav({ tasks, setActiveComponent, feedback, activeIndex }) {
  return (
    <div className="sidenav">
      {tasks.map((task, index) => (
        <button
          key={task.id}
          className={`task-button ${feedback[task.id]} ${
            activeIndex === index ? "active" : ""
          }`}
          onClick={() => setActiveComponent(task.id)}
        >
          Feladat {task.taskId}
        </button>
      ))}
    </div>
  );
}

export default Sidenav;
