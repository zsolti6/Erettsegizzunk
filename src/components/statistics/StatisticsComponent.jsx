import React, { useEffect, useState } from "react";
import axios from "axios";
import { BASE_URL } from '../../config';
import { StatisticsPieChart } from './StatisticsPieChart';
import { LineGraph } from './LineGraph';
import { DetailedStatistics } from './DetailedStatistics';
import { FeatureCards } from './FeatureCards';
import '../../css/SubPage.css';
import { useMediaQuery } from 'react-responsive';

export const StatisticsComponent = ({ user }) => {
  const [errorMessage, setErrorMessage] = useState("");
  const [loading, setLoading] = useState(true);
  const [userStats, setUserStats] = useState([]);
  const [fillingByDate, setFillingByDate] = useState([]);
  
  const isMobile = useMediaQuery({ query: '(max-width: 768px)' });

  useEffect(() => {
    if (user) {
      const fetchInitialData = async () => {
        try {
          setLoading(true);
          
          const [statsResponse, fillingResponse] = await Promise.all([
            axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-taskFilloutCount`, {
              userId: user.id,
              token: user.token
            }),
            axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-filling-byDate`, {
              userId: user.id,
              token: user.token
            })
          ]);

          setUserStats(Object.entries(statsResponse.data).map(([name, count]) => ({ name, count })));
          setFillingByDate(Object.entries(fillingResponse.data).map(([date, value]) => ({ date, value })));
        } catch (error) {
          console.error("Error fetching statistics:", error);
          setErrorMessage("Hiba történt az adatok betöltése során.");
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
              onClick={() => window.location.href = "/belepes"}
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

  if (errorMessage) {
    return (
      <div className="d-flex justify-content-center align-items-center vh-100">
        <div className="alert alert-danger text-center">
          {errorMessage}
        </div>
      </div>
    );
  }

  return (
    <div className={`mt-4 top-padding w-100 ${isMobile ? 'p-2' : 'p-5'}`}>
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
    </div>
  );
};