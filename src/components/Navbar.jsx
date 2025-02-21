import React from "react";
import { Link, useNavigate } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import "../css/Navbar.css";

export const Navbar = () => {
  const navigate = useNavigate();

  const rememberMe = localStorage.getItem("rememberMe") === "true";
  const googleLogged = JSON.parse(rememberMe ? localStorage.getItem("googleLogged") : sessionStorage.getItem("googleLogged"));
  const user = JSON.parse(rememberMe ? localStorage.getItem("user") : sessionStorage.getItem("user"));
  const googleUser = JSON.parse(rememberMe ? localStorage.getItem("googleUser") : sessionStorage.getItem("googleUser"));

  const navigateToProfile = () => {
    navigate("/profile");
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark fixed-top">
      <div className="container-fluid">
        <span className="navbar-brand" onClick={() => navigate("/")}>Érettségizzünk!</span>
        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav me-auto">
            <li className="nav-item">
              <Link to="/statistics" className="nav-link">Statisztika</Link>
            </li>
            <li className="nav-item">
              <Link to="/tutorial" className="nav-link">Útmutató</Link>
            </li>
            <li className="nav-item">
              <Link to="/search" className="nav-link">Feladat keresése</Link>
            </li>
            <li className="nav-item">
              <Link to="/selector" className="nav-link">Új feladatlap</Link>
            </li>
          </ul>
          {user ? (
            <div className="d-flex align-items-center">
              <span className="me-2 fs-5 text-white">{user.name || googleUser.displayName}</span>
              <img
                onClick={navigateToProfile}
                src={
                  googleLogged
                    ? `${googleUser.photoURL}?t=${new Date().getTime()}`
                    : `http://images.erettsegizzunk.nhely.hu/${user.profilePicturePath}?t=${new Date().getTime()}`
                }
                alt=""
                className="rounded-circle" style={{ maxHeight: "35px" }}
              />
            </div>
          ) : (
            <div className="d-flex">
              <Link to="/login" className="btn btn-outline-light me-2">Bejelentkezés</Link>
              <Link to="/register" className="btn btn-light">Regisztráció</Link>
            </div>
          )}
        </div>
      </div>
    </nav>
  );
}