import "bootstrap/dist/css/bootstrap.min.css";
import "../css/SubPage.css";
import { FaDiscord } from "react-icons/fa";
import "react-multi-carousel/lib/styles.css";
import { FooterComponent } from "./Footer";

export const HomeLoggedIn = ({ user }) => {
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