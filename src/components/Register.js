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

function GenerateRandomPassword(length = 16) {
  const lowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
  const upperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
  const digits = "0123456789";
  const specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?/";
  const allChars = lowerCaseChars + upperCaseChars + digits + specialChars;
  let password = "";
  password += lowerCaseChars[Math.floor(Math.random() * lowerCaseChars.length)];
  password += upperCaseChars[Math.floor(Math.random() * upperCaseChars.length)];
  password += digits[Math.floor(Math.random() * digits.length)];
  password += specialChars[Math.floor(Math.random() * specialChars.length)];
  for (let i = password.length; i < length; i++) {
    password += allChars[Math.floor(Math.random() * allChars.length)];
  }
  password = password.split('').sort(() => Math.random() - 0.5).join('');
  return password;
}

function RegisterPage() {
  const [formData, setFormData] = useState({
    loginName: "",
    password: "",
    confirmPassword: "",
    email: ""
  });
  const [error, setError] = useState("");
  const [passwordVisible, setPasswordVisible] = useState(false);

  const togglePasswordVisibility = () => {
    setPasswordVisible(!passwordVisible);
  };

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.id]: e.target.value });
  };

  const handleGeneratePassword = () => {
    const newPassword = GenerateRandomPassword();
    setFormData({ ...formData, password: newPassword, confirmPassword: newPassword });
  };

  const validateForm = () => {
    if (!formData.loginName) return "Kérjük, adja meg a felhasználónevet!";
    if (!formData.email) return "Kérjük, adja meg az email címét!";
    if (!formData.password) return "Kérjük, adjon meg egy jelszót!";
    if (formData.password !== formData.confirmPassword) return "A jelszavak nem egyeznek!";
    return "";
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    const validationError = validateForm();
    if (validationError) {
      setError(validationError);
      return;
    }
    setError("");
    try {
      const salt = GenerateSalt(64);
      const body = {
        loginName: formData.loginName,
        hash: sha256(formData.password + salt).toString(),
        salt: salt,
        permissionId: 1,
        active: true,
        email: formData.email,
        profilePicturePath: "default.jpg"
      };
      const url = "http://localhost:5000/erettsegizzunk/Registry/regisztracio";
      await axios.post(url, body);
    } catch (error) {
      setError('Hiba történt a regisztráció során: ' + (error.response ? error.response.data : error.message));
    }
  };

  return (
    <div>
      <Navbar />
      <div className="container d-flex justify-content-center align-items-center" style={{ height: "100vh" }}>
        <div className="card p-4" style={{ width: "400px" }}>
          <h2 className="text-center mb-4">Fiók létrehozása</h2>
          {error && <div className="alert alert-danger">{error}</div>}
          <form onSubmit={handleRegister}>
            <div className="form-group mb-3">
              <input placeholder="Felhasználónév" type="text" className="form-control" id="loginName" maxLength={10} value={formData.loginName} onChange={handleChange} />
            </div>
            <div className="form-group mb-3">
              <input placeholder="Email cím" type="email" className="form-control" id="email" value={formData.email} onChange={handleChange} />
            </div>
            <div className="form-group mb-3">
              <div className="input-group">
                <input placeholder="Jelszó" type={passwordVisible ? "text" : "password"} className="form-control" id="password" value={formData.password} onChange={handleChange} />
                <button type="button" className="btn btn-outline-secondary" onClick={togglePasswordVisibility}>{passwordVisible ? <i class="bi bi-eye"></i> : <i class="bi bi-eye-slash"></i>}</button>
                <button type="button" className="btn btn-outline-secondary" onClick={handleGeneratePassword} title="Jelszó generálása"><i class="bi bi-shuffle"></i></button>
              </div>
            </div>
            <div className="form-group mb-3">
              <input placeholder="Jelszó megerősítése" type="password" className="form-control" id="confirmPassword" value={formData.confirmPassword} onChange={handleChange} />
            </div>
            <button type="submit" className="btn btn-primary w-100">Regisztrálás</button>
          </form>
        </div>
      </div>
    </div>
  );
}

export default RegisterPage;