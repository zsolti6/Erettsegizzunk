import React from "react";
import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";

function Navbar() {
    const navigate = useNavigate();
    return (
        <nav style={navStyles}>
            <div onClick={() => navigate("/")} style={logoStyles}>Érettségizzünk!</div>
            <div style={linkContainerStyles}>
                <Link to="/tori" style={homeLinkStyles}>Történelem</Link>
                <Link to="/matek" style={homeLinkStyles}>Matematika</Link>
                <Link to="/magyar" style={homeLinkStyles}>Magyar nyelv</Link>
                <div style={authLinksWrapperStyles}>
                    <Link to="/login" style={linkStyles}>Bejelentkezés</Link>
                    <Link to="/register" style={linkStyles}>Regisztráció</Link>
                </div>
            </div>
        </nav>
    );
}

const navStyles = {
    position: 'fixed',
    top: 0,
    left: 0,
    width: '100%',
    backgroundColor: '#333',
    padding: '10px 20px',
    color: '#fff',
    zIndex: 1000,
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
};
  
const logoStyles = {
    fontSize: '24px',
    fontWeight: 'bold',
    marginLeft: '-5px',
};
  
const linkContainerStyles = {
    display: 'flex',
    alignItems: 'center',
    flex: 1,
};
  
const homeLinkStyles = {
    textDecoration: 'none',
    color: '#fff',
    fontSize: '18px',
    transition: 'color 0.3s',
    marginLeft: "25px",
};
  
const authLinksWrapperStyles = {
    display: 'flex',
    gap: '15px',
    marginLeft: 'auto',
};
  
const linkStyles = {
    textDecoration: 'none',
    color: '#fff',
    fontSize: '18px',
    transition: 'color 0.3s',
};

export default Navbar;