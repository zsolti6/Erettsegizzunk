import React from 'react';
import '../css/Sidenav.css';

function Sidenav({ tasks, setActiveComponent }) {
  return (
    <div className="sidenav">
      {tasks.map((task, index) => (
        <button
          key={task.id || index}
          onClick={() => setActiveComponent(task)} // Update the active component
        >
          Feladat {index + 1}
        </button>
      ))}
    </div>
  );
}

export default Sidenav;
