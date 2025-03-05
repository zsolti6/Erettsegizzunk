import React, { useState, useEffect } from "react";
import { Container, Row, Col, Button, Card } from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "../css/taskStyle.css";

export const TaskComponent = ({ elem, values, updateValues }) => {
  const [taskValues, setTaskValues] = useState(Array.isArray(values) ? values : [values]);

  useEffect(() => {
    setTaskValues(Array.isArray(values) ? values : [values]);
  }, [elem.id, values]);

  const handleTextboxChange = (index, value) => {
    const newValues = [...taskValues];
    newValues[index] = value;
    setTaskValues(newValues);
    updateValues(newValues);
  };

  const handleRadioChange = (index) => {
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

  return (
    <Card className="shadow-sm p-3 mb-4 bg-white rounded">
      <Card.Body>
        <Card.Title>{elem.taskId}. {elem.description}</Card.Title>
        <Card.Text>{elem.text}</Card.Text>
        {elem.picName && <img className="img-fluid rounded" src={`https://res.cloudinary.com/drpkpopsq/image/upload/v1741078235/${elem.picName}`} alt={elem.picName} />}
        <div className="mt-3">
          {elem.type.name === "textbox" && elem.isCorrect.split(";").map((_, index) => (
            <input key={index} className="form-control mb-2" type="text" value={taskValues[index] || ""} onChange={(e) => handleTextboxChange(index, e.target.value)} />
          ))}
          {elem.type.name === "radio" && elem.isCorrect.split(";").map((_, index) => (
            <div key={index} className="form-check">
              <input className="form-check-input" type="radio" name={`radio-${elem.id}`} checked={taskValues[index] === "1"} onChange={() => handleRadioChange(index)} />
              <label className="form-check-label">{elem.answers.split(";")[index]}</label>
            </div>
          ))}
          {elem.type.name === "checkbox" && elem.isCorrect.split(";").map((_, index) => (
            <div key={index} className="form-check">
              <input className="form-check-input" type="checkbox" checked={taskValues[index] === "1"} onChange={() => handleCheckboxChange(index)} />
              <label className="form-check-label">{elem.answers.split(";")[index]}</label>
            </div>
          ))}
        </div>
      </Card.Body>
    </Card>
  );
};

export const ExerciseComponent = ({ exercises, activeIndex, setActiveIndex, taskValues, updateTaskValues }) => {
  return (
    <Container className="mt-4">
      <Row className="justify-content-center">
        <Col md={8}>
          {exercises.length > 0 && (
            <TaskComponent
              elem={exercises[activeIndex]}
              values={taskValues[exercises[activeIndex].id]?.values || []}
              updateValues={(newValues) => updateTaskValues(exercises[activeIndex].id, newValues)}
            />
          )}
          <div className="d-flex justify-content-between mt-3">
            {activeIndex > 0 && <Button variant="primary" onClick={() => setActiveIndex(activeIndex - 1)}>Previous</Button>}
            {activeIndex < exercises.length - 1 && <Button variant="primary" onClick={() => setActiveIndex(activeIndex + 1)}>Next</Button>}
          </div>
        </Col>
      </Row>
    </Container>
  );
};
