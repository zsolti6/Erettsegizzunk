import React from "react";

export const FooterComponent = () => {
  return (
    <footer className="mt-auto py-3 bg-purple-800 text-white text-center color-bg1">
      <div className="container">
        <p className="mb-0 text-white">
          Github: @
          <a
            target="_blank"
            rel="noopener noreferrer"
            href="https://github.com/zsolti6/Erettsegizzunk"
            className="text-white hover:text-gray-300"
          >
            Érettségizzünk
          </a>
        </p>
      </div>
    </footer>
  );
};