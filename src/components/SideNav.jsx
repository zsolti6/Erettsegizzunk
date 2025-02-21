import React from "react";
import "../css/Sidenav.css";

export const Sidenav = ({ tasks, setActiveComponent, activeIndex }) => {
  return (
    <div className="sidenav">
      {tasks.map((task, index) => (
        <button id={("task"+task.taskId)}
          key={task.id}
          className={`task-button ${
            activeIndex === index ? "active" : ""
          }`}
          onClick={() => setActiveComponent(task.taskId - 1)}
        >
          Feladat {task.taskId}
        </button>
      ))}
    </div>
  );
}