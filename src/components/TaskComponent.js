import React, { useState, useEffect } from "react";
import "../css/taskStyle.css";

function TaskComponent({ elem, values, updateValues }) {
  const [taskValues, setTaskValues] = useState(
    Array.isArray(values) ? values : [values] // Ensure it's always an array
  );

  useEffect(() => {
    setTaskValues(Array.isArray(values) ? values : [values]); // Ensure consistency
  }, [elem.id, values]);

  const handleTextboxChange = (index, value) => {
    const newValues = [...taskValues];
    newValues[index] = value;
    setTaskValues(newValues);
    updateValues(newValues);
  };

  const handleRadioChange = (index) => {
    if (!Array.isArray(taskValues)) return; // Prevent errors

    const newValues = taskValues.map((_, i) => (i === index ? "1" : "0"));
    setTaskValues(newValues);
    updateValues(newValues);
  };

  const handleCheckboxChange = (index) => {
    const newValues = [...taskValues];
    newValues[index] = newValues[index] === "1" ? "0" : "1";
    setTaskValues(newValues);
    updateValues(newValues);
  };

  if (elem.type.name === "textbox") {
    return (
      <div className="task">
        <h3>{elem.taskId}. feladat</h3>
        <h5><b>{elem.description}</b></h5>
        <p>{elem.text}</p>
        {elem.picName != null && <img className="taskPic" src={"http://images.erettsegizzunk.nhely.hu/" + elem.picName} alt={elem.picName} title={elem.picName} />}        
        {elem.isCorrect.split(";").map(
          (helyes, index) =>
            helyes === "1" && (
              <div key={index}>
                <input
                  className="tbStyle"
                  id={`textbox-${elem.id}-${index}`}
                  type="text"
                  value={taskValues[index] || ""}
                  onChange={(e) => handleTextboxChange(index, e.target.value)}
                />
              </div>
            )
        )}
      </div>
    );
  }

  if (elem.type.name === "radio") {
    return (
      <div className="task">
        <h3>{elem.taskId}. feladat</h3>
        <h3><b>{elem.description}</b></h3>
        <p>{elem.text}</p>
        {elem.picName != null && <img className="taskPic" src={"http://images.erettsegizzunk.nhely.hu/" + elem.picName} alt={elem.picName} title={elem.picName} />}
        <div className={elem.picName != null ? "inputGroup2" : "inputGroup1" }>
        {elem.isCorrect.split(";").map((helyes, index) => (
          <div key={index}>
            <input
              className="rStyle form-check-input"
              name={`radio-${elem.id}`}
              type="radio"
              id={`radio-${elem.id}-${index}`}
              value={index}
              checked={taskValues[index] === "1"}
              onChange={() => handleRadioChange(index)}
            />
            <label htmlFor={`radio-${elem.id}-${index}`}>
              {elem.answers.split(";")[index]}
            </label>
          </div>
        ))}
        </div>
      </div>
    );
  }

  if (elem.type.name === "checkbox") {
    return (
      <div className="task">
        <h3>{elem.taskId}. feladat</h3>
        <h5><b>{elem.description}</b></h5>
        <p>{elem.text}</p>
        {elem.picName != null && <img className="taskPic" src={"http://images.erettsegizzunk.nhely.hu/" + elem.picName} alt={elem.picName} title={elem.picName} />}
        {elem.isCorrect.split(";").map((helyes, index) => (
          <div key={index}>
            <input
              className="cbStyle form-check-input"
              name={`checkbox-${elem.id}`}
              type="checkbox"
              id={`checkbox-${elem.id}-${index}`}
              value={index}
              checked={taskValues[index] === "1"}
              onChange={() => handleCheckboxChange(index)}
            />
            <label htmlFor={`checkbox-${elem.id}-${index}`}>
              {elem.answers.split(";")[index]}
            </label>
          </div>
        ))}
      </div>
    );
  }

  return null;
}

export default TaskComponent;
