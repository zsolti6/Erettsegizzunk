import React from "react";
import { Link, useNavigate } from "react-router-dom";
import "../css/Navbar.css";

function Navbar() {
  const navigate = useNavigate();

  const rememberMe = localStorage.getItem("rememberMe") == "true";
  const googleLogged = JSON.parse(rememberMe ? localStorage.getItem("googleLogged") : sessionStorage.getItem("googleLogged"));
  const user = JSON.parse(rememberMe ? localStorage.getItem("user") : sessionStorage.getItem("user"));
  const googleUser = JSON.parse(rememberMe ? localStorage.getItem("googleUser") : sessionStorage.getItem("googleUser"));
  
  
  const navigateToProfile = () => {
    navigate("/profile");
  };

  return (
    <nav>
      <div onClick={() => navigate("/")} className="logo">
        Érettségizzünk!
      </div>
      <div className="link-container">
        <Link to="/statistics" className="home-link">Statisztika</Link>
        <Link to="/tutorial" className="home-link">Útmutató</Link>
        <Link to="/search" className="home-link">Feladat keresése</Link>
        <Link to="/selector" className="home-link">Új feladatlap</Link>

        {user ? (
          <div className="auth-links-wrapper noHover">
            <span>{user.name || googleUser.displayName}</span>
            <img
              onClick={navigateToProfile}
              src={
                googleLogged
                  ? `${googleUser.photoURL}?t=${new Date().getTime()}` 
                  : `http://images.erettsegizzunk.nhely.hu/${user.profilePicturePath}?t=${new Date().getTime()}`
              }
              alt="Profile"
              className="profile-picture"
              id="pp"
            />
          </div>
        ) : (
          <div className="auth-links-wrapper">
            <Link to="/login" className="home-link">Bejelentkezés</Link>
            <Link to="/register" className="home-link">Regisztráció</Link>
          </div>
        )}
      </div>
    </nav>
  );
}

export default Navbar;