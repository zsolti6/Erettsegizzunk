import React, { useState } from "react";
import "../css/Selector.css";
import Navbar from "./Navbar";

function SelectorComponent() {
  // State for the subject selection
  const [selectedSubject, setSelectedSubject] = useState("matematika");

  // State for the difficulty level selection
  const [selectedDifficulty, setSelectedDifficulty] = useState("value-1");

  const handleSubjectChange = (e) => {
    setSelectedSubject(e.target.value);
  };

  const handleDifficultyChange = (e) => {
    setSelectedDifficulty(e.target.value);
  };

  return (
    <div className="exercise" style={{ height: "72vh" }}>
      <Navbar />
      <div>
        <h3>Válassz tantárgyat</h3>
        <form className="exercise-form">
          <div className="radio-inputs">
            <label className="radio">
              <input
                type="radio"
                name="subject"
                value="matematika"
                checked={selectedSubject === "matematika"}
                onChange={handleSubjectChange}
              />
              <span className="name">Matematika</span>
            </label>
            <label className="radio">
              <input
                type="radio"
                name="subject"
                value="tortenelem"
                checked={selectedSubject === "tortenelem"}
                onChange={handleSubjectChange}
              />
              <span className="name">Történelem</span>
            </label>
            <label className="radio">
              <input
                type="radio"
                name="subject"
                value="magyar"
                checked={selectedSubject === "magyar"}
                onChange={handleSubjectChange}
              />
              <span className="name">Magyar nyelv</span>
            </label>
          </div>

          <p>Középszintű vagy emelt szintű érettségi feladatokat szeretnél gyakorolni?</p>
          <div className="radio-input">
            <label>
              <input
                value="value-1"
                name="difficulty"
                type="radio"
                checked={selectedDifficulty === "value-1"}
                onChange={handleDifficultyChange}
              />
              <span>Közép szint</span>
            </label>
            <label>
              <input
                value="value-2"
                name="difficulty"
                type="radio"
                checked={selectedDifficulty === "value-2"}
                onChange={handleDifficultyChange}
              />
              <span>Emelt szint</span>
            </label>
            <span className="selection"></span>
          </div>
          <br />
          <button type="submit">Feladatlap megkezdése</button>
        </form>
      </div>
    </div>
  );
}

export default SelectorComponent;
