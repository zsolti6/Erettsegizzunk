import React from "react";
import { useNavigate } from "react-router-dom";

function Profile() {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem('user');
    navigate("/login");
  };

  return (
    <div>
      <h1>Profile</h1>
      <button onClick={handleLogout}>Logout</button>
    </div>
  );
}

export default Profile;