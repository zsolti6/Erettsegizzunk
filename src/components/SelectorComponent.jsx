import { useNavigate } from "react-router-dom";
import React, { useState, useEffect } from "react";
import axios from "axios";
import { Modal, Button } from "react-bootstrap";
import "../css/Selector.css";
import { BASE_URL } from '../config';

export const SelectorComponent = () => {
  const [formData, setFormData] = useState({
    subject: "",
    difficulty: "közép",
  });

  const [subjectId, setSubjectId] = useState("");
  const [subjects, setSubjects] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    axios.get(`${BASE_URL}/erettsegizzunk/Tantargyak/get-tantargyak`)
      .then((response) => {
        const formattedSubjects = response.data.map((subject) => ({
          id: subject.id,
          name: subject.name,
          value: String(subject.name),
          label: subject.name,
        }));

        setSubjects(formattedSubjects);

        if (formattedSubjects.length > 0) {
          setSubjectId(formattedSubjects[0].id);
          setFormData((prev) => ({
            ...prev,
            subject: formattedSubjects[0].name,
          }));
        }
      })
      .catch((error) => {
        console.error("Error fetching subjects:", error);
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  useEffect(() => {
    const savedExercises = localStorage.getItem("exercises");
    const savedTaskValues = localStorage.getItem("taskValues");

    if (savedExercises && savedTaskValues) {
      setShowModal(true);
    }
  }, []);

  const handleContinue = () => {
    const savedExercises = JSON.parse(localStorage.getItem("exercises"));
    const savedTaskValues = JSON.parse(localStorage.getItem("taskValues"));
    navigate("/gyakorlas", {
      state: {
        subject: formData.subject,
        difficulty: formData.difficulty,
        subjectId,
        savedExercises,
        savedTaskValues,
      },
    });
  };

  const handleNewAttempt = () => {
    setShowModal(false);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;

    if (name === "subject") {
      const selectedSubject = subjects.find((subject) => subject.id.toString() === value);
      if (selectedSubject) {
        setSubjectId(selectedSubject.id);
        setFormData((prev) => ({
          ...prev,
          subject: selectedSubject.name,
        }));
      }
    } else {
      setFormData((prev) => ({ ...prev, [name]: value }));
    }
  };

  const handleStartExercise = () => {
    navigate("/gyakorlas", { state: { ...formData, subjectId } });
  };

  return (
    <div className="page-wrapper">
      <div className="content-container">
        {loading ? (
          <div className="spinner-container">
            <div className="spinner"></div>
          </div>
        ) : (
          <>
            <h4>Válassz tantárgyat</h4>
            <form className="exercise-form">
              <div className="radio-group">
                {subjects.map(({ id, name }) => (
                  <label className="radio-option" key={id}>
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

              <div className="radio-group">
                <label className="radio-option">
                  <input
                    type="radio"
                    name="difficulty"
                    value="közép"
                    checked={formData.difficulty === "közép"}
                    onChange={handleChange}
                  />
                  <span className="name">Közép szint</span>
                </label>
                <label className="radio-option">
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

              <button type="button" className="select-button" onClick={handleStartExercise}>
                Feladatlap megkezdése
              </button>
            </form>
          </>
        )}
      </div>

      <Modal show={showModal} onHide={handleNewAttempt} centered>
        <Modal.Header closeButton>
          <Modal.Title>Folytatni szeretnéd?</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p>Van egy korábbi próbálkozásod. Szeretnéd folytatni?</p>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleNewAttempt}>
            Új próbálkozás
          </Button>
          <Button variant="primary" onClick={handleContinue}>
            Folytatás
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
};