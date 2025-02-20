import React from "react";
import { Footer } from "flowbite-react";

function FooterComponent() {
  return (
    <Footer
      container
      style={{
        marginTop: "-8vh",
        position: "relative",
        bottom: 0,
        width: "100%",
        height: "8vh",
        zIndex: "2",
      }}
    >
      <div className="text-center p-3">
        <span>
          Elérhetőség: @
          <a href="https://github.com/pixel-pirates">Pixel Pirates</a>
        </span>
      </div>
    </Footer>
  );
}

export default FooterComponent;
