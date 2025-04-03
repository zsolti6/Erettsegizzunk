import React, { useEffect, useState } from "react";
import axios from "axios";
import { BASE_URL } from "../../config";
import { StatisticsPieChart } from "./StatisticsPieChart";
import { LineGraph } from "./LineGraph";
import { DetailedStatistics } from "./DetailedStatistics";
import { FeatureCards } from "./FeatureCards";
import "../../css/SubPage.css";
import { useMediaQuery } from "react-responsive";
import { MessageModal } from "../common/MessageModal"; // Import the reusable MessageModal component

export const StatisticsComponent = ({ user }) => {
  const [loading, setLoading] = useState(true);
  const [userStats, setUserStats] = useState([]);
  const [fillingByDate, setFillingByDate] = useState([]);
  const [messageModal, setMessageModal] = useState({ show: false, type: "", message: "" }); // State for modal

  const isMobile = useMediaQuery({ query: "(max-width: 768px)" });

  useEffect(() => {
    if (user) {
      const fetchInitialData = async () => {
        try {
          setLoading(true);

          const [statsResponse, fillingResponse] = await Promise.all([
            axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-taskFilloutCount`, {
              userId: user.id,
              token: user.token,
            }),
            axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-filling-byDate`, {
              userId: user.id,
              token: user.token,
            }),
          ]);

          const statsData = statsResponse.data;
          setUserStats(Object.entries(statsData).map(([name, count]) => ({ name, count })));
          setFillingByDate(Object.entries(fillingResponse.data).map(([date, value]) => ({ date, value })));
        } catch (error) {
          setMessageModal({
            show: true,
            type: "error",
            message: "Hiba történt az adatok betöltése során. Kérjük, próbálja újra később.",
          });
        } finally {
          setLoading(false);
        }
      };

      fetchInitialData();
    }
  }, [user]);

  if (!user) {
    return (
      <div className="page-wrapper">
        <div className="text-center mt-5">
          <h1 style={{ marginTop: "5rem" }}>Bejelentkezés szükséges</h1>
          <p className="lead text-white mt-3">
            Jelentkezz be a statisztikák megtekintéséhez.
          </p>
          <div className="mt-4">
            <button
              className="btn color-bg3 btn-lg border-0"
              onClick={() => (window.location.href = "/belepes")}
            >
              Bejelentkezés
            </button>
          </div>
          <div className="mt-5">
            <FeatureCards />
          </div>
        </div>
      </div>
    );
  }

  if (loading) {
    return (
      <div className="d-flex justify-content-center align-items-center vh-100">
        <div className="spinner-container">
          <div className="spinner"></div>
        </div>
      </div>
    );
  }

  // Check if userStats is empty
  if (userStats.length === 0) {
    return (
      <div className="d-flex justify-content-center align-items-center vh-100">
        <div className="text-center">
          <h1 className="text-white">Nincsenek elérhető statisztikák</h1>
          <p className="lead text-white mt-3">
            Úgy tűnik, hogy még nem fejeztél be egyetlen feladatot sem. Kezdd el a gyakorlást, hogy megtekinthesd a statisztikáidat!
          </p>
          <button
            className="btn btn-primary btn-lg mt-4"
            onClick={() => (window.location.href = "/feladat-valasztas")}
          >
            Kezdj el gyakorolni
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className={`mt-4 top-padding w-100 ${isMobile ? "pt-5 p-2" : "p-5"}`}>
      <div className="row g-3 mb-4">
        <div className="col-12 col-md-6">
          <div className="card h-100 color-bg2">
            <div className="card-body">
              <h3 className="card-title text-center mb-0 text-white">Feladatok Statisztikája</h3>
              <StatisticsPieChart data={userStats} isMobile={isMobile} />
            </div>
          </div>
        </div>
        <div className="col-12 col-md-6">
          <div className="card h-100 color-bg2">
            <div className="card-body">
              <h3 className="card-title text-center mb-3 text-white">Kitöltések Dátum Szerint</h3>
              <LineGraph data={fillingByDate} isMobile={isMobile} />
            </div>
          </div>
        </div>
      </div>

      <div className="row">
        <div className="col-12">
          <div className="card taskCard color-bg2">
            <div className="card-body">
              <DetailedStatistics user={user} isMobile={isMobile} />
            </div>
          </div>
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