import React from 'react';
import '../css/Sidenav.css';

function Sidenav({ tasks, setActiveComponent }) {
  // Ensure each task has a taskId property
  const tasksWithIds = tasks.map((task, index) => ({
    ...task,
    taskId: index + 1,
  }));

  return (
    <div className="sidenav">
      {tasksWithIds.map((task) => (
        <button
          key={task.id || task.taskId}
          onClick={() => setActiveComponent(task)} // Update the active component
        >
          Feladat {task.taskId}
        </button>
      ))}
    </div>
  );
}

export default Sidenav;
