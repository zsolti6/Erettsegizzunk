import React, { useState } from "react";
import axios from "axios";
import "bootstrap/dist/css/bootstrap.min.css";
import Navbar from "./Navbar";
import sha256 from "crypto-js/sha256";
import { useNavigate } from "react-router-dom";

function LoginPage() {
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigator = useNavigate();

  const togglePasswordVisibility = () => {
    setPasswordVisible(!passwordVisible);
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const saltUrl = "http://localhost:5000/erettsegizzunk/Login/SaltRequest";
      const saltResponse = await axios.post(saltUrl, JSON.stringify(username), {
        headers: {
          'Content-Type': 'application/json',
        },
      });

      const salt = saltResponse.data;
      const tmpHash = sha256(password + salt.toString()).toString();
      const loginUrl = "http://localhost:5000/erettsegizzunk/Login";
      const body = {
        loginName: username,
        tmpHash: tmpHash,
      };

      const loginResponse = await axios.post(loginUrl, body);
      if (loginResponse.status === 200) {
        const user = loginResponse.data;
        localStorage.setItem("user", JSON.stringify(user));
        console.log(loginResponse.data);
        
        // Send the token and user.id back to the specified link
        fetch('https://localhost:7066/erettsegizzunk/Token/add-token', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({ token: user.token, id: user.id }),
        })
          .then(response => {
            if (!response.ok) {
              throw new Error('Network response was not ok');
            }
            const contentType = response.headers.get('content-type');
            if (!contentType || !contentType.includes('application/json')) {
              throw new Error('Response is not JSON');
            }
            return response.json();
          })
          .then(data => {
            localStorage.setItem("profileId", data.profileId);
          })
          .catch(error => {
            console.error('Error:', error);
          });

        //alert("Sikeres bejelentkezés!");
        navigator("/"); // Corrected navigation
      } else {
        alert("Hiba történt a bejelentkezéskor!");
      }
    } catch (error) {
      alert("Hiba történt a bejelentkezéskor!");
    }
  };

  return (
    <div>
      <Navbar />
    <div className="container d-flex justify-content-center align-items-center" style={{ height: "100vh" }}>
      <div className="card p-4" style={{ width: "400px" }}>
        <h2 className="text-center mb-4">Login</h2>
        <form onSubmit={handleLogin}>
          <div className="form-group mb-3">
            <label htmlFor="username">Username</label>
            <input
              type="text"
              className="form-control"
              id="username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>
          <div className="form-group mb-3">
            <label htmlFor="password">Password</label>
            <div className="input-group">
              <input
                type={passwordVisible ? "text" : "password"}
                className="form-control"
                id="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
              <button
                type="button"
                className="btn btn-outline-secondary"
                onClick={togglePasswordVisibility}
              >
                {passwordVisible ? "Hide" : "Show"}
              </button>
            </div>
          </div>
          <button type="submit" className="btn btn-primary w-100">Belépés</button>
        </form>
      </div>
    </div>
    </div>
  );
}

export default LoginPage;