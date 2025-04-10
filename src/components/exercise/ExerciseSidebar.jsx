import React from "react";
import { Sidenav } from "../SideNav";
import "bootstrap-icons/font/bootstrap-icons.css";

export const ExerciseSidebar = ({
  isOpen,
  setIsOpen,
  exercises,
  activeIndex,
  setActiveIndex,
  taskValues,
}) => {
  return (
    <>
      <button
        className="btn btn-primary m-2 color-bg1 border-0"
        onClick={() => setIsOpen(!isOpen)}
        style={{
          position: "fixed",
          top: "70px",
          left: isOpen ? "260px" : "10px",
          zIndex: 1001,
          transition: "left 0.3s",
        }}
      >
        <i className={`bi bi-${isOpen ? "x" : "list"}`}></i>
      </button>

      <div
        className="sidenav p-0"
        style={{
          width: "250px",
          position: "fixed",
          left: isOpen ? "0" : "-250px",
          transition: "left 0.3s",
          zIndex: 1000,
          marginTop: "32px",
          height: "100vh",
        }}
      >
        <Sidenav
          tasks={exercises}
          isOpen={isOpen}
          setIsOpen={setIsOpen}
          setActiveComponent={setActiveIndex}
          activeIndex={activeIndex}
          taskValues={taskValues}
        />
      </div>
    </>
  );
};