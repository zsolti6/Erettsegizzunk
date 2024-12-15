import React from "react";
import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import "../css/Navbar.css";

function Navbar() {
    const navigate = useNavigate();
    return (
        <nav>
            <div onClick={() => navigate("/")} className="logo">Érettségizzünk!</div>
            <div className="link-container">
                <Link to="/statistics" className="home-link">Statisztika</Link>
                <Link to="/tutorial" className="home-link">Útmutató</Link>
                <Link to="/search" className="home-link">Feladat keresése</Link>
                <Link to="/selector" className="home-link">Új feladatlap</Link>
                <div className="auth-links-wrapper">
                    <Link to="/login" className="auth-link">Bejelentkezés</Link>
                    <Link to="/register" className="auth-link">Regisztráció</Link>
                </div>
            </div>
        </nav>
    );
}

export default Navbar;