import { useEffect } from "react";

export const useLocalStorage = (exercises, taskValues) => {
  const saveToLocalStorage = () => {
    localStorage.setItem("exercises", JSON.stringify(exercises));
    localStorage.setItem("taskValues", JSON.stringify(taskValues));
  };

  useEffect(() => {
    saveToLocalStorage();
  }, [exercises, taskValues]);

  useEffect(() => {
    window.addEventListener("beforeunload", saveToLocalStorage);
    return () => {
      window.removeEventListener("beforeunload", saveToLocalStorage);
    };
  }, [exercises, taskValues]);

  return { saveToLocalStorage };
};
