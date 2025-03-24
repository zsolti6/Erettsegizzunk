import React from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';

export const LineGraph = ({ data }) => {
  return (
    <ResponsiveContainer width="100%" height="75%">
      <LineChart data={data}>
        <CartesianGrid strokeDasharray="3 3"/>
        <XAxis 
          dataKey="date"
          label={{ value: "Dátum", position: "insideBottom", offset: 5, dy: 10, style: { fill: 'white' } }} 
          tick={{ fill: "white"}}
        />
        <YAxis 
          label={{ value: "Kitöltött feladatlapok", angle: -90, position: "insideLeft", offset: 30, dy: 75, dx: -10, style: { fill: 'white' } }} 
          tick={{ fill: "white"}}
        />
        <Tooltip />
        <Line type="monotone" dataKey="value" stroke="#8884d8" strokeWidth={2} />
      </LineChart>
    </ResponsiveContainer>
  );
};