import axios from "axios";
import { BASE_URL } from "../../config";

export const saltRequest = async (loginName) => {
  try {
    const response = await axios.post(
      `${BASE_URL}/erettsegizzunk/Login/SaltRequest`,
      JSON.stringify(loginName),
      { headers: { "Content-Type": "application/json" } }
    );
    return response.data;
  } catch (error) {
    throw new Error(
      error.response?.data?.message || "Hiba történt a salt lekérése során."
    );
  }
};