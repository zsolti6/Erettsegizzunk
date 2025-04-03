import React, { useEffect, useState } from "react";

type Point = [number, number];
type Polygon = Point[];

const generatePolygons = (width: number, height: number, rows: number, cols: number): Polygon[] => {
  const points: Point[] = [];
  const polygonWidth = 200;
  const polygonHeight = 150;
  const extendedWidth = width + polygonWidth * cols * 0.2;
  const extendedHeight = height + polygonHeight * rows * 0.2;

  // Generate grid points with randomness
  for (let i = 0; i <= rows; i++) {
    for (let j = 0; j <= cols; j++) {
      const x = (j * extendedWidth) / cols + (Math.random() * width) / cols * 0.5 - 100;
      const y = (i * extendedHeight) / rows + (Math.random() * height) / rows * 0.5 - 50;
      points.push([x, y]);
    }
  }

  const polygons: Polygon[] = [];
  for (let i = 0; i < rows; i++) {
    for (let j = 0; j < cols; j++) {
      const topLeft = points[i * (cols + 1) + j];
      const topRight = points[i * (cols + 1) + j + 1];
      const bottomLeft = points[(i + 1) * (cols + 1) + j];
      const bottomRight = points[(i + 1) * (cols + 1) + j + 1];

      if (Math.random() > 0.5) {
        polygons.push([topLeft, bottomLeft, bottomRight]);
        polygons.push([topLeft, topRight, bottomRight]);
      } else {
        polygons.push([topLeft, bottomLeft, topRight]);
        polygons.push([topRight, bottomLeft, bottomRight]);
      }
    }
  }

  return polygons;
};

export const PolygonBackground: React.FC<{ rows?: number; cols?: number }> = ({ rows = 6, cols = 10 }) => {
  const [polygons, setPolygons] = useState<Polygon[]>([]);
  const [initialDimensions] = useState({
    width: typeof window !== 'undefined' ? window.innerWidth : 1200,
    height: typeof window !== 'undefined' ? window.innerHeight : 800
  });

  // Generate polygons only once on mount
  useEffect(() => {
    setPolygons(generatePolygons(
      initialDimensions.width,
      initialDimensions.height,
      rows,
      cols
    ));
  }, [initialDimensions, rows, cols]);

  const themeColors = [
    "var(--primary-shade-1)",
    "var(--primary-shade-2)",
    "var(--primary-shade-3)",
    "var(--primary-shade-4)",
    "var(--primary-shade-5)"
  ];

  return (
    <svg
      className="polygon-background"
      xmlns="http://www.w3.org/2000/svg"
      viewBox={`0 0 ${initialDimensions.width} ${initialDimensions.height}`}
      preserveAspectRatio="none"
      style={{
        position: 'fixed',
        top: 0,
        left: 0,
        width: '100%',
        height: '100%',
        zIndex: -1,
        pointerEvents: 'none' // Makes the background non-interactive
      }}
    >
      {polygons.map((points, index) => (
        <polygon
          key={`poly-${index}`}
          points={points.map((p) => p.join(",")).join(" ")}
          fill={themeColors[Math.floor(Math.random() * themeColors.length)]}
          opacity="0.7" // Slightly transparent for better readability
        />
      ))}
    </svg>
  );
};