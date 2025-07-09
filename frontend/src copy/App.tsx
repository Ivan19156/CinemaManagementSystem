import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import 'react-toastify/dist/ReactToastify.css';
import { LoginPage } from "./pages/LoginPage";
import { RegisterPage } from "./pages/RegisterPage";
import { AuthChoicePage } from "./pages/AuthChoicePage";
import { AdminDashboard } from "./pages/AdminDashboard"; // ← додаємо імпорт
import { AdminUsersPage } from "./pages/AdminUsersPage";
import { AdminFilmsPage } from "./pages/AdminFilmsPage";
import { AdminDiscountPage } from "./pages/AdminDiscountPage";
import { AdminHallsPage } from "./pages/AdminHallsPage";
import { AdminSalesPage } from "./pages/AdminSalesPage";
import { AdminTicketsPage } from "./pages/AdminTicketsPage";
import { AdminSessionsPage } from "./pages/AdminSessionPage";
import { UsersHomePage } from "./pages/UsersHomePage";

const App = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<AuthChoicePage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/admin/dashboard" element={<AdminDashboard />} /> {/* ← новий маршрут */}
        <Route path="/admin/users" element={<AdminUsersPage />} />
        <Route path="/admin/films" element={<AdminFilmsPage />} />
        <Route path="/admin/discounts" element={<AdminDiscountPage />} />
        <Route path="/admin/halls" element={<AdminHallsPage />} />
        <Route path="/admin/sessions" element={<AdminSessionsPage />} />
        <Route path="/admin/sales" element={<AdminSalesPage />} />
        <Route path="/admin/tickets" element={<AdminTicketsPage />} />
         <Route path="/users/home" element={<UsersHomePage />} />
      </Routes>
    </BrowserRouter>
  );
};

export default App;
