import "./App.css";
import usePreventZoom from "./usePreventZoom";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "./Home";
import ExerciseComponent from "./ExerciseComponent";
import Login from "./Login";
import Register from "./Register";
import FooterComponent from "./Footer";

function App() {
  usePreventZoom();
  return (
    <div className="App">
      <Router>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route
            path="/matek"
            element={<ExerciseComponent subject={"Matematika"} />}
          />
          <Route
            path="/tori"
            element={<ExerciseComponent subject={"Történelem"} />}
          />
          <Route
            path="/magyar"
            element={<ExerciseComponent subject={"Magyar nyelv"} />}
          />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
        </Routes>
        <FooterComponent/>
      </Router>
    </div>
  );
}

export default App;
