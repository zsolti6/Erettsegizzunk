import React, { useState, useEffect } from "react";

function TaskComponent({ elem }) {
  console.log(elem.megoldasok);

  // Initialize state with default structure when component is mounted
  const [checkedState, setCheckedState] = useState({});

  // Use useEffect to ensure state is updated when elem.id changes
  useEffect(() => {
    // Initialize checked state for this particular elem.id if it doesn't exist yet
    setCheckedState((prevState) => ({
      ...prevState,
      [elem.id]: prevState[elem.id] || {
        radio: "", // Default radio state
        checkbox: [], // Default checkbox state
        textboxes: Array(elem.helyese.split(";").length).fill("") // Initialize textboxes as empty strings
      }
    }));
  }, [elem.id, elem.helyese.length]); // Only run when elem.id or elem.helyese length changes

  const handleRadioChange = (groupId, value) => {
    setCheckedState((prevState) => ({
      ...prevState,
      [groupId]: {
        ...prevState[groupId],
        radio: value // Update the selected radio button
      }
    }));
  };

  const handleCheckboxChange = (groupId, value) => {
    setCheckedState((prevState) => {
      const updatedCheckboxes = prevState[groupId].checkbox.includes(value)
        ? prevState[groupId].checkbox.filter((item) => item !== value)
        : [...prevState[groupId].checkbox, value];

      return {
        ...prevState,
        [groupId]: {
          ...prevState[groupId],
          checkbox: updatedCheckboxes // Update the selected checkboxes
        }
      };
    });
  };

  const handleTextboxChange = (groupId, index, value) => {
    setCheckedState((prevState) => ({
      ...prevState,
      [groupId]: {
        ...prevState[groupId],
        textboxes: prevState[groupId].textboxes.map((textbox, i) =>
          i === index ? value : textbox // Update the textbox value at the correct index
        )
      }
    }));
  };

  if (elem.tipus.nev === "textbox") {
    return (
      <div>
        <h3>{elem.leiras}</h3>
        {elem.helyese.split(";").map((helyes, index) => {
          if (helyes === "1") {
            return (
              <div key={index}>
                <input
                  id={`textbox-${elem.id}-${index}`}
                  type="text"
                  value={checkedState[elem.id]?.textboxes[index] || ""} // Bind value to state
                  onChange={(e) =>
                    handleTextboxChange(elem.id, index, e.target.value) // Update state on change
                  }
                />
              </div>
            );
          }
          return null; // Only render elements marked as correct (helyes === "1")
        })}
      </div>
    );
  }

  if (elem.tipus.nev === "radio") {
    return (
      <div>
        <h3>{elem.leiras}</h3>
        {elem.helyese.split(";").map((helyes, index) => {
          const isChecked = checkedState[elem.id]?.radio === `${index}`;
          return (
            <div key={index}>
              <input
                name={`radio-${elem.id}`}
                type="radio"
                id={`radio-${elem.id}-${index}`}
                value={index}
                checked={isChecked}
                onChange={() => handleRadioChange(elem.id, `${index}`)}
              />
              <label htmlFor={`radio-${elem.id}-${index}`}>
                {elem.megoldasok.split(";")[index]}
              </label>
            </div>
          );
        })}
      </div>
    );
  }

  if (elem.tipus.nev === "checkbox") {
    return (
      <div>
        <h3>{elem.leiras}</h3>
        {elem.helyese.split(";").map((helyes, index) => {
          const isChecked = checkedState[elem.id]?.checkbox.includes(`${index}`);
          return (
            <div key={index}>
              <input
                name={`checkbox-${elem.id}`}
                type="checkbox"
                id={`checkbox-${elem.id}-${index}`}
                value={index}
                checked={isChecked}
                onChange={() => handleCheckboxChange(elem.id, `${index}`)}
              />
              <label htmlFor={`checkbox-${elem.id}-${index}`}>
                {elem.megoldasok.split(";")[index]}
              </label>
            </div>
          );
        })}
      </div>
    );
  }

  return null; // If none of the types match
}

export default TaskComponent;
