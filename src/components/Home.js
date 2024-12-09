import React from 'react';
import { useNavigate } from 'react-router-dom';
import "../css/SubPage.css";

function Home() {
    return (
        <div className="App border w-10" style={{
          height: "92vh"
        }}>
          <h1 style={{
            marginTop:"5vh"
          }}>Érettségizzünk!</h1>
          <p style={{
            marginTop:"2vh"
          }}>Egy applikáció, ami segítséget nyújt érettségi előtt álló diákoknak a felkészüléshez.</p>
          <div className='container h-50 w-100'>
            <div className='row h-100 w-100'>
              <div className='col col-md-4 col-12 cursor'><SubjectComponent text={"Statisztika"} linkto={"/statistics"}/></div>
              <div className='col col-md-4 col-12 cursor'><SubjectComponent text={"Új feladatlap"} linkto={"/selector"}/></div>
              <div className='col col-md-4 col-12 cursor'><SubjectComponent text={"Útmutató"} linkto={"/tutorial"}/></div>
            </div>
          </div>
        </div>
    );
}

export default Home;

function SubjectComponent({text, linkto}){
    const navigate = useNavigate();
    return(
        <div onClick={() => navigate(linkto)} className='bg-light w-75 p-3 d-inline-block border rounded align-middle' style={{
            height: "25vh",
            marginTop: "20vh"
        }}>
        <h3 style={{
            marginTop: "8vh"
        }}>{text}</h3>
        </div>
    );
}