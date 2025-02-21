import React from "react";
import "../css/SubPage.css";

export const StatisticsComponent = () => {
  return (
    <div
      style={{
        height: "92vh",
      }}
    >
      <div style={{ display: "flex" }}>
        <div style={{ marginLeft: "250px", padding: "20px", flex: 1 }} />
      </div>
      <div className="statistics"></div>
    </div>
  );
}