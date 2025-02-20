import React, { useEffect } from "react";
import { useLocation } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import Navbar from "./Navbar";
import { Tooltip } from "bootstrap";
import "../css/taskStyle.css";
import axios from "axios";

const ExerciseStats = () => {
  const { state } = useLocation();
  const { taskValues, exercises, subjectId } = state || {};

  const sortedTaskValues = Object.values(taskValues || {}).sort((a, b) => a.taskId - b.taskId);
  
  useEffect(() => {
    const user = getUser();
    if (!user) return;

    const taskCorrects = getTaskCorrectness();

    //console.log("Task Corrects:", taskCorrects);
    sendStatistics(user, taskCorrects);
  }, []);

  const getUser = () => {
    const rememberMe = localStorage.getItem("rememberMe") === "true";
    return rememberMe
      ? JSON.parse(localStorage.getItem("user"))
      : JSON.parse(sessionStorage.getItem("user"));
  };

  const getTaskCorrectness = () => {
    const correctness = {};
    sortedTaskValues.forEach((task) => {
      const exercise = exercises.find((ex) => ex.taskId === task.taskId);
      const result = getCorrectAnswers(task);
      const guess = getUserAnswers(task);
      correctness[exercise.id] = result === guess;
    });
    return correctness;
  };

  const getCorrectAnswers = (task) => {
    const isCorrectArray = task.isCorrect.split(";");
    const answersArray = task.answers.split(";");
    return answersArray.filter((_, index) => isCorrectArray[index] === "1").join(", ");
  };

  const getUserAnswers = (task) => {
    if (task.isCorrect === "1;") return task.values;
    const guessArray = task.values;
    const answersArray = task.answers.split(";");
    return answersArray.filter((_, index) => guessArray[index] === "1").join(", ") || "Nem válaszoltál";
  };

  const sendStatistics = (user, taskCorrects) => {
    axios.put("http://localhost:5000/erettsegizzunk/UserStatistics/put-statisztika", {
      userId: user.id,
      token: user.token,
      subjectId: subjectId,
      taskIds: taskCorrects,
    })
    .then(response => console.log("Stats updated:", response))
    .catch(error => console.error("Error updating stats:", error));
  };

  useEffect(() => {
    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach((el) => new Tooltip(el));
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
                {["Feladat", "Megoldás", "Válaszaid", "Értékelés"].map((header, index) => (
                  <th key={index} scope="col" className="text-center fs-5">{header}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {sortedTaskValues.map((task, index) => {
                const exercise = exercises.find((ex) => ex.taskId === task.taskId);
                return (
                  <tr key={index}>
                    <td
                      className="fw-bold fs-5 text-center"
                      data-bs-toggle="tooltip"
                      data-bs-placement="top"
                      data-bs-title={`${exercise?.description || "N/A"}\n${exercise?.text || "N/A"}`}
                    >
                      {task.taskId}
                    </td>
                    <td>{getCorrectAnswers(task)}</td>
                    <td>{getUserAnswers(task)}</td>
                    <td>{getCorrectAnswers(task) === getUserAnswers(task) ? "✅" : "❌"}</td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default ExerciseStats;
