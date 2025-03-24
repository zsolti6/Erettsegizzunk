import React from 'react';
import { PieChart, Pie, Cell, Tooltip, Legend, ResponsiveContainer } from 'recharts';

const COLORS = ["#0088FE", "#FFBB28", "#FF0000"];

export const StatisticsPieChart = ({ data }) => {
  return (
    <ResponsiveContainer width="100%" height={400}>
      <PieChart>
        <Pie 
          data={data} 
          dataKey="count" 
          nameKey="name" 
          cx="50%" 
          cy="50%" 
          outerRadius={125} 
          label
        >
          {data.map((entry, index) => (
            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
          ))}
        </Pie>
        <Legend />
        <Tooltip />
      </PieChart>
    </ResponsiveContainer>
  );
};