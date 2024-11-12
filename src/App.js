import './App.css';
import { Footer } from "flowbite-react";
import usePreventZoom from './usePreventZoom'

function App() {
  usePreventZoom();
  return (
    <div className="App border w-10" style={{
      height: "100vh"
    }}>
      <h1 style={{
        marginTop:"5vh"
      }}>Érettségizzünk</h1>
      <p style={{
        marginTop:"2vh"
      }}>Lorem ipsum dolor sit amet consectetur adipisicing elit. Ea, nostrum.</p>
      <div className='container h-50 w-100'>
        <div className='row h-100 w-100'>
          <div className='col col-md-4 col-sm-12 col-12'><SubjectComponent subject={"Történelem"}/></div>
          <div className='col col-md-4 col-sm-12 col-12'><SubjectComponent subject={"Matematika"}/></div>
          <div className='col col-md-4 col-sm-12 col-12'><SubjectComponent subject={"Magyar nyelv"}/></div>
        </div>
      </div>
      <Footer container style={{
        position:"absolute",
        left:0,
        bottom:0,
        right:0
      }}>
        <div className="text-center p-3">
          <span><a href=''>Pixel Pirates</a></span>
        </div>
      </Footer>
    </div>
  );
}

function SubjectComponent({subject}){
  return(
    <div className='bg-light w-75 p-3 d-inline-block border rounded align-middle' style={{
      height: "25vh",
      marginTop: "20vh"
    }}>
      <h3 style={{
        marginTop: "8vh"
      }}>{subject}</h3>
    </div>
  );
}

export default App;