import React, { useState, useEffect } from "react";
import "../css/SettingsWindow.css";
import { presetColors } from "../config";

const SettingsWindow = ({ onClose }) => {
  const [bgColor, setBgColor] = useState(
    localStorage.getItem("bgColor") || "#303D5C"
  );

  useEffect(() => {
    document.documentElement.style.setProperty("--bg-color", bgColor);
  }, [bgColor]);

  useEffect(() => {
    localStorage.setItem("bgColor", bgColor);
  }, [bgColor]);

  const handleColorSelect = (color) => {
    setBgColor(color);
    document.documentElement.style.setProperty("--bg-color", color);
    localStorage.setItem("bgColor", color);
  };

  const handleReset = () => {
    const defaultColor = "#303d5c";
    setBgColor(defaultColor);
    document.documentElement.style.setProperty("--bg-color", defaultColor);
    localStorage.setItem("bgColor", defaultColor);
  };

  return (
    <div className="settings-window-overlay">
      <div className="settings-window">
        <h2>Beállítások</h2>
        <div className="settings-option">
          <label>Felhasználói felület színe:</label>
          <div className="color-selector">
            {presetColors.map((color) => (
              <button
                key={color}
                style={{
                  backgroundColor: color,
                  width: "30px",
                  height: "30px",
                  margin: "5px",
                  border: "none",
                  cursor: "pointer",
                }}
                onClick={() => handleColorSelect(color)}
              />
            ))}
          </div>
        </div>
        <div className="settings-buttons">
          <button className="reset-button" onClick={handleReset}>
            Visszaállítás
          </button>
          <button className="close-button" onClick={onClose}>
            Bezár
          </button>
        </div>
      </div>
    </div>
  );
};

export default SettingsWindow;