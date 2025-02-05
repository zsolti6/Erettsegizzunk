import React, { useState } from "react";
import axios from "axios";
import "bootstrap/dist/css/bootstrap.min.css";
import sha256 from "crypto-js/sha256";
import Navbar from "./Navbar";

function GenerateSalt(SaltLength) {
  const karakterek = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
  let salt = "";

  for (let i = 0; i < SaltLength; i++) {
    const randomIndex = Math.floor(Math.random() * karakterek.length);
    salt += karakterek[randomIndex];
  }

  return salt;
}

function RegisterPage() {
  const [formData, setFormData] = useState({
    loginName: "",
    password: "",
    name: "",
    email: ""
  });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.id]: e.target.value });
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      const salt = GenerateSalt(64);
      const body = {
        loginName: formData.loginName,
        hash: sha256(formData.password + salt).toString(),
        salt: salt,
        name: formData.name,
        permissionId: 1, // Set permissionId to 1
        active: true,
        email: formData.email,
        profilePicturePath: "default.jpg" // Set profilePicturePath to "default.jpg"
      };

      const url = "http://localhost:5000/erettsegizzunk/Registry";
      const response = await axios.post(url, body);
      //alert(response.data);
    } catch (error) {
      console.error('Error:', error.response ? error.response.data : error.message);
    }
  };

  return (
    <div>
      <Navbar />
    <div className="container d-flex justify-content-center align-items-center" style={{ height: "100vh" }}>
      <div className="card p-4" style={{ width: "400px" }}>
        <h2 className="text-center mb-4">Regisztráció</h2>
        <form onSubmit={handleRegister}>
          <div className="form-group mb-3">
            <label htmlFor="loginName">Felhasználónév</label>
            <input
              type="text"
              className="form-control"
              id="loginName"
              value={formData.loginName}
              onChange={handleChange}
              required
            />
          </div>
          <div className="form-group mb-3">
            <label htmlFor="password">Jelszó</label>
            <input
              type="password"
              className="form-control"
              id="password"
              value={formData.password}
              onChange={handleChange}
              required
            />
          </div>
          <div className="form-group mb-3">
            <label htmlFor="name">Teljes név</label>
            <input
              type="text"
              className="form-control"
              id="name"
              value={formData.name}
              onChange={handleChange}
              required
            />
          </div>
          <div className="form-group mb-3">
            <label htmlFor="email">Email cím</label>
            <input
              type="email"
              className="form-control"
              id="email"
              value={formData.email}
              onChange={handleChange}
              required
            />
          </div>
          <button type="submit" className="btn btn-primary w-100">Regisztrálás</button>
        </form>
      </div>
    </div>
    </div>
  );
}

export default RegisterPage;