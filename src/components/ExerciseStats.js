import React, { useEffect } from "react";
import { useLocation } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import Navbar from "./Navbar";
import { Tooltip } from "bootstrap"; // Import Bootstrap Tooltip
import "../css/taskStyle.css";

const ExerciseStats = () => {
  const { state } = useLocation();
  const { taskValues, exercises } = state || {};

  const headers = ["Feladat", "Megoldás", "Válaszaid", "Értékelés"];

  const sortedTaskValues = Object.values(taskValues || {}).sort((a, b) => a.taskId - b.taskId);

  const tableData = sortedTaskValues.map((task) => {
    const exercise = exercises.find((ex) => ex.taskId === task.taskId);
    const isCorrectArray = task.isCorrect.split(';');
    const answersArray = task.answers.split(';');
    const result = answersArray.filter((_, index) => isCorrectArray[index] === '1').join(", ");
    let guess = "";

    if (task.isCorrect === "1;") {
      guess = task.values;
    } else {
      const guessArray = task.values;
      guess = answersArray.filter((_, index) => guessArray[index] === '1').join(", ");
    }

    return {
      taskId: task.taskId,
      description: exercise?.description || "N/A",
      text: exercise?.text || "N/A",
      isCorrect: result,
      values: guess === "" ? "Nem válaszoltál" : guess,
      marks: result === guess ? "✅" : "❌",
    };
  });

  useEffect(() => {
    // Initialize Bootstrap tooltips correctly
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    tooltipTriggerList.forEach((tooltipTriggerEl) => {
      new Tooltip(tooltipTriggerEl);
    });
  }, []);

  return (
    <div className="container col-md-8" style={{ height: "100vh" }}>
      <Navbar />
      <div style={{ marginTop: "8vh" }}>
        <h2 className="mb-4">Feladatok összegzése</h2>

        <div className="table-responsive">
          <table className="table table-striped table-bordered">
            <thead className="thead-dark">
              <tr>
                {headers.map((header, index) => (
                  <th key={index} scope="col" className="text-center fs-5">{header}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {tableData.map((row, index) => (
                <tr key={index}>
                  <td
                    className="fw-bold fs-5 text-center"
                    data-bs-toggle="tooltip"
                    data-bs-placement="top"
                    data-bs-title={`${row.description}\n${row.text}`} // Use data-bs-title instead of title
                  >
                    {row.taskId}
                  </td>
                  <td>{row.isCorrect}</td>
                  <td>{row.values}</td>
                  <td>{row.marks}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default ExerciseStats;
