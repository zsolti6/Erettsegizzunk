import React, { useEffect, useState } from "react";

type Point = [number, number];
type Polygon = Point[];

const generatePolygons = (width: number, height: number, rows: number, cols: number): Polygon[] => {
  const points: Point[] = [];

  // Define fixed polygon size
  const polygonWidth = 200; // Fixed width for each polygon
  const polygonHeight = 150; // Fixed height for each polygon

  // Extend the area for both width and height
  const extendedWidth = width + polygonWidth * cols; // Extend beyond the screen
  const extendedHeight = height + polygonHeight * rows; // Extend beyond the screen

  // Generate grid points with randomness and extended width/height
  for (let i = 0; i <= rows; i++) {
    for (let j = 0; j <= cols; j++) {
      const x = j * polygonWidth + (Math.random() * polygonWidth * 0.5 - polygonWidth * 0.25);
      const y = i * polygonHeight + (Math.random() * polygonHeight * 0.5 - polygonHeight * 0.25);
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

      // Split the square into two triangles
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
  const [windowDimensions, setWindowDimensions] = useState({ width: window.innerWidth, height: window.innerHeight });

  useEffect(() => {
    const handleResize = () => {
      const { innerWidth, innerHeight } = window;
      setWindowDimensions((prevDimensions) => {
        // Only update state if dimensions have actually changed
        if (prevDimensions.width !== innerWidth || prevDimensions.height !== innerHeight) {
          return { width: innerWidth, height: innerHeight };
        }
        return prevDimensions;
      });
    };

    window.addEventListener("resize", handleResize);
    return () => window.removeEventListener("resize", handleResize);
  }, []);

  useEffect(() => {
    setPolygons(generatePolygons(windowDimensions.width, windowDimensions.height, rows, cols));
  }, [windowDimensions, rows, cols]);

  const themeColors = ["var(--primary-shade-1)", "var(--primary-shade-2)", "var(--primary-shade-3)", "var(--primary-shade-4)", "var(--primary-shade-5)"];

  return (
    <>
      <svg
        className="polygon-background"
        xmlns="http://www.w3.org/2000/svg"
        viewBox={`0 0 ${windowDimensions.width} ${windowDimensions.height}`}
      >
        {polygons.map((points, index) => (
          <polygon
            key={index}
            points={points.map((p) => p.join(",")).join(" ")}
            fill={themeColors[Math.floor(Math.random() * themeColors.length)]}
          />
        ))}
      </svg>
    </>
  );
};

