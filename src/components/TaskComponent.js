import React, { useState, useEffect } from 'react';

function TaskComponent({ elem, onNavigatePrevious, onNavigateNext, activeIndex, totalTasks }) {
  const [checkedState, setCheckedState] = useState({});

  useEffect(() => {
    setCheckedState((prevState) => ({
      ...prevState,
      [elem.id]: prevState[elem.id] || {
        radio: "",
        checkbox: [],
        textboxes: Array(elem.helyese.split(";").length).fill(""),
      },
    }));
  }, [elem]);

  const handleTextboxChange = (taskId, index, value) => {
    setCheckedState((prevState) => ({
      ...prevState,
      [taskId]: {
        ...prevState[taskId],
        textboxes: prevState[taskId].textboxes.map((textbox, i) =>
          i === index ? value : textbox
        ),
      },
    }));
  };

  const handleRadioChange = (taskId, value) => {
    setCheckedState((prevState) => ({
      ...prevState,
      [taskId]: {
        ...prevState[taskId],
        radio: value,
      },
    }));
  };

  const handleCheckboxChange = (taskId, value) => {
    setCheckedState((prevState) => {
      const updatedCheckboxes = prevState[taskId].checkbox.includes(value)
        ? prevState[taskId].checkbox.filter((item) => item !== value)
        : [...prevState[taskId].checkbox, value];

      return {
        ...prevState,
        [taskId]: {
          ...prevState[taskId],
          checkbox: updatedCheckboxes,
        },
      };
    });
  };

  const renderNavigationButtons = () => (
    <div>
      {activeIndex > 0 && <button onClick={onNavigatePrevious}>Előző</button>}
      {activeIndex < totalTasks - 1 ? (
        <button onClick={onNavigateNext}>Következő</button>
      ) : (
        <button onClick={() => { /* Placeholder for future action */ }}>Feladatlap befejezése</button>
      )}
    </div>
  );

  if (elem.tipus.nev === "textbox") {
    return (
      <div>
        <h2>{elem.taskId}. feladat</h2>
        <h3>{elem.leiras}</h3>
        {elem.helyese.split(";").map((helyes, index) => (
          helyes === "1" && (
            <div key={index}>
              <input
                className="tbStyle"
                id={`textbox-${elem.id}-${index}`}
                type="text"
                value={checkedState[elem.id]?.textboxes[index] || ""}
                onChange={(e) => handleTextboxChange(elem.id, index, e.target.value)}
              />
            </div>
          )
        ))}
        {renderNavigationButtons()}
      </div>
    );
  }

  if (elem.tipus.nev === "radio") {
    return (
      <div>
        <h2>{elem.taskId}. feladat</h2>
        <h3>{elem.leiras}</h3>
        {elem.helyese.split(";").map((helyes, index) => (
          <div key={index}>
            <input
              className="rStyle"
              name={`radio-${elem.id}`}
              type="radio"
              id={`radio-${elem.id}-${index}`}
              value={index}
              checked={checkedState[elem.id]?.radio === `${index}`}
              onChange={() => handleRadioChange(elem.id, `${index}`)}
            />
            <label htmlFor={`radio-${elem.id}-${index}`}>
              {elem.megoldasok.split(";")[index]}
            </label>
          </div>
        ))}
        {renderNavigationButtons()}
      </div>
    );
  }

  if (elem.tipus.nev === "checkbox") {
    return (
      <div>
        <h2>{elem.taskId}. feladat</h2>
        <h3>{elem.leiras}</h3>
        {elem.helyese.split(";").map((helyes, index) => (
          <div key={index}>
            <input
              className="cbStyle"
              name={`checkbox-${elem.id}`}
              type="checkbox"
              id={`checkbox-${elem.id}-${index}`}
              value={index}
              checked={checkedState[elem.id]?.checkbox.includes(`${index}`)}
              onChange={() => handleCheckboxChange(elem.id, `${index}`)}
            />
            <label htmlFor={`checkbox-${elem.id}-${index}`}>
              {elem.megoldasok.split(";")[index]}
            </label>
          </div>
        ))}
        {renderNavigationButtons()}
      </div>
    );
  }

  return null;
}

export default TaskComponent;
