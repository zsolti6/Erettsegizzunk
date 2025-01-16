import React from 'react';
import TaskComponent from './TaskComponent';

function ExerciseWindow({ activeTask, onPrevious, onNext, activeIndex, totalTasks }) {
  console.log("Rendering activeTask in ExerciseWindow:", activeTask);

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
          />
        </div>
      ) : (
        <p>Válassz feladatokat.</p>
      )}
    </div>
  );
}

export default ExerciseWindow;
