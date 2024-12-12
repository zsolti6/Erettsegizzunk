import { useNavigate } from "react-router-dom";
import React, { useState } from "react";
import "../css/Selector.css";
import Navbar from "./Navbar";
import { Link } from "react-router-dom";

function SelectorComponent() {
  const [selectedSubject, setSelectedSubject] = useState("matematika");
  const [selectedDifficulty, setSelectedDifficulty] = useState("közép");

  const navigate = useNavigate();

  const handleSubjectChange = (e) => {
    setSelectedSubject(e.target.value);
  };

  const handleDifficultyChange = (e) => {
    setSelectedDifficulty(e.target.value);
  };

  const handleStartExercise = () => {
    navigate("/exercise", {
      state: { tantargy: selectedSubject, szint: selectedDifficulty },
    });
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
                value="történelem"
                checked={selectedSubject === "történelem"}
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
                value="közép"
                name="difficulty"
                type="radio"
                checked={selectedDifficulty === "közép"}
                onChange={handleDifficultyChange}
              />
              <span>Közép szint</span>
            </label>
            <label>
              <input
                value="emelt"
                name="difficulty"
                type="radio"
                checked={selectedDifficulty === "emelt"}
                onChange={handleDifficultyChange}
              />
              <span>Emelt szint</span>
            </label>
            <span className="selection"></span>
          </div>
          <br />
          <button type="button" onClick={handleStartExercise}>
            Feladatlap megkezdése
          </button>
        </form>
      </div>
    </div>
  );
}

export default SelectorComponent;
