import React from "react";
import { Modal, Button } from "react-bootstrap";

export const MessageModal = ({ show, type, message, onClose }) => {
  return (
    <Modal show={show} onHide={onClose} centered>
      <Modal.Header closeButton>
        <Modal.Title>
          {type === "success" ? "Sikeres művelet" : "Hiba"}
        </Modal.Title>
      </Modal.Header>
      <Modal.Body>{message}</Modal.Body>
      <Modal.Footer>
        <Button variant="primary" onClick={onClose}>
          Rendben
        </Button>
      </Modal.Footer>
    </Modal>
  );
};