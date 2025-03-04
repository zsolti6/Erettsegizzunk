import React, { useState, useEffect } from "react";
import "./App.css";
import usePreventZoom from "./usePreventZoom";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { Home } from "./components/Home";
import { ExerciseComponent } from "./components/ExerciseComponent";
import { LoginPage } from "./components/Login";
import { RegisterPage } from "./components/Register";
import { FooterComponent } from "./components/Footer";
import { StatisticsComponent } from "./components/StatisticsComponent";
import { TutorialComponent } from "./components/TutorialComponent";
import { SearchComponent } from "./components/SearchComponent";
import { SelectorComponent } from "./components/SelectorComponent";
import { ExerciseStats } from "./components/ExerciseStats";
import { Profile } from "./components/Profile";
import { PasswordReset } from "./components/PasswordReset";
import { Navbar } from "./components/Navbar";

export const App = () => {
  usePreventZoom();
  const [user, setUser] = useState(null);
  const [googleLogged, setGoogleLogged] = useState(false);
  const rememberMe = localStorage.getItem("rememberMe") === "true";
  
  useEffect(() => {
    const storedUser = rememberMe ? localStorage.getItem("user") : sessionStorage.getItem("user");
    if (storedUser) {
      setUser(JSON.parse(storedUser));
    }
    
    const storedGoogleLogged = rememberMe ? localStorage.getItem("googleLogged") : sessionStorage.getItem("googleLogged");
    if (storedGoogleLogged) {
      setGoogleLogged(JSON.parse(storedGoogleLogged));
    }
  }, []); // No dependency on `rememberMe` to avoid unnecessary re-runs
  

  const handleLogout = () => {
    setUser(null);
    setGoogleLogged(false);
    if (rememberMe) {
      localStorage.removeItem("user");
      localStorage.removeItem("googleUser");
      localStorage.removeItem("googleLogged");
    } else {
      sessionStorage.removeItem("user");
      sessionStorage.removeItem("googleUser");
      localStorage.removeItem("googleLogged");
    }
    localStorage.removeItem("rememberMe");
  };

  const handleLogin = (userData, isGoogleLogged) => {
    setUser(userData);
    setGoogleLogged(isGoogleLogged);
  };

  useEffect(() => {
    if(user){
      if (rememberMe) {
        localStorage.setItem("user", JSON.stringify(user));
      } else {
        sessionStorage.setItem("user", JSON.stringify(user));
      }
    }
  }, [user]);

  return (
    <div>
      <Router>
        <Navbar user={user} googleLogged={googleLogged} handleLogout={handleLogout} />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/statisztika" element={<StatisticsComponent user={user} />} />
          <Route path="/utmutato" element={<TutorialComponent />} />
          <Route path="/feladat-kereses" element={<SearchComponent />} />
          <Route path="/gyakorlas" element={<ExerciseComponent />} />
          <Route path="/feladat-valasztas" element={<SelectorComponent />} />
          <Route path="/gyakorlas/statisztika" element={<ExerciseStats />} />
          <Route path="/belepes" element={<LoginPage handleLogin={handleLogin} />} />
          <Route path="/regisztracio" element={<RegisterPage />} />
          <Route path="/profil" element={<Profile user={user} setUser={setUser} googleLogged={googleLogged} handleLogout={handleLogout} />} />
          <Route path="/elfelejtett-jelszo" element={<PasswordReset />} />
        </Routes>
        <FooterComponent />
      </Router>
    </div>
  );
};