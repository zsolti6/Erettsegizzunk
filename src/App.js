import "./App.css";
import usePreventZoom from "./usePreventZoom";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "./components/Home";
import ExerciseComponent from "./components/ExerciseComponent";
import Login from "./components/Login";
import Register from "./components/Register";
import FooterComponent from "./components/Footer";
import StatisticsComponent from "./components/StatisticsComponent";
import TutorialComponent from "./components/TutorialComponent";
import SearchComponent from "./components/SearchComponent";
import SelectorComponent from "./components/SelectorComponent";
import ExericeStats from "./components/ExerciseStats";
import Profile from "./components/Profile";
import PasswordReset from "./components/PasswordReset";

function App() {
  usePreventZoom();
  return (
    <div className="App">
      <Router>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/statistics" element={<StatisticsComponent />} />
          <Route path="/tutorial" element={<TutorialComponent />} />
          <Route path="/search" element={<SearchComponent />} />
          <Route path="/exercise" element={<ExerciseComponent />} />
          <Route path="/selector" element={<SelectorComponent />} />
          <Route path="/exercise/stats" element={<ExericeStats />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/profile" element={<Profile />} />
          <Route path="/reset-password" element={<PasswordReset />} />
        </Routes>
        <FooterComponent />
      </Router>
    </div>
  );
}

export default App;