import React, { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "../css/SubPage.css";
import pngegg from "./pngegg.png";
import logo from "./logo.png";
import axios from "axios";
import { HiChevronDoubleDown } from "react-icons/hi2";
import Carousel from "react-multi-carousel";
import "react-multi-carousel/lib/styles.css";

export const Home = () => {
  const [articles, setArticles] = useState([]);

  useEffect(() => {
    const fetchRSS = async () => {
      try {
        const response = await axios.get(
          "https://api.allorigins.win/raw?url=" + encodeURIComponent("https://eduline.hu/rss")
        );

        const parser = new DOMParser();
        const xml = parser.parseFromString(response.data, "application/xml");
        const items = xml.querySelectorAll("item");

        const articleList = Array.from(items).map((item) => ({
          title: item.querySelector("title")?.textContent || "No title",
          link: item.querySelector("link")?.textContent || "#",
          description: item.querySelector("description")?.textContent || "No description available.",
        }));

        setArticles(articleList);
      } catch (error) {
        console.error("Error fetching RSS feed:", error);
      }
    };

    fetchRSS();
  }, []);

  const responsive = {
    superLargeDesktop: {
      breakpoint: { max: 4000, min: 3000 },
      items: 5
    },
    desktop: {
      breakpoint: { max: 3000, min: 1024 },
      items: 3
    },
    tablet: {
      breakpoint: { max: 1024, min: 464 },
      items: 2
    },
    mobile: {
      breakpoint: { max: 464, min: 0 },
      items: 1
    }
  };

  return (
    <div id="mainDiv" className="d-flex flex-column min-vh-100">
      <div id="subDiv" className="w-100 mt-5">
        <div id="textDiv">
          <div id="titleDiv">
            <img id="logo" className="img-fluid right" src={logo} alt="Logo" />
          </div>
          <div id="descriptionDiv" className="right">
            <p className="text-center">
              Szeretnél jó eredményt elérni az érettségin?<br />Jó helyen jársz!
            </p>
            <HiChevronDoubleDown className="arrow" />
          </div>
        </div>
        <img id="bg-image" src={pngegg} alt="Background" className="w-100" />
      </div>
      
      <div className="container mb-5">
        <Carousel responsive={responsive} infinite={true} autoPlay={true} autoPlaySpeed={3000}>
          {articles.map((article, index) => (
            <div key={index} className="card">
              <div className="card-body">
                <h5 className="card-title">{article.title}</h5>
                <p className="card-text">{article.description}</p>
                <a href={article.link} target="_blank" rel="noopener noreferrer" className="btn btn-primary">
                  Read more
                </a>
              </div>
            </div>
          ))}
        </Carousel>
      </div>
    </div>
  );
};