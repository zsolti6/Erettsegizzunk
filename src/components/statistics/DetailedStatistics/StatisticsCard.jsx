import React from "react";
import { PieChart, Pie, Cell, Tooltip, ResponsiveContainer } from "recharts";
import { FaChevronUp, FaChevronDown } from "react-icons/fa";
import { IMG_URL } from "../../../config";
import { FaImage } from "react-icons/fa";

const COLORS = ["#00FF00", "#FF0000"];

export const StatisticsCard = React.memo(
  ({ item, isExpanded, onToggleExpand, modalImage, setModalImage }) => {
    const correct = item.joRossz[0];
    const incorrect = item.joRossz[1];
    const total = correct + incorrect;
    const percentage = total > 0 ? ((correct / total) * 100).toFixed(1) : 0;

    const chartData = React.useMemo(
      () => [
        { name: "Helyes", value: correct },
        { name: "Helytelen", value: incorrect },
      ],
      [correct, incorrect]
    );

    const formattedDate = React.useMemo(
      () => new Date(item.utolsoKitoltesDatum).toLocaleDateString("hu-HU"),
      [item.utolsoKitoltesDatum]
    );

    const themesList = React.useMemo(
      () => item.task.themes.map((x) => x.name).join(", ") || "Nincs téma",
      [item.task.themes]
    );

    return (
      <div className="statisticsCard mb-3 color-bg3">
        <div
          className="card-header d-flex justify-content-between align-items-center cursor-pointer color-bg3 detailedTaskCardHeader"
          onClick={onToggleExpand}
          aria-expanded={isExpanded}
        >
          <div className="col-6 col-md-6 col-lg-6 col-10 text-truncate">
            {item.task.description}
          </div>

          <div className="col-6 col-md-2 col-lg-2 text-center d-none d-md-block">
            <b>Sikeres:</b> {correct}
          </div>

          <div className="col-6 col-md-2 col-lg-2 text-center d-none d-md-block">
            <b>Sikertelen:</b> {incorrect}
          </div>

          <div className="col-2 col-md-2 text-end">
            <span className="arrow">
              {isExpanded ? <FaChevronUp /> : <FaChevronDown />}
            </span>
          </div>
        </div>

        {isExpanded && (
          <div className="card-body">
            <div className="row">
              <div className="col-12 col-md-6 mb-3 mb-md-0">
                <div className="mb-3">
                  <div className="fw-bold">Feladat leírása:</div>
                  <div>{item.task.description}</div>
                </div>

                <div>
                  <div className="fw-bold">Feladat szövege:</div>
                  <div>{item.task.text}</div>
                </div>
                <br />
                <div className="text-center">
                  {item.task.picName && (
                    <FaImage
                      className="text-primary clickable-icon"
                      size={24}
                      title="Kép megtekintése"
                      onClick={() =>
                        setModalImage(`${IMG_URL}${item.task.picName}`)
                      }
                    />
                  )}
                </div>
              </div>

              <div className="col-12 col-md-2 mb-3 mb-md-0">
                <div className="mb-2">
                  <div className="fw-bold text-center">Témák:</div>
                  <div className="text-center">{themesList}</div>
                </div>

                <div>
                  <div className="fw-bold text-center">Tantárgy:</div>
                  <div className="text-center">{item.task.subject.name}</div>
                </div>
              </div>

              <div className="col-12 col-md-2 mb-3 mb-md-0">
                <div className="mb-2">
                  <div className="fw-bold text-center">Utolsó kitöltés:</div>
                  <div className="text-center">{formattedDate}</div>
                </div>

                <div>
                  <div className="fw-bold text-center">Eredmény:</div>
                  <div className="text-center">
                    {item.utolsoSikeres ? (
                      <span className="text-success">✅</span>
                    ) : (
                      <span className="text-danger">❌</span>
                    )}
                  </div>
                </div>
              </div>

              <div className="col-12 col-md-2 d-flex flex-column justify-content-center align-items-center">
                <div style={{ width: "100%", height: 100 }}>
                  <ResponsiveContainer>
                    <PieChart>
                      <Pie
                        data={chartData}
                        dataKey="value"
                        cx="50%"
                        cy="50%"
                        outerRadius={40}
                        innerRadius={30}
                        labelLine={false}
                      >
                        {chartData.map((entry, index) => (
                          <Cell
                            key={`cell-${index}`}
                            fill={COLORS[index % COLORS.length]}
                          />
                        ))}
                      </Pie>
                      <Tooltip
                        formatter={(value) => [
                          `${value} válasz`,
                          value === 1 ? "Helyes" : "Helytelen",
                        ]}
                      />
                    </PieChart>
                  </ResponsiveContainer>
                </div>
                <div className="mt-2">
                  <strong>{percentage}%</strong>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    );
  },
  (prevProps, nextProps) => {
    return (
      prevProps.item.id === nextProps.item.id &&
      prevProps.isExpanded === nextProps.isExpanded
    );
  }
);

StatisticsCard.displayName = "StatisticsCard";