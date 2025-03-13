import React, { useState, useEffect } from "react";
import "../css/SettingsWindow.css";

const SettingsWindow = ({ onClose }) => {
  const [bgColor, setBgColor] = useState(localStorage.getItem("bgColor") || "#303D5C");

  useEffect(() => {
    document.documentElement.style.setProperty('--bg-color', bgColor);
  }, [bgColor]);

  useEffect(() => {
    localStorage.setItem("bgColor", bgColor);
  }, [bgColor]);

  const handleBgColorChange = (e) => {
    setBgColor(e.target.value);
    document.documentElement.style.setProperty('--bg-color', e.target.value);
  };

  const handleReset = () => {
    const defaultColor = "#303d5c";
    setBgColor(defaultColor);
    document.documentElement.style.setProperty('--bg-color', defaultColor);
    localStorage.setItem("bgColor", defaultColor);
  };

  return (
    <div className="settings-window-overlay">
      <div className="settings-window">
        <h2>Settings</h2>
        <div className="settings-option">
          <label htmlFor="bgColor">Felhasználói felület színe:</label>
          <input
            type="color"
            id="bgColor"
            value={bgColor}
            onChange={handleBgColorChange}
          />
        </div>
        <div className="settings-buttons">
          <button className="reset-button" onClick={handleReset}>Reset</button>
          <button className="close-button" onClick={onClose}>Close</button>
        </div>
      </div>
    </div>
  );
};

export default SettingsWindow;