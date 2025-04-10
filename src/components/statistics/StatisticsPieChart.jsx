import React from "react";
import {
  PieChart,
  Pie,
  Cell,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";

const COLORS = ["#0088FE", "#FFBB28", "#FF0000"];

export const StatisticsPieChart = ({ data, isMobile }) => {
  return (
    <ResponsiveContainer width="100%" height={isMobile ? 300 : 400}>
      <PieChart>
        <Pie
          data={data}
          dataKey="count"
          nameKey="name"
          cx="50%"
          cy="50%"
          outerRadius={isMobile ? 80 : 125}
          label
        >
          {data.map((entry, index) => (
            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
          ))}
        </Pie>
        <Legend
          layout={isMobile ? "horizontal" : "horizontal"}
          verticalAlign={isMobile ? "bottom" : "bottom"}
          align="center"
        />
        <Tooltip />
      </PieChart>
    </ResponsiveContainer>
  );
};