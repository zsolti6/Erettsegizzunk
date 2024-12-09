import React from "react";
import "../css/SubPage.css";
import Navbar from "./Navbar";

function StatisticsComponent(){
    return(
        <div style={{
            height: "92vh"
        }}>
            <Navbar/>
            <div style={{ display: 'flex' }}>
                <div style={{ marginLeft: '250px', padding: '20px', flex: 1 }}/>
            </div>
            <div className="statistics">
                    
            </div>
        </div>
    );
}

export default StatisticsComponent;