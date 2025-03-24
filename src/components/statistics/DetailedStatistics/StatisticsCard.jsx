import React from 'react';
import { PieChart, Pie, Cell, Tooltip } from 'recharts';

const COLORSsmall = ["#00FF00", "#FF0000"];

export const StatisticsCard = ({ item, isExpanded, onToggleExpand }) => {
  const correct = item.joRossz[0];
  const incorrect = item.joRossz[1];
  const total = correct + incorrect;
  const percentage = total > 0 ? ((correct / total) * 100).toFixed(1) : 0;

  return (
    <div className="statisticsCard mb-3 color-bg3">
      <div
        className="card-header d-flex justify-content-between align-items-center cursor-pointer color-bg3 detailedTaskCardHeader"
        onClick={onToggleExpand}
      >
        <div className="col-6 col-md-4 col-lg-4 text-truncate">{item.task.description}</div>
        <div className="col-6 col-md-2 col-lg-4 text-center d-none d-md-block">
          <b>Sikeres:</b> {correct}
        </div>
        <div className="col-6 col-md-2 col-lg-2 text-center d-none d-md-block">
          <b>Sikertelen:</b> {incorrect}
        </div>
        <div className="col-2 col-md-2 text-end">
          <span className="arrow">{isExpanded ? "▲" : "▼"}</span>
        </div>
      </div>

      {isExpanded && (
        <div className="card-body">
          <div className="row">
            <div className="col-12 col-md-6 mb-3 mb-md-0">
              <div className="col-12 col-md-12 mb-3 mb-md-0">
                <div className="fw-bold">Feladat leírása:</div>
                <div>{item.task.description}</div>
              </div>
              <br />
              <div className="col-12 col-md-12 mb-3 mb-md-0">
                <div className="fw-bold">Feladat szövege:</div>
                <div>{item.task.text}</div>
              </div>
            </div>

            <div className="col-12 col-md-2 mb-3 mb-md-0">
              <div className="mb-2">
                <div className="fw-bold text-center">Témák:</div>
                <div className="text-center">
                  {item.task.themes.map(x => x.name).join(", ") || "Nincs téma"}
                </div>
              </div>
              <div>
                <div className="fw-bold text-center">Tantárgy:</div>
                <div className="text-center">{item.task.subject.name}</div>
              </div>
            </div>

            <div className="col-12 col-md-2 mb-3 mb-md-0">
              <div className="mb-2">
                <div className="fw-bold text-center">Utolsó kitöltés dátuma:</div>
                <div className="text-center">
                  {new Date(item.utolsoKitoltesDatum).toLocaleDateString("hu-HU")}
                </div>
              </div>
              <div>
                <div className="fw-bold text-center">Utolsó eredmény:</div>
                <div className="text-center">{item.utolsoSikeres ? "✅" : "❌"}</div>
              </div>
            </div>

            <div className="col-12 col-md-2 d-flex flex-column justify-content-center align-items-center">
              <PieChart width={100} height={100}>
                <Pie
                  data={[
                    { name: "Helyes", value: correct },
                    { name: "Helytelen", value: incorrect },
                  ]}
                  dataKey="value"
                  cx="50%"
                  cy="50%"
                  outerRadius={40}
                  label={false}
                >
                  {[...Array(2)].map((_, i) => (
                    <Cell key={i} fill={COLORSsmall[i]} />
                  ))}
                </Pie>
                <Tooltip />
              </PieChart>
              <div className="mt-2">{percentage}%</div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};