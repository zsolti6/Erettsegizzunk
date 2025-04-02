import React from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';

const CustomTooltip = ({ active, payload }) => {
  if (active && payload && payload.length) {
    return (
      <div className="custom-tooltip" style={{ backgroundColor: '#333', padding: '5px 10px', borderRadius: '5px', color: 'white' }}>
        <p className='text-white'>{`Kitöltések: ${payload[0].value}`}</p>
      </div>
    );
  }
  return null;
};

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
            angle: isMobile ? -45 : -25,
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
        <Tooltip content={<CustomTooltip />} />
        <Line type="monotone" dataKey="value" stroke="#8884d8" strokeWidth={2} />
      </LineChart>
    </ResponsiveContainer>
  );
};