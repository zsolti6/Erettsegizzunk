import axios from "axios";
import Navbar from "./Navbar";
import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.min.css';

function Profile() {
  const [userData, setUserData] = useState({
      id: 0,
      name: "string",
      email: "string",
      permission: 0,
      newsletter: false,
      profilePicture: "string",
      profilePicturePath: "string",
      token: "string"
  });
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [changePassword, setChangePassword] = useState(false);
  const rememberMe = localStorage.getItem("rememberMe") == "true";
  const navigate = useNavigate();

  const togglePasswordVisibility = () => {
    setPasswordVisible(!passwordVisible);
  };

  useEffect(() => {
    if ((rememberMe ? localStorage.getItem("user") : sessionStorage.getItem("user")) == null) {
      navigate("/Login");
    }
  }, [navigate, rememberMe]);

  useEffect(() => {
    const storedUser = rememberMe ? localStorage.getItem("user") : sessionStorage.getItem("user");
    if (storedUser) {
      const parsedUser = JSON.parse(storedUser);
      setUserData({
        name: parsedUser.name || "",
        email: parsedUser.email || "",
        newsletter: parsedUser.newsletter || false,
        id: parsedUser.id || 0,
        permission: parsedUser.permission || 0,
        profilePicture: parsedUser.profilePicture || null,
        profilePicturePath: parsedUser.profilePicturePath || "string",
        token: parsedUser.token || "string"
      });
    }
  }, [rememberMe]);

  const formData = {
    token: userData.token,
    loginName: userData.name,
    oldPassword: "",
    newPassword: ""
  }

  const handlePasswordChange = () => {
    if(changePassword){
      axios.put("https://localhost:7066/erettsegizzunk/Password/jelszo-modositas", formData)
      .then((response) => {
        console.log(response);
      });
    }
  }

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setUserData({
      ...userData,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const handleLogout = () => {
    axios
      .post(
        "http://localhost:5000/erettsegizzunk/Logout",
        JSON.stringify(JSON.parse(rememberMe ? localStorage.getItem("user") : sessionStorage.getItem("user")).token),
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      )
      .then((response) => {
        if (response.status === 200) {
          if(rememberMe){
            localStorage.removeItem("user");
            localStorage.removeItem("googleUser");
            localStorage.removeItem("googleLogged");
          }else{
            sessionStorage.removeItem("user");
            sessionStorage.removeItem("googleUser");
            localStorage.removeItem("googleLogged");
          }
          localStorage.removeItem("rememberMe");
        }
      });
    navigate("/login");
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    console.log(userData);

    axios.put("https://localhost:7066/erettsegizzunk/User/sajat-felhasznalo-modosit", userData)
    .then((response) => {
      console.log(response);
    });
  };

  return (
    <div className="d-flex flex-column min-vh-100">
      <Navbar />
      <div className="container mt-5 mb-5">
        <h1 className="text-center mb-4 mt-5">Profilom</h1>
        <form onSubmit={handleSubmit} className="bg-light p-4 rounded shadow mx-auto mt-5" style={{ maxWidth: '500px' }}>
          <div className="mb-3">
            <label htmlFor="name" className="form-label">
              Felhasználónév
            </label>
            <input
              type="text"
              id="name"
              name="name"
              value={userData.name}
              onChange={handleInputChange}
              className="form-control"
              required
            />
          </div>
          <div className="mb-3">
            <label htmlFor="email" className="form-label">
              Email cím
            </label>
            <input
              type="email"
              id="email"
              name="email"
              value={userData.email}
              onChange={handleInputChange}
              className="form-control"
              disabled={(rememberMe ? localStorage.getItem("googleLogged") === "true" : sessionStorage.getItem("googleLogged") === "true")}
              required
            />
          </div>
          <div className="mb-3 mt-1 d-flex align-items-center">
            <input
              type="checkbox"
              id="passwordChange"
              name="passwordChange"
              checked={/*userData.newsletter*/ false}
              onChange={handleInputChange}
              className="form-check-input me-2"
            />
            <label htmlFor="passwordChange" style={{lineHeight: "1"}} className="form-check-label">
              Szeretnék jelszavat változtatni
            </label>
          </div>
          <div className="form-group mb-3">
  <input 
    placeholder="Jelszó" 
    type="password"  // Always hidden
    className="form-control" 
    id="password" 
    value={formData.password} 
  />
</div>

<div className="form-group mb-3">
  <div className="input-group">
    <input 
      placeholder="Jelszó megerősítése" 
      type={passwordVisible ? "text" : "password"}
      className="form-control" 
      id="confirmPassword" 
      value={formData.confirmPassword} 
    />
    <button 
      type="button" 
      className="btn btn-outline-secondary" 
      onClick={togglePasswordVisibility}
    >
      {passwordVisible ? <i className="bi bi-eye"></i> : <i className="bi bi-eye-slash"></i>}
    </button>
  </div>
</div>

          <div className="mb-3 mt-1">
            <input
              type="checkbox"
              id="newsletter"
              name="newsletter"
              checked={userData.newsletter}
              onChange={handleInputChange}
              className="form-check-input"
            />
            <label htmlFor="newsletter" className="form-check-label ms-2">
              Feliratkozom a hírlevélre
            </label>
          </div>
          <button onClick={handlePasswordChange} type="submit" className="btn btn-primary w-100 mb-3">
            Mentés
          </button>
        </form>
        {(rememberMe ? !localStorage.getItem("googleUser") : !sessionStorage.getItem("googleUser")) &&
        <button
          onClick={console.log("asd")}
          className="btn btn-primary mt-3"
        >
          Jelszó megváltoztatása
        </button>
        }<br/>
        <button
          onClick={handleLogout}
          className="btn btn-primary mt-3"
        >
          Kijelentkezés
        </button>
      </div>
    </div>
  );
}

export default Profile;