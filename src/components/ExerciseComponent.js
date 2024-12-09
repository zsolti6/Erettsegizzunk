import React from "react";
import "../css/SubPage.css";
import Sidenav from "./SideNav";
import Navbar from "./Navbar";

function ExerciseComponent(){
    return(
        <div style={{
            height: "92vh"
        }}>
            <Navbar/>
            <div style={{ display: 'flex' }}>
                <Sidenav />
                <div style={{ padding: '20px', flex: 1 }} />
            </div>
            <div className="exercise">
                
            </div>
        </div>
    );
}

export default ExerciseComponent;
