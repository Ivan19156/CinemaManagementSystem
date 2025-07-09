import { useNavigate } from "react-router-dom";
import { api } from "../api/axios";
import { useState } from "react";
import {
  Container,
  TextField,
  Button,
  Typography,
  CircularProgress,
  Box,
  Alert,
  Paper,
} from "@mui/material";

interface LoginForm {
  email: string;
  password: string;
}

interface UserMeResponse {
  role: string;
}

export const LoginPage = () => {
  const navigate = useNavigate();
  const [form, setForm] = useState<LoginForm>({
    email: "",
    password: "",
  });

  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleLogin = async () => {
    setError(null);
    setLoading(true);
    try {
      await api.post("/users/login", form);
      const res = await api.get<UserMeResponse>("/users/me");
      const role = res.data.role;

      if (role && role.toLowerCase() === "admin") {
        navigate("/admin/dashboard");
      } else {
        navigate("/users/home");
      }
    } catch (err) {
      setError("Логін не вдався або сесія недоступна");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container maxWidth="xs" sx={{ mt: 8 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Typography variant="h5" gutterBottom>
          Увійти
        </Typography>

        <TextField
          fullWidth
          type="email"
          label="Email"
          margin="normal"
          value={form.email}
          onChange={(e) => setForm({ ...form, email: e.target.value })}
        />

        <TextField
          fullWidth
          type="password"
          label="Пароль"
          margin="normal"
          value={form.password}
          onChange={(e) => setForm({ ...form, password: e.target.value })}
        />

        {error && (
          <Alert severity="error" sx={{ mt: 2 }}>
            {error}
          </Alert>
        )}

        <Box sx={{ mt: 3, textAlign: "center" }}>
          <Button
            variant="contained"
            color="primary"
            onClick={handleLogin}
            disabled={loading}
            fullWidth
          >
            {loading ? <CircularProgress size={24} /> : "Увійти"}
          </Button>
        </Box>
      </Paper>
    </Container>
  );
};

