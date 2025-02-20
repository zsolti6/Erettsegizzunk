import { useNavigate } from "react-router-dom";
import React, { useState, useEffect } from "react";
import axios from "axios";
import "../css/Selector.css";
import Navbar from "./Navbar";

function SelectorComponent() {
  const [formData, setFormData] = useState({
    subject: "",
    difficulty: "közép",
    subjectId: 0
  });
  const [subjects, setSubjects] = useState([]);
  const navigate = useNavigate();
  const dict = {
    "magyar": 3,
    "matek"
  }
  useEffect(() => {
    axios.get("https://localhost:7066/erettsegizzunk/Tantargyak/get-tantargyak")
      .then((response) => {
        const formattedSubjects = response.data.map((subject) => ({
          value: String(subject.name), // Convert id to string
          label: subject.name
        }));
        console.log(formattedSubjects);
        setSubjects(formattedSubjects);
        setFormData((prev) => ({
          ...prev,
          subject: formattedSubjects[0]?.value || "",
        }));
      })
      .catch((error) => {
        console.error("Error fetching subjects:", error);
      });
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    const key = Object.keys(formData).find(key => obj[key] === valueToFind);
  };

  const handleStartExercise = () => {
    navigate("/exercise", { state: formData, subjectId: formData.subjectId });
  };

  return (
    <div className="page-wrapper">
      <Navbar />
      <div className="content-container">
        <h3>Válassz tantárgyat</h3>
        <form className="exercise-form">
          <div className="radio-inputs">
            {subjects.map(({ value, label }) => (
              <label className="radio" key={value}>
                <input
                  type="radio"
                  name="subject"
                  value={value}
                  checked={formData.subject === value}
                  onChange={handleChange}
                />
                <span className="name">{label}</span>
              </label>
            ))}
          </div>

          <p>Középszintű vagy emelt szintű érettségi feladatokat szeretnél gyakorolni?</p>

          <div className="radio-inputs1">
            <label className="radio">
              <input
                type="radio"
                name="difficulty"
                value="közép"
                checked={formData.difficulty === "közép"}
                onChange={handleChange}
              />
              <span className="name">Közép szint</span>
            </label>
            <label className="radio">
              <input
                type="radio"
                name="difficulty"
                value="emelt"
                checked={formData.difficulty === "emelt"}
                onChange={handleChange}
              />
              <span className="name">Emelt szint</span>
            </label>
          </div>

          <button type="button" onClick={handleStartExercise}>
            Feladatlap megkezdése
          </button>
        </form>
      </div>
    </div>
  );
}

export default SelectorComponent;
