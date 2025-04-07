import React, { useState } from "react";
import axios from "axios";
import { BASE_URL } from "../../config";
import "../../css/Login.css"; // Import the CSS file
import { MessageModal } from "../common/MessageModal"; // Import the reusable MessageModal component

export const PasswordReset = () => {
  const [email, setEmail] = useState("");
  const [messageModal, setMessageModal] = useState({ show: false, type: "", message: "" }); // State for modal
  const [loading, setLoading] = useState(false);

  const handlePasswordReset = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const response = await axios.post(
        `${BASE_URL}/erettsegizzunk/Password/elfelejtett-jelszo-keres`,
        JSON.stringify(email),
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (response.status === 200) {
        setMessageModal({
          show: true,
          type: "success",
          message: "Sikeres jelszó visszaállítási kérelem. Ellenőrizze az email fiókját!",
        });
      } else {
        setMessageModal({
          show: true,
          type: "error",
          message: "Hiba történt a kérés feldolgozásakor. Próbálja újra később.",
        });
      }
    } catch (error) {
      console.error("Password reset request failed", error);
      setMessageModal({
        show: true,
        type: "error",
        message: "Nem sikerült elküldeni a kérést. Ellenőrizze az email címet és próbálja újra.",
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <div
        className="login-container d-flex justify-content-center align-items-center"
        style={{ height: "100vh" }}
      >
        <div className="card p-4" style={{ width: "400px" }}>
          <h2 className="text-center mb-4">Jelszó visszaállítása</h2>
          <form onSubmit={handlePasswordReset}>
            <div className="form-group mb-3">
              <input
                placeholder="Email cím"
                type="email"
                className="form-control"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>

            <button
              type="submit"
              className="btn color-bg1 text-white w-100"
              disabled={loading}
            >
              {loading ? "Küldés..." : "Jelszó visszaállítási link küldése"}
            </button>

            <div className="d-flex justify-content-between mt-3">
              <a href="/belepes" className="text-muted">
                Vissza a bejelentkezéshez
              </a>
              <a href="/regisztracio" className="text-muted">
                Még nincs fiókod?
              </a>
            </div>
          </form>
        </div>
      </div>

      {/* Reusable Message Modal */}
      <MessageModal
        show={messageModal.show}
        type={messageModal.type}
        message={messageModal.message}
        onClose={() => setMessageModal({ ...messageModal, show: false })}
      />
    </div>
  );
};