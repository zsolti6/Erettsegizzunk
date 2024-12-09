import React from 'react';
import '../css/Sidenav.css';

function Sidenav() {
  const buttons = [];

  for (let i = 1; i <= 15; i++) {
    buttons.push(
      <button key={i} onClick={() => {
        console.log(`Feladat ${i}`)
      }}>Feladat {i}</button>
    );
  }
  return (
    <div className="sidenav">
      {buttons}
    </div> 
  );
}

export default Sidenav;