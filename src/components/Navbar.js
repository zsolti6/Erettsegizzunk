import React from "react";
import { Link, useNavigate } from "react-router-dom";
import "../css/Navbar.css";

function Navbar() {
  const navigate = useNavigate();
  
  const googleLogged = JSON.parse(localStorage.getItem("googleLogged"));
  const user = JSON.parse(localStorage.getItem("user"));
  const googleUser = JSON.parse(localStorage.getItem("googleUser"));
 
  
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
                  ? googleUser.photoURL // Google profile picture
                  : `http://images.erettsegizzunk.nhely.hu/${user.profilePicturePath}` // Local profile picture
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
