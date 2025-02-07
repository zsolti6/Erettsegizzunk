import React from "react";
import { useNavigate } from "react-router-dom";
import "../css/SubPage.css";
import Navbar from "./Navbar";

function Home() {
  return (
    <div className="d-flex flex-column min-vh-100">
      <Navbar />
      <div className="App container d-flex flex-column align-items-center justify-content-center text-center flex-grow-1 py-5">
        <h1>Érettségizzünk!</h1>
        <p className="mt-3 px-3" style={{ maxWidth: "600px" }}>
          Egy applikáció, ami segítséget nyújt érettségi előtt álló diákoknak a
          felkészüléshez.
        </p>
        <div className="row w-100 g-4 justify-content-center">
          <div className="col-md-4 col-sm-12">
            <SubjectComponent text={"Statisztika"} linkto={"/statistics"} />
          </div>
          <div className="col-md-4 col-sm-12">
            <SubjectComponent text={"Új feladatlap"} linkto={"/selector"} />
          </div>
          <div className="col-md-4 col-sm-12">
            <SubjectComponent text={"Útmutató"} linkto={"/tutorial"} />
          </div>
        </div>
      </div>
    </div>
  );
}

export default Home;

function SubjectComponent({ text, linkto }) {
  const navigate = useNavigate();
  return (
    <div
      id={linkto}
      onClick={() => navigate(linkto)}
      className="action bg-light p-4 d-flex align-items-center justify-content-center border rounded cursor-pointer shadow-sm"
      style={{ minHeight: "120px", fontSize: "1.2rem", fontWeight: "bold" }}
    >
      {text}
    </div>
  );
}
