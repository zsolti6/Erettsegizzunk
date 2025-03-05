import React from "react";
import { Footer } from "flowbite-react";

export const FooterComponent = () => {
  return (
    <Footer
    id="footer"
      container
      style={{
        position: "relative",
        bottom: 0,
        width: "100%",
        height: "6vh",
        zIndex: "2",
      }}
    >
      <div className="text-center p-2 color-bg3 h-100 text-light">
        <p>
          Elérhetőség: @
          <a href="https://github.com/pixel-pirates">Pixel Pirates</a>
        </p>
      </div>
    </Footer>
  );
}