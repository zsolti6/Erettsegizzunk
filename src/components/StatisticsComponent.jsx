import React from "react";
import "../css/SubPage.css";
import axios from "axios";
import { useEffect, useState } from "react";
import { PieChart, Pie, Cell, Tooltip, Legend } from "recharts";
import { BASE_URL } from '../config';

const COLORS = ["#0088FE", "#FFBB28", "#FF0000"];

export const StatisticsComponent = ({user}) => {
  const [userStatistics, setUserStatistics] = React.useState({});
  const [data, setData] = useState([]);

  useEffect(() => {
    if (!user) return;  // Don't run if user is null
    
    console.log(user);
  
    const body = {
      id: user.id,
      token: user.token,
      subjectIds: [1, 2, 3],
      themeIds: [0]
    };
  
    axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-one-statistics`, body)
      .then((response) => {
        setUserStatistics(response.data);
        console.log(response.data);
        setData(Object.entries(response.data).map(([name, [successRate, count]]) => ({
          name,
          count
        })));
        console.log(data);
      })
      .catch((error) => {
        console.log(error);
      });
  }, [user]);
  
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