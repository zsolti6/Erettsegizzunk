import React from "react";
import { Link, useNavigate } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import "../css/Navbar.css";

export const Navbar = ({ user, googleLogged }) => {
  const navigate = useNavigate();
  const navigateToProfile = () => {
    navigate("/profil");
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark color-bg1 fixed-top">
      <div className="container-fluid">
        <span className="navbar-brand" onClick={() => navigate("/")}>Érettségizzünk!</span>
        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav me-auto">
            <li className="nav-item">
              <Link to="/statisztika" className="nav-link">Statisztika</Link>
            </li>
            <li className="nav-item">
              <Link to="/utmutato" className="nav-link">Útmutató</Link>
            </li>
            <li className="nav-item">
              <Link to="/feladat-kereses" className="nav-link">Feladat keresése</Link>
            </li>
            <li className="nav-item">
              <Link to="/feladat-valasztas" className="nav-link">Új feladatlap</Link>
            </li>
          </ul>
          {user ? (
            <div className="d-flex align-items-center">
              <span className="me-2 fs-5 text-white">{user.name || user.displayName}</span>
              <img
                onClick={navigateToProfile}
                src={
                  googleLogged
                    ? user.photoURL
                    : `https://res.cloudinary.com/drpkpopsq/image/upload/v1741078235/${user.profilePicturePath}`
                }
                alt="kep"
                className="rounded-circle" style={{ maxHeight: "35px" }}
              />
            </div>
          ) : (
            <div className="d-flex">
              <Link to="/belepes" className="btn btn-outline-light me-2">Bejelentkezés</Link>
              <Link to="/regisztracio" className="btn btn-light color-text2">Regisztráció</Link>
            </div>
          )}
        </div>
      </div>
    </nav>
  );
};