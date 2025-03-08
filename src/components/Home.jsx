import React from "react";
import { useNavigate } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import "../css/SubPage.css";

export const Home = () => {
  return (
    <div id="mainDiv" className="d-flex flex-column min-vh-100">
      <div className="container-fluid d-flex flex-column align-items-center justify-content-center text-center flex-grow-1 py-5">
        <div id="contentDiv" className="d-flex flex-column align-items-center">
        <h1 className="fw-bold display-4">Érettségizzünk!</h1>
        <p className="mt-3 px-3 lead" style={{ maxWidth: "600px" }}>
          Egy applikáció, ami segítséget nyújt érettségi előtt álló diákoknak a felkészüléshez.
        </p>
        <div className="row w-100 g-3 gx-5 justify-content-center subject-row">
          <div className="col-lg-4 col-md-4 col-sm-8 col-10">
            <SubjectComponent text={"Statisztika"} icon={<i className="bi bi-bar-chart-line"></i>} linkto={"/statisztika"} />
          </div>
          <div className="col-lg-4 col-md-4 col-sm-8 col-10">
            <SubjectComponent text={"Új feladatlap"} icon={<i className="bi bi-pencil-square"></i>} linkto={"/feladat-valasztas"} />
          </div>
          <div className="col-lg-4 col-md-4 col-sm-8 col-10">
            <SubjectComponent text={"Útmutató"} icon={<i className="bi bi-book"></i>} linkto={"/utmutato"} />
          </div>
        </div>
      </div>
      </div>
    </div>
  );
}

function SubjectComponent({ text, linkto, icon }) {
  const navigate = useNavigate();
  return (
    <div
      id={linkto}
      onClick={() => navigate(linkto)}
      className="action p-4 d-flex align-items-center justify-content-center cursor-pointer shadow-sm text-center w-100 color-bg2 color-text1"
    >
      <h2>
      {text}<br></br>
      {icon}
      </h2>
    </div>
  );
}
