import axios from "axios";

// Отримуємо базову адресу з .env або використовуємо localhost за замовчуванням
const API_URL = import.meta.env.VITE_API_URL || "http://localhost:8080";

// Створюємо клієнт axios із базовим URL і підтримкою cookies (наприклад, auth_token)
export const api = axios.create({
  baseURL: `${API_URL}/api`, // API з префіксом /api, згідно з контролером
  withCredentials: true,     // дозволяє автоматично надсилати куки
});