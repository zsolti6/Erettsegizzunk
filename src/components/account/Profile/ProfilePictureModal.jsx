import React from "react";
import { Modal, Button } from "react-bootstrap";
import { IMG_URL } from "../../../config";

export const ProfilePictureModal = ({
  show,
  onHide,
  selectedPicture,
  setSelectedPicture,
  handlePictureChange,
}) => {
  return (
    <Modal show={show} onHide={onHide} centered>
      <Modal.Header closeButton>
        <Modal.Title>Válassz profilképet</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <div className="d-flex justify-content-around">
          {["01", "02", "03", "04", "05"].map((num) => (
            <img
              key={num}
              src={`${IMG_URL}profile${num}.png`}
              alt={`profile${num}`}
              className={`rounded-circle ${
                selectedPicture === `profile${num}.png`
                  ? "border border-primary"
                  : ""
              }`}
              style={{
                width: "80px",
                height: "80px",
                cursor: "pointer",
                border:
                  selectedPicture === `profile${num}.png`
                    ? "3px solid blue"
                    : "none",
              }}
              onClick={() => setSelectedPicture(`profile${num}.png`)}
            />
          ))}
        </div>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>
          Mégse
        </Button>
        <Button
          variant="primary"
          onClick={handlePictureChange}
          disabled={!selectedPicture}
        >
          Profilkép módosítása
        </Button>
      </Modal.Footer>
    </Modal>
  );
};