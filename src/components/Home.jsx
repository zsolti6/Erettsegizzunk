import React from "react";
import { useNavigate } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import "../css/SubPage.css";
import pngegg from "./pngegg.png";
import logo from "./logo.png";

export const Home = () => {
  return (
    <div id="mainDiv" className="d-flex flex-column min-vh-100">
      <div id="subDiv" className="w-100 mt-5">
        <div id="textDiv">
          <div id="titleDiv">
            <img id="logo" className="img-fluid right" src={logo} alt="Logo" />
          </div>
          <div id="descriptionDiv" className="right">
            <p className="text-center">
              Szeretnél jó eredményt elérni az érettségin?<br />Jó helyen jársz!
            </p>
          </div>
        </div>
        <img id="bg-image" src={pngegg} alt="Background" className="w-100" />
      </div>
    </div>
  );
}