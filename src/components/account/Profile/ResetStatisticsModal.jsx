import React from "react";
import { Modal, Button } from "react-bootstrap";

export const ResetStatisticsModal = ({ show, onHide, resetStatistics }) => {
  return (
    <Modal show={show} onHide={onHide} centered>
      <Modal.Header closeButton>
        <Modal.Title>Statisztika visszaállítása</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        Biztosan vissza szeretnéd állítani a statisztikádat? Ez a művelet nem
        vonható vissza.
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>
          Mégse
        </Button>
        <Button variant="danger" onClick={resetStatistics}>
          Visszaállítás
        </Button>
      </Modal.Footer>
    </Modal>
  );
};