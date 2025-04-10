import React from "react";

export const ExerciseControls = ({
  activeIndex,
  exercises,
  setActiveIndex,
  sendStatistics,
}) => {
  return (
    <div className="d-flex justify-content-center align-items-center gap-2 flex-wrap mt-0">
      {activeIndex > 0 && (
        <button
          className="btn color-bg1 text-white"
          onClick={() => setActiveIndex(activeIndex - 1)}
        >
          Előző feladat
        </button>
      )}
      {activeIndex < exercises.length - 1 ? (
        <button
          className="btn color-bg1 text-white"
          onClick={() => setActiveIndex(activeIndex + 1)}
        >
          Következő feladat
        </button>
      ) : (
        <button className="btn color-bg1 text-white" onClick={sendStatistics}>
          Feladatok leadása
        </button>
      )}
    </div>
  );
};
