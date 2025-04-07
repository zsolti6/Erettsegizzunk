import React, { useState, useEffect } from "react";
import axios from "axios";
import sha256 from "crypto-js/sha256";
import { useNavigate } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import { BASE_URL } from "../../config";
import "../../css/Profile.css";
import { MessageModal } from "../common/MessageModal"; // Import the reusable MessageModal component
import { Modal, Button } from "react-bootstrap"; // Import Bootstrap Modal

export const Profile = ({ user, setUser, googleLogged, handleLogout }) => {
  const [userData, setUserData] = useState({
    id: 0,
    name: "string",
    email: "string",
    permission: 0,
    newsletter: false,
    profilePicture: "string",
    profilePicturePath: "string",
    token: "string",
  });
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [changePassword, setChangePassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [showModal, setShowModal] = useState(false); // State to control confirmation modal visibility
  const [messageModal, setMessageModal] = useState({ show: false, type: "", message: "" }); // State for error/success modal
  const navigate = useNavigate();

  const togglePasswordVisibility = () => {
    setPasswordVisible(!passwordVisible);
  };

  useEffect(() => {
    if (user == null) {
      navigate("/belepes");
    }
  }, [navigate, user]);

  const [formData, setFormData] = useState({
    token: "",
    loginName: "",
    oldPassword: "",
    newPassword: "",
  });

  useEffect(() => {
    if (user) {
      setUserData({
        name: user.name || "",
        email: user.email || "",
        newsletter: user.newsletter || false,
        id: user.id || 0,
        permission: user.permission || 0,
        profilePicture: user.profilePicture || null,
        profilePicturePath: user.profilePicturePath || "string",
        token: user.token || "string",
      });

      setFormData({
        token: user.token || "string",
        loginName: user.name || "string",
        oldPassword: "",
        newPassword: "",
      });
    }
  }, [user, changePassword]);

  const resetStatistics = async () => {
    const body = {
      userId: user.id,
      token: user.token,
    };

    try {
      await axios.request({
        method: "DELETE",
        url: `${BASE_URL}/erettsegizzunk/UserStatistics/statisztika-reset`,
        data: body,
        headers: {
          "Content-Type": "application/json",
        },
      });
      setMessageModal({ show: true, type: "success", message: "A statisztika sikeresen visszaállítva." });
    } catch (error) {
      setMessageModal({
        show: true,
        type: "error",
        message: error.response?.data?.message || "Hiba történt a statisztika visszaállítása során.",
      });
    } finally {
      setShowModal(false);
    }
  };

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setUserData({
      ...userData,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    if (changePassword) {
      try {
        const saltUrl = `${BASE_URL}/erettsegizzunk/Login/SaltRequest`;
        const saltResponse = await axios.post(
          saltUrl,
          JSON.stringify(formData.loginName),
          {
            headers: {
              "Content-Type": "application/json",
            },
          }
        );

        const salt = saltResponse.data;
        const tmpHashOldPswd = sha256(
          formData.oldPassword + salt.toString()
        ).toString();
        const tmpHashNewPswd = sha256(
          formData.newPassword + salt.toString()
        ).toString();

        const updatedFormData = {
          ...formData,
          oldPassword: tmpHashOldPswd,
          newPassword: tmpHashNewPswd,
        };

        await axios.post(
          `${BASE_URL}/erettsegizzunk/Password/jelszo-modositas`,
          updatedFormData
        ).then((response) => {
          setMessageModal({ show: true, type: "success", message: response.message });
        }).catch((error) => {
          setMessageModal({
            show: true,
            type: "error",
            message: error.response?.data?.message || "Hiba történt a jelszó módosítása során.",
          });
        });

        
      } catch (error) {
        setMessageModal({
          show: true,
          type: "error",
          message: error.response?.data?.message || "Hiba történt a jelszó módosítása során.",
        });
        setLoading(false);
        return;
      }
    }

    try {
      await axios
        .put(`${BASE_URL}/erettsegizzunk/User/sajat-felhasznalo-modosit`, userData)
        .then((response) => {
          if (response.status === 200) {
            setChangePassword(false);
            setUser(userData);
            setMessageModal({ show: true, type: "success", message: "Felhasználói adatok sikeresen frissítve!" });
          }
        });
    } catch (error) {
      setMessageModal({
        show: true,
        type: "error",
        message: error.response?.data?.message || "Hiba történt az adatok frissítése során.",
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="profile-container d-flex flex-column min-vh-100">
      <div className="container mt-5 mb-5">
        <h1 className="text-center mb-3 mt-5 text-white">Adataim</h1>
        <form
          onSubmit={handleSubmit}
          className="profile-card bg-light p-4 rounded shadow mx-auto mt-4"
        >
          <div className="mb-1">
            <label htmlFor="name" className="form-label">
              Felhasználónév
            </label>
            <input
              maxLength={10}
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
              disabled={googleLogged}
              required
            />
          </div>
          {!googleLogged && (
            <div>
              <div className="mb-3 mt-1 d-flex align-items-center">
                <input
                  type="checkbox"
                  id="passwordChange"
                  name="passwordChange"
                  checked={changePassword}
                  onChange={() => {
                    setChangePassword(!changePassword);
                    setFormData({
                      ...formData,
                      oldPassword: "",
                      newPassword: "",
                    });
                    setPasswordVisible(false);
                  }}
                  className="form-check-input me-2"
                />
                <label
                  htmlFor="passwordChange"
                  style={{ lineHeight: "1" }}
                  className="form-check-label"
                >
                  Szeretnék jelszavat változtatni
                </label>
              </div>
              <div className="form-group mb-3">
                <input
                  placeholder="Régi jelszó"
                  type="password"
                  className="form-control"
                  id="password"
                  disabled={!changePassword}
                  onChange={(e) =>
                    setFormData({ ...formData, oldPassword: e.target.value })
                  }
                  value={formData.oldPassword}
                />
              </div>
              <div className="form-group mb-3">
                <div className="input-group">
                  <input
                    placeholder="Új jelszó"
                    type={passwordVisible ? "text" : "password"}
                    className="form-control"
                    id="confirmPassword"
                    disabled={!changePassword}
                    onChange={(e) =>
                      setFormData({ ...formData, newPassword: e.target.value })
                    }
                    value={formData.newPassword}
                  />
                  <button
                    type="button"
                    className="btn btn-outline-secondary"
                    onClick={togglePasswordVisibility}
                    disabled={!changePassword}
                  >
                    {passwordVisible ? (
                      <i className="bi bi-eye"></i>
                    ) : (
                      <i className="bi bi-eye-slash"></i>
                    )}
                  </button>
                </div>
              </div>
            </div>
          )}
          <div className="mb-3 mt-1">
            <input
              type="checkbox"
              id="newsletter"
              name="newsletter"
              checked={userData.newsletter === true}
              onChange={handleInputChange}
              className="form-check-input"
            />
            <label htmlFor="newsletter" className="form-check-label ms-2">
              Feliratkozom a hírlevélre
            </label>
          </div>
          <button
            type="submit"
            className="btn color-bg1 text-white w-100 mb-2"
            disabled={loading}
          >
            {loading ? "Mentés folyamatban..." : "Mentés"}
          </button>
          <button
            type="button"
            onClick={() => setShowModal(true)} // Show the confirmation modal
            className="btn btn-danger text-white w-100 mb-2"
          >
            Statisztika visszaállítása
          </button>
        </form>
      </div>

      {/* Confirmation Modal */}
      <Modal show={showModal} onHide={() => setShowModal(false)} centered>
        <Modal.Header closeButton>
          <Modal.Title>Statisztika visszaállítása</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          Biztosan vissza szeretnéd állítani a statisztikádat? Ez a művelet nem
          vonható vissza.
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowModal(false)}>
            Mégse
          </Button>
          <Button variant="danger" onClick={resetStatistics}>
            Visszaállítás
          </Button>
        </Modal.Footer>
      </Modal>

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
