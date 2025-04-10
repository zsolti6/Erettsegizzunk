import React from "react";

export const FeatureCards = () => {
  const features = [
    {
      icon: "fas fa-chart-pie",
      title: "Statisztikák",
      description: "Kövesd a feladatok megoldásának eredményeit",
    },
    {
      icon: "fas fa-tasks",
      title: "Feladatok",
      description: "Gyakorolj különböző témakörökben",
    },
    {
      icon: "fas fa-calendar-alt",
      title: "Idővonal",
      description: "Nézd meg a kitöltéseid időrendben",
    },
  ];

  return (
    <div className="row justify-content-center">
      {features.map((feature, index) => (
        <div className="col-md-4 mb-4" key={index}>
          <div className="card color-bg2 h-100">
            <div className="card-body text-center">
              <i className={`${feature.icon} fa-3x text-white mb-3`}></i>
              <h4 className="card-title text-white">{feature.title}</h4>
              <p className="card-text text-white">{feature.description}</p>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
};
