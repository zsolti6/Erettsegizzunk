import React from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';

export const LineGraph = ({ data, isMobile }) => {
  return (
    <ResponsiveContainer width="100%" height={isMobile ? 300 : 350}>
      <LineChart data={data}>
        <CartesianGrid strokeDasharray="3 3"/>
        <XAxis 
          dataKey="date"
          label={{ 
            value: "Dátum", 
            position: "insideBottom", 
            offset: isMobile ? 0   : 5, 
            dy: isMobile ? 0 : 10, 
            style: { fill: 'white', fontSize: isMobile ? 12 : 18 } 
          }} 
          height={isMobile ? 65 : 50}
          tick={{ 
            fill: "white",
            fontSize: isMobile ? 10 : 12,
            angle: isMobile ? -45 : 0,
            textAnchor: isMobile ? 'end' : 'middle',
            dy: isMobile ? 0 : 10
          }}
        />
        <YAxis 
          label={{ 
            value: "Kitöltött feladatlapok", 
            angle: -90, 
            position: "insideLeft", 
            offset: isMobile ? 15 : 30, 
            dy: isMobile ? 50 : 75, 
            dx: isMobile ? -5 : -10, 
            style: { fill: 'white', fontSize: isMobile ? 12 : 18 } 
          }} 
          tick={{ fill: "white", fontSize: isMobile ? 10 : 14 }}
        />
        <Tooltip />
        <Line type="monotone" dataKey="value" stroke="#8884d8" strokeWidth={2} />
      </LineChart>
    </ResponsiveContainer>
  );
};