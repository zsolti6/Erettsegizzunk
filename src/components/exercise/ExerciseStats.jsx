import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import { Tooltip } from "bootstrap";
import { IMG_URL } from "../../config";
import { FaImage } from "react-icons/fa";
import "../../css/taskStyle.css";

export const ExerciseStats = () => {
  const { state } = useLocation();
  const navigate = useNavigate();
  const { taskValues, exercises } = state || {};
  const sortedTaskValues = Object.values(taskValues || {}).sort((a, b) => a.taskId - b.taskId);

  const [modalImage, setModalImage] = useState(null); // State to store the image for the modal

  const getCorrectAnswers = (task) =>
    task.answers.split(";").filter((_, i) => task.isCorrect.split(";")[i] === "1").join(", ");
  const getUserAnswers = (task) =>
    task.answers.split(";").filter((_, i) => task.values[i] === "1").join(", ") || "Nem válaszoltál";

  useEffect(() => {
    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach((el) => new Tooltip(el));
  }, []);

  return (
    <div className="page-wrapper">
      <div className="container col-md-8 mt-3">
        <h2 className="mb-4 text-center text-white mt-4">Feladatok összegzése</h2>
        <div className="table-responsive">
          <table className="table color-bg2">
            <thead className="thead-dark">
              <tr>
                {["Feladat", "Leírás", "Megoldás", "Válaszaid", "Értékelés"].map((h, i) => (
                  <th key={i} className="text-center">{h}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {sortedTaskValues.map((task, i) => (
                <tr key={i}>
                  <td className="color-bg3 fs-1">{task.taskId}</td>
                  <td className="color-bg3">
                    <b>{exercises[i].description}</b>
                    <br />
                    {exercises[i].text}
                    <br/>
                    {exercises[i].picName && (
                      <FaImage
                        className="text-primary clickable-icon"
                        size={24}
                        title="Kép megtekintése"
                        onClick={() => setModalImage(`${IMG_URL}${exercises[i].picName}`)}
                      />
                    )}
                  </td>
                  <td className="color-bg3" title="asd">{getCorrectAnswers(task)}</td>
                  <td className="color-bg3">{getUserAnswers(task)}</td>
                  <td className="text-center color-bg3">
                    {getCorrectAnswers(task) === getUserAnswers(task) ? (
                      <span className="text-success">✅</span>
                    ) : (
                      <span className="text-danger">❌</span>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* Modal for Image */}
        {modalImage && (
          <div
            className="modal fade show d-block"
            tabIndex="-1"
            role="dialog"
            style={{ backgroundColor: "rgba(0, 0, 0, 0.5)" }}
          >
            <div className="modal-dialog modal-dialog-centered" role="document">
              <div className="modal-content">
                <div className="modal-header">
                  <h5 className="modal-title">Feladat kép</h5>
                  <button
                    type="button"
                    className="btn-close"
                    aria-label="Close"
                    onClick={() => setModalImage(null)}
                  ></button>
                </div>
                <div className="d-flex justify-content-center">
                  <img
                    id="statsImg"
                    className="img-fluid rounded p-3"
                    src={modalImage}
                    alt="Enlarged view"
                    style={{ maxHeight: '80vh', maxWidth: '100%' }}
                  />
                </div>
              </div>
            </div>
          </div>
        )}

        {/* Buttons at the bottom */}
        <div className="d-flex justify-content-center gap-3 mt-4">
          <button
            className="btn color-bg1 text-white"
            onClick={() => navigate("/")}
          >
            Vissza a főoldalra
          </button>
          <button
            className="btn color-bg1 text-white"
            onClick={() => navigate("/feladat-valasztas")}
          >
            Új feladatlap
          </button>
        </div>
      </div>
    </div>
  );
};