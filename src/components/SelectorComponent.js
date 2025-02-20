import { useNavigate } from "react-router-dom";
import React, { useState, useEffect } from "react";
import axios from "axios";
import "../css/Selector.css";
import Navbar from "./Navbar";

function SelectorComponent() {
  const [formData, setFormData] = useState({
    subject: "",  // Keep subjectName inside formData
    difficulty: "közép"
  });

  const [subjectId, setSubjectId] = useState(""); // Store subjectId separately
  const [subjects, setSubjects] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    axios
      .get("https://localhost:7066/erettsegizzunk/Tantargyak/get-tantargyak")
      .then((response) => {
        const formattedSubjects = response.data.map((subject) => ({
          id: subject.id,
          name: subject.name,
          value: String(subject.name), // Convert id to string
          label: subject.name
        }));

        setSubjects(formattedSubjects);

        if (formattedSubjects.length > 0) {
          setSubjectId(formattedSubjects[0].id);
          setFormData((prev) => ({
            ...prev,
            subject: formattedSubjects[0].name
          }));
        }
        console.log(formData);
      })
      .catch((error) => {
        console.error("Error fetching subjects:", error);
      });
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;

    if (name === "subject") {
      const selectedSubject = subjects.find((subject) => subject.id.toString() === value);
      if (selectedSubject) {
        setSubjectId(selectedSubject.id); // Update subjectId separately
        setFormData((prev) => ({
          ...prev,
          subject: selectedSubject.name // Keep subjectName inside formData
        }));
      }
    } else {
      setFormData((prev) => ({ ...prev, [name]: value }));
    }
  };

  const handleStartExercise = () => {
    navigate("/exercise", { state: { ...formData, subjectId } }); // Pass subjectId separately
  };

  return (
    <div className="page-wrapper">
      <Navbar />
      <div className="content-container">
        <h3>Válassz tantárgyat</h3>
        <form className="exercise-form">
          <div className="radio-inputs">
            {subjects.map(({ id, name }) => (
              <label className="radio" key={id}>
                <input
                  type="radio"
                  name="subject"
                  value={id}
                  checked={subjectId === id}
                  onChange={handleChange}
                />
                <span className="name">{name}</span>
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
