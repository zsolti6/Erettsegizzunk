import React from "react";
import "./SubPage.css";
import Sidenav from "./SideNav";
import Navbar from "./Navbar";

function ExerciseComponent({subject}){
    return(
        <div style={{
            height: "92vh"
        }}>
            <Navbar/>
            <div style={{ display: 'flex' }}>
                <Sidenav />
                <div style={{ marginLeft: '250px', padding: '20px', flex: 1 }}/>
            </div>
            <div className="exercises"/>
        </div>
    );
}

export default ExerciseComponent;