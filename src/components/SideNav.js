import React from 'react';
import '../css/Sidenav.css';

function Sidenav({ tasks, setActiveComponent }) {
  return (
    <div className="sidenav">
      {tasks.map((task) => (
        <button
          key={task.id}
          onClick={() => {
            console.log("Sidenav button clicked:", task.id);
            setActiveComponent(task.id);
          }}
        >
          Feladat {task.taskId}
        </button>
      ))}
    </div>
  );
}

export default Sidenav;
