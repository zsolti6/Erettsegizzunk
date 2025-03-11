import React, { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "../css/SubPage.css";
import pngegg from "./pngegg.png";
import logo from "./logo.png";
import axios from "axios";
import { HiChevronDoubleDown } from "react-icons/hi2";

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
      
      <div className="container mt-4">
        <div className="row">
          {articles.map((article, index) => (
            <div key={index} className="col-md-4 mb-4">
              <div className="card">
                <div className="card-body">
                  <h5 className="card-title">{article.title}</h5>
                  <p className="card-text">{article.description}</p>
                  <a href={article.link} target="_blank" rel="noopener noreferrer" className="btn btn-primary">
                    Read more
                  </a>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};
