import React from 'react';
import './Sidenav.css';

function Sidenav() {
  const buttons = [];

  for (let i = 1; i <= 30; i++) {
    buttons.push(
      <button key={i}>Feladat {i}</button>
    );
  }
  return (
    <div className="sidenav">
      {buttons}
    </div> 
  );
}

export default Sidenav;