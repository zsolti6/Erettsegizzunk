import React from "react";
import "../css/Sidenav.css";

export const Sidenav = ({ tasks, setActiveComponent, activeIndex, isOpen, setIsOpen }) => {
  return (
    <div className={`sidenav ${isOpen ? "open" : ""}`}>
      

      {/* Task Buttons */}
      {tasks.map((task, index) => (
        <button
          key={task.id}
          className={`task-button btn btn-outline-primary w-100 text-start mb-2 ${
            activeIndex === index ? "active" : ""
          }`}
          onClick={() => setActiveComponent(task.taskId - 1)}
        >
          Feladat {task.taskId}
        </button>
      ))}
    </div>
  );
};
