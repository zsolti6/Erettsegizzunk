import React, { useEffect, useState } from "react";
import "../css/SubPage.css";
import axios from "axios";
import { PieChart, Pie, Cell, Tooltip, Legend } from "recharts";
import { BASE_URL } from '../config';

const COLORS = ["#0088FE", "#FFBB28", "#FF0000"];

export const StatisticsComponent = ({ user }) => {
  const [userStatistics, setUserStatistics] = useState({});
  const [data, setData] = useState([]);

  console.log(user);
  

  useEffect(() => {
    if (!user) return; // Don't run if user is null

    console.log(user);

    const body = {
      id: user.id,
      token: user.token,
      subjectIds: [1, 2, 3],
      themeIds: [0],
    };

    axios
      .post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-one-statistics`, body)
      .then((response) => {
        setUserStatistics(response.data);
        console.log(response.data);
        setData(
          Object.entries(response.data).map(([name, [successRate, count]]) => ({
            name,
            count
          }))
        );
      })
      .catch((error) => {
        console.log(error);
      });
  }, [user]);

  return (
    <div>
      {user != null ? <Statistics data={data} /> : (
        <div className="text-center mt-5">
          <h1 style={{ marginTop: "5rem" }}>Bejelentkezés szükséges ennek a funkciónak a használatához</h1>
        </div>
      )}
    </div>
  );
};

function Statistics({ data }) {
  return (
    <PieChart width={400} height={400}>
      <Pie
        data={data}
        dataKey="count"
        nameKey="name"
        cx="50%"
        cy="60%"
        fill="#8884d8"
        outerRadius={100}
        label
      >
        {data.map((entry, index) => (
          <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
        ))}
      </Pie>
      <Legend />
      <Tooltip />
    </PieChart>
  );
}
