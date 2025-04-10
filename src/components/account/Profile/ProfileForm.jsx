import React from "react";
import { IMG_URL } from "../../../config";

export const ProfileForm = ({
  userData,
  handleInputChange,
  changePassword,
  setChangePassword,
  formData,
  setFormData,
  passwordVisible,
  togglePasswordVisibility,
  googleLogged,
  loading,
  handleSubmit,
  setShowModal,
  setShowPictureModal,
}) => {
  return (
    <form
      onSubmit={handleSubmit}
      className="profile-card bg-light p-4 rounded shadow mx-auto mt-5"
    >
      <div className="text-center position-relative">
        <div
          className="profile-picture-container mx-auto mt-0"
          style={{
            width: "110px",
            height: "120px",
            overflow: "hidden",
            border: "3px solid #fff",
            borderRadius: "15%",
            boxShadow: "0 0 5px rgba(0, 0, 0, 0.5)",
          }}
        >
          <img
            onClick={() => setShowPictureModal(true)}
            src={
              googleLogged
                ? userData.photoURL
                : `${IMG_URL}${userData.profilePicturePath || "default.png"}`
            }
            alt="Profile"
            className="img-fluid"
            style={{
              objectFit: "cover",
              cursor: "pointer",
            }}
          />
        </div>
      </div>
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
              }}
              className="form-check-input me-2"
            />
            <label htmlFor="passwordChange" className="form-check-label">
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
        onClick={() => setShowModal(true)}
        className="btn btn-danger text-white w-100 mb-2"
      >
        Statisztika visszaállítása
      </button>
    </form>
  );
};