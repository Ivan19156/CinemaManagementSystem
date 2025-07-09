import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App"; // ✅ працює з default export
import "./index.css"; // якщо є

const root = ReactDOM.createRoot(document.getElementById("root")!);
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);

