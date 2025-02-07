import React, { useState } from "react";
import axios from "axios";
import "bootstrap/dist/css/bootstrap.min.css";
import Navbar from "./Navbar";
import sha256 from "crypto-js/sha256";
import { useNavigate } from "react-router-dom";
import { auth, provider, signInWithPopup } from "../firebaseConfig";

const handleGoogleLogin = async () => {
  try {
    const result = await signInWithPopup(auth, provider);
    const user = result.user;

    const body = {
      loginName: user.displayName,
      email: user.email,
      profilePicturePath: user.photoURL,
      permissionId: 1,
      active: true
    };

    const url = "http://localhost:5000/erettsegizzunk/Registry/googleLogin";
    await axios.post(url, body);

    console.log("Login successful!", user);
  } catch (error) {
    console.error("Google login failed", error);
  }
};

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

        navigator("/");
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
          <h2 className="text-center mb-4">Bejelentkezés</h2>
          <form onSubmit={handleLogin}>
            <div className="form-group mb-3">
              <input
              placeholder="Felhasználónév"
                type="text"
                className="form-control"
                id="username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                required
              />
            </div>
            <div className="form-group mb-3">
              <div className="input-group">
                <input
                placeholder="Jelszó"
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
                  {passwordVisible ? "Mutat" : "Elrejt"}
                </button>
                
              </div>
            </div>
            <button type="submit" className="btn btn-primary w-100">Belépés</button>
            {/* Links for Forgot Password and Registration */}
            <div className="d-flex justify-content-between mt-3">
              <a href="/forgot-password" className="text-muted">Elfelejtett jelszó?</a>
              <a href="/register" className="text-muted">Még nincs fiókod?</a>
            </div>
            <button class="googleLogin mt-3" onClick={handleGoogleLogin}>
              <svg xmlns="http://www.w3.org/2000/svg" preserveAspectRatio="xMidYMid" viewBox="0 0 256 262">
              <path fill="#4285F4" d="M255.878 133.451c0-10.734-.871-18.567-2.756-26.69H130.55v48.448h71.947c-1.45 12.04-9.283 30.172-26.69 42.356l-.244 1.622 38.755 30.023 2.685.268c24.659-22.774 38.875-56.282 38.875-96.027"></path>
              <path fill="#34A853" d="M130.55 261.1c35.248 0 64.839-11.605 86.453-31.622l-41.196-31.913c-11.024 7.688-25.82 13.055-45.257 13.055-34.523 0-63.824-22.773-74.269-54.25l-1.531.13-40.298 31.187-.527 1.465C35.393 231.798 79.49 261.1 130.55 261.1"></path>
              <path fill="#FBBC05" d="M56.281 156.37c-2.756-8.123-4.351-16.827-4.351-25.82 0-8.994 1.595-17.697 4.206-25.82l-.073-1.73L15.26 71.312l-1.335.635C5.077 89.644 0 109.517 0 130.55s5.077 40.905 13.925 58.602l42.356-32.782"></path>
              <path fill="#EB4335" d="M130.55 50.479c24.514 0 41.05 10.589 50.479 19.438l36.844-35.974C195.245 12.91 165.798 0 130.55 0 79.49 0 35.393 29.301 13.925 71.947l42.211 32.783c10.59-31.477 39.891-54.251 74.414-54.251"></path>
            </svg>
              Folytatás Google-fiókkal
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}

export default LoginPage;
