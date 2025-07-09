import { useState } from "react";
import { register } from "../api/auth";
import { RegisterUserDto } from "../types/auth";
import {
  Container,
  TextField,
  Button,
  Typography,
  Alert,
  Paper,
  Box,
} from "@mui/material";

export const RegisterPage = () => {
  const [form, setForm] = useState<RegisterUserDto>({
    name: "",
    email: "",
    password: "",
  });

  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);

    try {
      const response = await register(form);
      console.log("Реєстрація успішна. ID:", response.data);
      setSuccess(true);
      setForm({
        name: "",
        email: "",
        password: "",
      });
    } catch (err: any) {
      setError(err.response?.data || "Помилка при реєстрації");
    }
  };

  return (
    <Container maxWidth="xs" sx={{ mt: 8 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Typography variant="h5" gutterBottom>
          Реєстрація
        </Typography>

        <Box component="form" onSubmit={handleSubmit} noValidate>
          <TextField
            fullWidth
            label="Повне ім'я"
            name="name"
            value={form.name}
            onChange={handleChange}
            margin="normal"
            required
          />

          <TextField
            fullWidth
            label="Email"
            type="email"
            name="email"
            value={form.email}
            onChange={handleChange}
            margin="normal"
            required
          />

          <TextField
            fullWidth
            label="Пароль"
            type="password"
            name="password"
            value={form.password}
            onChange={handleChange}
            margin="normal"
            required
          />

          {/* Якщо потрібне підтвердження пароля, можна додати ще один TextField тут */}

          <Button
            type="submit"
            variant="contained"
            color="primary"
            fullWidth
            sx={{ mt: 3 }}
          >
            Зареєструватися
          </Button>
        </Box>

        {error && (
          <Alert severity="error" sx={{ mt: 2 }}>
            {error}
          </Alert>
        )}
        {success && (
          <Alert severity="success" sx={{ mt: 2 }}>
            Реєстрація успішна!
          </Alert>
        )}
      </Paper>
    </Container>
  );
};
