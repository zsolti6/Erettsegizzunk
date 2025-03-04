import React, { useEffect, useState } from "react";
import "../css/SubPage.css";
import axios from "axios";
import { PieChart, Pie, Cell, Tooltip, Legend, LineChart, Line, XAxis, YAxis, CartesianGrid } from "recharts";
import { BASE_URL } from '../config';

const COLORS = ["#0088FE", "#FFBB28", "#FF0000"];

export const StatisticsComponent = ({ user }) => {
  if (!user) {
    return (
      <div className="text-center mt-5">
        <h1 style={{ marginTop: "5rem" }}>Bejelentkezés szükséges ennek a funkciónak a használatához</h1>
      </div>
    );
  }

  return (
    <div>
      <UserStatisticsChart id="pie" user={user} />
      <FillingByDateChart id="graph" user={user} />
    </div>
  );
};

const UserStatisticsChart = ({ user }) => {
  const [data, setData] = useState([]);

  useEffect(() => {
    const fetchUserStatistics = async () => {
      try {
        const body = {
          id: user.id,
          token: user.token,
          subjectIds: [1, 2, 3],
          themeIds: [0],
        };
        const response = await axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-one-statistics`, body);
        const formattedData = Object.entries(response.data).map(([name, [successRate, count]]) => ({ name, count }));
        setData(formattedData);
      } catch (error) {
        console.error("Error fetching user statistics:", error);
      }
    };

    fetchUserStatistics();
  }, [user]);

  return <StatisticsPieChart data={data} />;
};

const FillingByDateChart = ({ user }) => {
  const [data, setData] = useState([]);

  useEffect(() => {
    const fetchFillingByDate = async () => {
      try {
        const body = { userId: user.id, token: user.token };
        const response = await axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-filling-byDate`, body);
        const formattedData = Object.entries(response.data).map(([date, value]) => ({ date, value }));
        setData(formattedData);
      } catch (error) {
        console.error("Error fetching filling by date:", error);
      }
    };

    fetchFillingByDate();
  }, [user]);

  return <LineGraph data={data} />;
};

const StatisticsPieChart = ({ data }) => {
  return (
    <PieChart width={400} height={400}>
      <Pie data={data} dataKey="count" nameKey="name" cx="50%" cy="60%" outerRadius={100} label>
        {data.map((entry, index) => (
          <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
        ))}
      </Pie>
      <Legend />
      <Tooltip />
    </PieChart>
  );
};

const LineGraph = ({ data }) => {
  return (
    <LineChart width={600} height={300} data={data}>
      <CartesianGrid strokeDasharray="3 3" />
      <XAxis dataKey="date" />
      <YAxis />
      <Tooltip />
      <Legend />
      <Line type="monotone" dataKey="value" stroke="#8884d8" strokeWidth={2} />
    </LineChart>
  );
};