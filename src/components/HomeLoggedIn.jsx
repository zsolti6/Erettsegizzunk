import "bootstrap/dist/css/bootstrap.min.css";
import "../css/SubPage.css";
import { FaDiscord } from "react-icons/fa";
import "react-multi-carousel/lib/styles.css";
import { FooterComponent } from "./Footer";
import axios from "axios";
import { useState, useEffect } from "react";
import { BASE_URL } from "../config";

export const HomeLoggedIn = ({ user }) => {
  const[streak, setStreak] = useState(0);

  useEffect(() => {
    const fetchStreak = async () => {
      try {
        const response = await axios.post(`${BASE_URL}/erettsegizzunk/UserStatistics/get-streak-count`, {
          userId: user.id,
          token: user.token,
        });
        setStreak(response.data);
      } catch (error) {
        // 
      }
    };

    if (user) {
      fetchStreak();
    }
  }, [user]);

  return (
    <div id="mainDiv" className="d-flex flex-column min-vh-100">
      {/* Hero Section */}
      <section
        className="hero-section text-center text-white d-flex justify-content-center align-items-center"
        style={{
          background: `linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)), url('https://images.unsplash.com/photo-1509062522246-3755977927d7?ixlib=rb-1.2.1&auto=format&fit=crop&w=1920&q=80')`,
          backgroundSize: "cover",
          backgroundPosition: "center",
          height: "100vh",
        }}
      >
        <div className="container">
          <h1 className="display-4">Üdv újra, {user.name}!</h1>
          <p className="lead text-white">
            Folytasd a gyakorlást, kövesd a fejlődésed, és készülj fel az érettségire!
          </p>
          <a href="/feladat-valasztas" className="btn btn-primary btn-lg color-bg2 border-0">
            Kezdjük a gyakorlást!
          </a>
          <div className="streak mt-5">
  {streak < 0 ? (
    <p className="streak-text text-white fs-2">Aktuális streak: {streak} nap
    <svg
    fill="#ff9029" 
    height="45px" 
    width="45px" 
    version="1.1" 
    id="Capa_1" 
    xmlns="http://www.w3.org/2000/svg" 
    xmlnsXlink="http://www.w3.org/1999/xlink" 
    viewBox="0 0 611.999 611.999" 
    xmlSpace="preserve" 
    stroke="#ff9029"
  >
    <g id="SVGRepo_bgCarrier" strokeWidth="0"></g>
    <g id="SVGRepo_tracerCarrier" strokeLinecap="round" strokeLinejoin="round"></g>
    <g id="SVGRepo_iconCarrier"> 
      <g> 
        <path d="M216.02,611.195c5.978,3.178,12.284-3.704,8.624-9.4c-19.866-30.919-38.678-82.947-8.706-149.952 c49.982-111.737,80.396-169.609,80.396-169.609s16.177,67.536,60.029,127.585c42.205,57.793,65.306,130.478,28.064,191.029 c-3.495,5.683,2.668,12.388,8.607,9.349c46.1-23.582,97.806-70.885,103.64-165.017c2.151-28.764-1.075-69.034-17.206-119.851 c-20.741-64.406-46.239-94.459-60.992-107.365c-4.413-3.861-11.276-0.439-10.914,5.413c4.299,69.494-21.845,87.129-36.726,47.386 c-5.943-15.874-9.409-43.33-9.409-76.766c0-55.665-16.15-112.967-51.755-159.531c-9.259-12.109-20.093-23.424-32.523-33.073 c-4.5-3.494-11.023,0.018-10.611,5.7c2.734,37.736,0.257,145.885-94.624,275.089c-86.029,119.851-52.693,211.896-40.864,236.826 C153.666,566.767,185.212,594.814,216.02,611.195z"></path> 
      </g> 
    </g>
  </svg></p>
    
  ) : (
    <p className="streak-text text-white fs-2">
  Nincs streak 
  <svg 
    viewBox="0 0 128 128" 
    xmlns="http://www.w3.org/2000/svg" 
    fill="#ff9029"
    width="45"
    height="45"
    className="ms-2"
  >
      <g id="SVGRepo_bgCarrier" strokeWidth="0"></g>
      <g id="SVGRepo_tracerCarrier" strokeLinecap="round" strokeLinejoin="round"></g>
      <g id="SVGRepo_iconCarrier">
        <path d="M64,12a37.89,37.89,0,0,0-25.3,9.65L39,22l4,12L26,59.5V99h76V50A38,38,0,0,0,64,12ZM50,49H60V39h8V49H78v8H68V76H60V57H50Z" fill="#707782"></path>
        <path d="M64,12c-1.18,0-2.36,0-3.51.16A38,38,0,0,1,95,50V99h7V50A38,38,0,0,0,64,12Z" fill="#fff"></path>
        <polygon points="68 57 68 76 60 76 60 57 50 57 50 49 60 49 60 39 68 39 68 49 78 49 78 57 68 57" fill="#b7bec8"></polygon>
        <rect height="18" fill="#b0b8c0" width="100" x="14" y="99"></rect>
        <path d="M14,99v18H52v-3a2,2,0,0,1,2-2H97a3,3,0,0,0,3-3V99Z" fill="#707782"></path>
        <path d="M95,93v6H26V59.5l6-9V88a5,5,0,0,0,5,5Z" fill="#58616d"></path>
        <rect height="18" fill="#eff8ff" width="7" x="107" y="99"></rect>
        <path d="M78,60H65V54H75V52H65V46H78a3,3,0,0,1,3,3v8A3,3,0,0,1,78,60Z" fill="#31352e"></path>
        <path d="M68,79H60a3,3,0,0,1-3-3V54h6V73h2V54h6V76A3,3,0,0,1,68,79Z" fill="#31352e"></path>
        <path d="M63,60H50a3,3,0,0,1-3-3V49a3,3,0,0,1,3-3H63v6H53v2H63Z" fill="#31352e"></path>
        <path d="M71,52H65V42H63V52H57V39a3,3,0,0,1,3-3h8a3,3,0,0,1,3,3Z" fill="#31352e"></path>
        <path d="M114,96H29V60.41L44.61,37H50c4,0,4-6,0-6H45.16l-2.83-8.48A35,35,0,0,1,99,50V90a3,3,0,0,0,6,0V50c0-35.41-42.06-54-68.31-30.58a3,3,0,0,0-.36,4.07l3.36,10.07L23.5,57.84A3,3,0,0,0,23,59.5V96H14a3,3,0,0,0-3,3v18a3,3,0,0,0,3,3H114a3,3,0,0,0,3-3V99A3,3,0,0,0,114,96Zm-3,18H17V102h94Z" fill="#31352e"></path>
      </g>
    </svg>
  </p>
  )}
</div>
        </div>
      </section>

      {/* Featured Section */}
      <section className="featured-section py-5" id="featured">
        <div className="container">
          <h2 className="text-center mb-5">Mit szeretnél csinálni ma?</h2>
          <div className="row">
            <div className="col-md-4 text-center">
              <div className="feature-item p-4 shadow-sm">
                <i className="fas fa-book-open fa-3x mb-3 text-primary"></i>
                <h3 className="text-primary">Új feladatok</h3>
                <p>Válassz új feladatokat és kezdj el gyakorolni különböző tantárgyakból.</p>
                <a href="/feladat-valasztas" className="btn btn-outline-primary mt-3">
                  Feladatok kiválasztása
                </a>
              </div>
            </div>
            <div className="col-md-4 text-center">
              <div className="feature-item p-4 shadow-sm">
                <i className="fas fa-user-friends fa-3x mb-3 text-info"></i>
                <h3 className="text-info">Közösség</h3>
                <p>Csatlakozz más diákokhoz és osszd meg tapasztalataidat a <a href="https://discord.gg/uR3uvaY5tp" target="_blank" className="discord-link"><FaDiscord className="fs-4"/> Discord</a> szerverünkön.</p>
              </div>
            </div>
            <div className="col-md-4 text-center">
              <div className="feature-item p-4 shadow-sm">
                <i className="fas fa-chart-line fa-3x mb-3 text-success"></i>
                <h3 className="text-success">Statisztikák</h3>
                <p>Kövesd a fejlődésed és nézd meg, mely területeken érdemes még gyakorolnod.</p>
                <a href="/statisztika" className="btn btn-outline-success mt-3">
                  Statisztikák megtekintése
                </a>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Call to Action Section */}
      <section
        className="cta-section text-center text-white py-5"
        style={{
          background: `linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)), url('https://images.pexels.com/photos/3184292/pexels-photo-3184292.jpeg?auto=compress&cs=tinysrgb&w=1920&h=1080&dpr=2')`,
          backgroundSize: "cover",
          backgroundPosition: "center",
        }}
      >
        <div className="container">
          <h2 className="mb-4">Ne hagyd abba a fejlődést!</h2>
          <p className="lead mb-4 text-white">
            Nézd meg a statisztikáidat, vagy válassz új feladatokat a további gyakorláshoz.
          </p>
          <a href="/statisztika" className="btn btn-light btn-lg color-bg3 border-0 me-3">
            Statisztikák
          </a>
          <a href="/feladat-valasztas" className="btn btn-light btn-lg color-bg2 border-0">
            Új teszt megkezdése
          </a>
        </div>
      </section>
      <FooterComponent />
    </div>
  );
};