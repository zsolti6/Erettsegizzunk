import React, { useState, useEffect } from "react";
import "./App.css";
import usePreventZoom from "./usePreventZoom";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { Home } from "./components/Home";
import { HomeLoggedIn } from "./components/HomeLoggedIn";
import { ExerciseComponent } from "./components/exercise/ExerciseComponent.jsx";
import { LoginPage } from "./components/account/Login";
import { RegisterPage } from "./components/account/Register";
import { StatisticsComponent } from "./components/statistics/StatisticsComponent.jsx";
import { SelectorComponent } from "./components/SelectorComponent";
import { ExerciseStats } from "./components/exercise/ExerciseStats.jsx";
import { Profile } from "./components/account/Profile";
import { PasswordReset } from "./components/account/PasswordReset";
import { Navbar } from "./components/Navbar";
import { BASE_URL } from "./config";
import axios from "axios";
import { PolygonBackground } from "./components/Poly.tsx";
import { Navigate } from "react-router-dom";

export const App = () => {
  usePreventZoom();
  const [user, setUser] = useState(null);
  const [googleLogged, setGoogleLogged] = useState(false);
  const rememberMe = localStorage.getItem("rememberMe") === "true";

  // Load user and settings from storage on first render
  useEffect(() => {
    const storedUser = rememberMe
      ? localStorage.getItem("user")
      : sessionStorage.getItem("user");
    if (storedUser) {
      setUser(JSON.parse(storedUser));
    }

    const storedGoogleLogged = rememberMe
      ? localStorage.getItem("googleLogged")
      : sessionStorage.getItem("googleLogged");

    if (storedGoogleLogged) {
      setGoogleLogged(JSON.parse(storedGoogleLogged));
    }

    // Load background color from localStorage
    const storedBgColor = localStorage.getItem("bgColor") || "#303D5C";
    document.documentElement.style.setProperty("--bg-color", storedBgColor);
  }, []);

  // Check if user is active after state updates
  useEffect(() => {
    checkActiveStatus();
  }, [user]);

  const checkActiveStatus = () => {
    if (user && user.token) {
      axios
        .post(`${BASE_URL}/erettsegizzunk/Logout/active`, user.token, {
          headers: {
            "Content-Type": "application/json",
          },
        })
        .then((response) => {
          if (response.data === false) {
            handleLogout();
          }
        })
        .catch((error) => {
          console.error("Error checking active status:", error);
        });
    }
  };

  const handleLogout = async () => {
    await axios
      .post(`${BASE_URL}/erettsegizzunk/Logout`, user.token, {
        headers: {
          "Content-Type": "application/json",
        },
      })
      .then((response) => {
        console.log("Logout response:", response.data);
      })
      .catch((error) => {
        console.log("Error logging out:", error);
      });
    setUser(null);
    setGoogleLogged(false);
    if (rememberMe) {
      localStorage.removeItem("user");
      localStorage.removeItem("googleUser");
      localStorage.removeItem("googleLogged");
    } else {
      sessionStorage.removeItem("user");
      sessionStorage.removeItem("googleUser");
      sessionStorage.removeItem("googleLogged");
    }
    localStorage.removeItem("rememberMe");
    window.location.href = "/";
  };

  const handleLogin = (userData, isGoogleLogged) => {
    setUser(userData);
    setGoogleLogged(isGoogleLogged);
  };

  useEffect(() => {
    if (user) {
      if (rememberMe) {
        localStorage.setItem("user", JSON.stringify(user));
      } else {
        sessionStorage.setItem("user", JSON.stringify(user));
      }
    }
  }, [user]);

  return (
    <Router>
      <div className="d-flex flex-column min-vh-100">
        <PolygonBackground className="polygon-background" />
        <Navbar
          user={user}
          googleLogged={googleLogged}
          handleLogout={handleLogout}
        />
        <div className="flex-grow-1">
          <Routes>
            <Route
              path="/"
              element={user ? <HomeLoggedIn user={user} /> : <Home />}
            />
            <Route
              path="/statisztika"
              element={<StatisticsComponent user={user} />}
            />
            <Route path="/gyakorlas" element={<ExerciseComponent />} />
            <Route path="/feladat-valasztas" element={<SelectorComponent />} />
            <Route path="/gyakorlas/statisztika" element={<ExerciseStats />} />
            <Route
              path="/belepes"
              element={<LoginPage user={user} handleLogin={handleLogin} />}
            />
            <Route
              path="/regisztracio"
              element={<RegisterPage user={user} />}
            />
            <Route
              path="/profil"
              element={
                <Profile
                  user={user}
                  setUser={setUser}
                  googleLogged={googleLogged}
                  handleLogout={handleLogout}
                />
              }
            />
            <Route path="/elfelejtett-jelszo" element={<PasswordReset />} />
            <Route path="*" element={<Navigate to="/" />} />
          </Routes>
        </div>
      </div>
    </Router>
  );
};
