import React from "react";
import { useLocation } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import Navbar from "./Navbar";

const ExerciseStats = () => {
  const { state } = useLocation();
  const { taskValues } = state || {};

  // Flatten the taskValues and prepare data to display
  const headers = ["Feladat", "Megoldás", "Válaszaid", "Értékelés"];

  // Sort taskValues based on taskId in ascending order
  const sortedTaskValues = Object.values(taskValues || {}).sort((a, b) => a.taskId - b.taskId);

  // Create table rows
  const tableData = sortedTaskValues.map((task) => {
    const helyeseArray = task.helyese.split(';');
    const megoldasokArray = task.megoldasok.split(';');
    const result = megoldasokArray.filter((_, index) => helyeseArray[index] === '1').join(", ");
    let guess = "";
    
    if (task.helyese === "1;") {
      guess = task.values;
    } else {
      const guessArray = task.values;
      guess = megoldasokArray.filter((_, index) => guessArray[index] === '1').join(", ");
    }

    return {
      taskId: task.taskId,
      helyese: result,
      values: guess, // Assuming 'values' is an array
      marks: result == guess ? "✅" : "❌",
    };
  });

  return (
    <div className="container col-md-8" style={{ height: "100vh" }}>
      <Navbar />
      <div style={{ marginTop: "8vh" }}>
        <h2 className="mb-4">Feladatok összegzése</h2>

        <div className="table-responsive">
          <table className="table table-striped table-bordered">
            {/* Table Header */}
            <thead className="thead-dark">
              <tr>
                {headers.map((header, index) => (
                  <th key={index} scope="col">{header}</th>
                ))}
              </tr>
            </thead>

            {/* Table Body */}
            <tbody>
              {tableData.map((row, index) => (
                <tr key={index}>
                  <td>{row.taskId}</td>
                  <td>{row.helyese}</td>
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
