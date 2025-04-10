import React, { useState, useEffect } from "react";
import { Card, Modal } from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../../css/taskStyle.css";
import { IMG_URL } from "../../../config";

export const TaskComponent = ({ elem, values, updateValues }) => {
  const [taskValues, setTaskValues] = useState(
    Array.isArray(values) ? values : [values]
  );
  const [showModal, setShowModal] = useState(false);
  const [imageSrc, setImageSrc] = useState("");

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

  const openModal = (imgSrc) => {
    setImageSrc(imgSrc);
    setShowModal(true);
  };

  const closeModal = () => {
    setShowModal(false);
    setImageSrc("");
  };

  return (
    <div>
      <Card
        id="taskCard"
        className="shadow-sm p-3 mb-4 color-bg2 rounded w-100 mx-auto"
      >
        <Card.Body>
          <Card.Title className="text-center text-white">
            {elem.taskId}. feladat<br></br> {elem.description}
          </Card.Title>
          <Card.Text
            className="text-center text-white"
            dangerouslySetInnerHTML={{
              __html: elem.text.replace(/^"|"$/g, "").replace(/\n/g, "<br>"),
            }}
          ></Card.Text>

          {elem.picName && (
            <div className="d-flex justify-content-center">
              <img
                id="taskImg"
                className="img-fluid rounded clickable-img"
                src={`${IMG_URL}${elem.picName}`}
                alt={elem.picName}
                onClick={() => openModal(`${IMG_URL}${elem.picName}`)}
                style={{
                  maxWidth: "50%",
                  height: "auto",
                  marginBottom: "20px",
                }}
              />
            </div>
          )}

          <div className="mt-auto color-bg2" id="taskInputs">
            {elem.type.name === "textbox" &&
              elem.isCorrect.split("|").map((_, index) => (
                <div key={index} className="d-flex justify-content-center">
                  {_.split("_").length > 1 ? (
                    <>
                      <b id="taskText" className="text-white">
                        {_.split("_")[0]}
                      </b>
                      <input
                        className="textbox-style mb-3 w-50 ml-3 color-bg3"
                        type="text"
                        value={taskValues[index] || ""}
                        onChange={(e) =>
                          handleTextboxChange(index, e.target.value)
                        }
                      />
                    </>
                  ) : (
                    <input
                      className="textbox-style mb-3 w-50 ml-3 color-bg3"
                      type="text"
                      value={taskValues[index] || ""}
                      onChange={(e) =>
                        handleTextboxChange(index, e.target.value)
                      }
                    />
                  )}
                </div>
              ))}

            {elem.type.name === "radio" && (
              <div className="d-flex justify-content-center">
                <div className="radio-container">
                  {elem.isCorrect.split(";").map((_, index) => (
                    <div key={index} className="form-check color-bg2">
                      <input
                        className="form-check-input"
                        type="radio"
                        id={`radio-${elem.id}-${index}`}
                        name={`radio-${elem.id}`}
                        checked={taskValues[index] === "1"}
                        onChange={() => handleRadioChange(index)}
                      />
                      <label
                        htmlFor={`radio-${elem.id}-${index}`}
                        className="radio-label color-bg3"
                      >
                        {elem.answers.split(";")[index]}
                      </label>
                    </div>
                  ))}
                </div>
              </div>
            )}

            {elem.type.name === "checkbox" && (
              <div className="d-flex justify-content-center">
                <div className="checkbox-container color-bg2">
                  {elem.isCorrect.split(";").map((_, index) => (
                    <div key={index} className="form-check color-bg2">
                      <input
                        className="form-check-input"
                        type="checkbox"
                        id={`checkbox-${elem.id}-${index}`}
                        checked={taskValues[index] === "1"}
                        onChange={() => handleCheckboxChange(index)}
                      />
                      <label
                        htmlFor={`checkbox-${elem.id}-${index}`}
                        className="checkbox-label color-bg3"
                      >
                        {elem.answers.split(";")[index]}
                      </label>
                    </div>
                  ))}
                </div>
              </div>
            )}
          </div>
        </Card.Body>
      </Card>

      <Modal show={showModal} onHide={closeModal} centered>
        <Modal.Header closeButton></Modal.Header>
        <Modal.Body>
          <div className="d-flex justify-content-center">
            <img
              className="img-fluid rounded w-100"
              src={imageSrc}
              alt="Enlarged view"
              style={{ maxHeight: "80vh", maxWidth: "100%" }}
            />
          </div>
        </Modal.Body>
      </Modal>
    </div>
  );
};
