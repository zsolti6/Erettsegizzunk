import React, { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { saltRequest } from "../common/saltRequest";
import { ProfilePictureModal } from "./Profile/ProfilePictureModal";
import { ResetStatisticsModal } from "./Profile/ResetStatisticsModal";
import { ProfileForm } from "./Profile/ProfileForm";
import { MessageModal } from "../common/MessageModal";
import "../../css/Profile.css";
import { BASE_URL } from "../../config";
import { sha256 } from "crypto-js";

export const Profile = ({ user, setUser, googleLogged }) => {
  const [userData, setUserData] = useState({});
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [changePassword, setChangePassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [messageModal, setMessageModal] = useState({
    show: false,
    type: "",
    message: "",
  });
  const [showPictureModal, setShowPictureModal] = useState(false);
  const [selectedPicture, setSelectedPicture] = useState("");
  const [formData, setFormData] = useState({});
  const navigate = useNavigate();

  const togglePasswordVisibility = () => {
    setPasswordVisible(!passwordVisible);
  };

  useEffect(() => {
    if (!user) {
      navigate("/belepes");
    } else {
      setUserData(user);
      setFormData({
        token: user.token || "",
        loginName: user.name || "",
        oldPassword: "",
        newPassword: "",
      });
    }
  }, [navigate, user]);

  const resetStatistics = async () => {
    try {
      await axios.delete(
        `${BASE_URL}/erettsegizzunk/UserStatistics/statisztika-reset`,
        {
          data: { userId: user.id, token: user.token },
          headers: { "Content-Type": "application/json" },
        }
      );
      setMessageModal({
        show: true,
        type: "success",
        message: "A statisztika sikeresen visszaállítva.",
      });
    } catch (error) {
      setMessageModal({
        show: true,
        type: "error",
        message:
          error.response?.data?.message ||
          "Hiba történt a statisztika visszaállítása során.",
      });
    } finally {
      setShowModal(false);
    }
  };

  const handlePictureChange = async () => {
    if (!selectedPicture) return;

    try {
      const updatedUserData = { ...userData, profilePicturePath: selectedPicture };
      await axios.put(
        `${BASE_URL}/erettsegizzunk/User/sajat-felhasznalo-modosit`,
        updatedUserData,
        { headers: { "Content-Type": "application/json" }
      });
      setUser(updatedUserData);
      setMessageModal({
        show: true,
        type: "success",
        message: "Profilkép sikeresen frissítve!",
      });
    } catch (error) {
      setMessageModal({
        show: true,
        type: "error",
        message:
          error.response?.data?.message ||
          "Hiba történt a profilkép frissítése során.",
      });
    } finally {
      setShowPictureModal(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    if (changePassword) {
      try {
        const salt = await saltRequest(formData.loginName);
        const tmpHashOldPswd = sha256(
          formData.oldPassword + salt.toString()
        ).toString();
        const tmpHashNewPswd = sha256(
          formData.newPassword + salt.toString()
        ).toString();

        await axios.post(
          `${BASE_URL}/erettsegizzunk/Password/jelszo-modositas`,
          {
            ...formData,
            oldPassword: tmpHashOldPswd,
            newPassword: tmpHashNewPswd,
          }
        );
        setMessageModal({
          show: true,
          type: "success",
          message: "Jelszó sikeresen módosítva.",
        });
      } catch (error) {
        setMessageModal({
          show: true,
          type: "error",
          message:
            error.response?.data?.message ||
            "Hiba történt a jelszó módosítása során.",
        });
        setLoading(false);
        return;
      }
    }

    try {
      await axios.put(
        `${BASE_URL}/erettsegizzunk/User/sajat-felhasznalo-modosit`,
        userData
      );
      setChangePassword(false);
      setUser(userData);
      setMessageModal({
        show: true,
        type: "success",
        message: "Felhasználói adatok sikeresen frissítve!",
      });
    } catch (error) {
      setMessageModal({
        show: true,
        type: "error",
        message:
          error.response?.data?.message ||
          "Hiba történt az adatok frissítése során.",
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="profile-container d-flex flex-column min-vh-100">
      <div className="container mt-5 mb-5">
        <ProfileForm
          userData={userData}
          setShowPictureModal={setShowPictureModal}
          handleInputChange={(e) =>
            setUserData({
              ...userData,
              [e.target.name]: e.target.type === "checkbox" ? e.target.checked : e.target.value,
            })
          }
          changePassword={changePassword}
          setChangePassword={setChangePassword}
          formData={formData}
          setFormData={setFormData}
          passwordVisible={passwordVisible}
          togglePasswordVisibility={togglePasswordVisibility}
          googleLogged={googleLogged}
          loading={loading}
          handleSubmit={handleSubmit}
          setShowModal={setShowModal}
        />
      </div>
      <ResetStatisticsModal
        show={showModal}
        onHide={() => setShowModal(false)}
        resetStatistics={resetStatistics}
      />
      <ProfilePictureModal
        show={showPictureModal}
        onHide={() => setShowPictureModal(false)}
        selectedPicture={selectedPicture}
        setSelectedPicture={setSelectedPicture}
        handlePictureChange={handlePictureChange}
      />
      <MessageModal
        show={messageModal.show}
        type={messageModal.type}
        message={messageModal.message}
        onClose={() => setMessageModal({ ...messageModal, show: false })}
      />
    </div>
  );
};