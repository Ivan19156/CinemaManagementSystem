// src/pages/AuthChoice.tsx
import { useNavigate } from "react-router-dom"

export default function AuthChoice() {
  const navigate = useNavigate()

  return (
    <div style={{ textAlign: "center", marginTop: "10rem" }}>
      <h1>Welcome to Cinema App</h1>
      <button onClick={() => navigate("/login")} style={{ margin: "1rem", padding: "1rem 2rem" }}>
        Login
      </button>
      <button onClick={() => navigate("/register")} style={{ margin: "1rem", padding: "1rem 2rem" }}>
        Register
      </button>
    </div>
  )
}
