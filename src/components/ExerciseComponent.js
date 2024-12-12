import React, { useEffect, useState } from "react";
import "../css/SubPage.css";
import Sidenav from "./SideNav";
import Navbar from "./Navbar";
import ExerciseWindow from "./ExerciseWindow";
import axios from "axios";
import { useLocation } from "react-router-dom";

function ExerciseComponent() {
  const [exercises, setExercises] = useState([]);
  const [activeComponent, setActiveComponent] = useState(null);

  const location = useLocation();
  const { tantargy, szint } = location.state || { tantargy: "történelem", szint: "közép" };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const postData = { tantargy, szint };
        console.log(postData);
        
        const response = await axios.post("/erettsegizzunk/Feladatok/get-random-feladatok", postData);
        setExercises(response.data);
        console.log(response.data);
        
      } catch (error) {
        console.error("Error during POST request:", error);
      }
    };

    fetchData();
  }, [tantargy, szint]);

  return (
    <div style={{ height: "92vh" }}>
      <Navbar />
      <div style={{ display: "flex" }}>
        <Sidenav tasks={exercises} setActiveComponent={setActiveComponent} />
        <div style={{ padding: "20px", flex: 1 }}>
          {activeComponent && <ExerciseWindow task={activeComponent} />}
        </div>
      </div>
    </div>
  );
}

export default ExerciseComponent;
