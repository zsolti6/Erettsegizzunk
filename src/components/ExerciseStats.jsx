import React, { useEffect } from "react";
import { useLocation } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import { Tooltip } from "bootstrap";
import "../css/taskStyle.css";

export const ExerciseStats = () => {
  const { state } = useLocation();
  const { taskValues, exercises } = state || {};
  const sortedTaskValues = Object.values(taskValues || {}).sort((a, b) => a.taskId - b.taskId);

  const getCorrectAnswers = (task) => task.answers.split(";").filter((_, i) => task.isCorrect.split(";")[i] === "1").join(", ");
  const getUserAnswers = (task) => task.answers.split(";").filter((_, i) => task.values[i] === "1").join(", ") || "Nem válaszoltál";

  useEffect(() => {
    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach((el) => new Tooltip(el));
  }, []);

  return (
    <div className="container col-md-8" style={{ height: "100vh", marginTop: "8vh" }}>
      <h2 className="mb-4">Feladatok összegzése</h2>
      <div className="table-responsive">
        <table className="table table-striped table-bordered">
          <thead className="thead-dark">
            <tr>{["Feladat", "Megoldás", "Válaszaid", "Értékelés"].map((h, i) => <th key={i} className="text-center">{h}</th>)}</tr>
          </thead>
          <tbody>
            {sortedTaskValues.map((task, i) => (
              <tr key={i}>
                <td>{task.taskId}</td>
                <td>{getCorrectAnswers(task)}</td>
                <td>{getUserAnswers(task)}</td>
                <td>{getCorrectAnswers(task) === getUserAnswers(task) ? "✅" : "❌"}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};
