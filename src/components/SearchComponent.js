import React from "react";
import "../css/SubPage.css";
import Navbar from "./Navbar";

function SearchComponent() {
  return (
    <div
      style={{
        height: "92vh",
      }}
    >
      <div style={{ display: "flex" }}>
        <div style={{ marginLeft: "250px", padding: "20px", flex: 1 }} />
      </div>
      <div className="search"></div>
    </div>
  );
}

export default SearchComponent;
