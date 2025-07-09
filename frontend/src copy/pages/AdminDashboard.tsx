import { useNavigate } from "react-router-dom";
import { Container, Typography, Button, Stack, Paper } from "@mui/material";

export const AdminDashboard = () => {
  const navigate = useNavigate();

  return (
    <Container maxWidth="sm" sx={{ mt: 4 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Typography variant="h4" align="center" gutterBottom>
          Панель адміністратора
        </Typography>

        <Stack spacing={2} mt={3}>
          <Button variant="contained" fullWidth onClick={() => navigate("/admin/users")}>
            Користувачі
          </Button>
          <Button variant="contained" fullWidth onClick={() => navigate("/admin/films")}>
            Фільми
          </Button>
          <Button variant="contained" fullWidth onClick={() => navigate("/admin/discounts")}>
            Знижки
          </Button>
          <Button variant="contained" fullWidth onClick={() => navigate("/admin/sessions")}>
            Сеанси
          </Button>
          <Button variant="contained" fullWidth onClick={() => navigate("/admin/sales")}>
            Продажі
          </Button>
          <Button variant="contained" fullWidth onClick={() => navigate("/admin/halls")}>
            Зали
          </Button>
          <Button variant="contained" fullWidth onClick={() => navigate("/admin/tickets")}>
            Квитки
          </Button>
        </Stack>
      </Paper>
    </Container>
  );
};
