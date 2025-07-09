import React from "react";
import { useNavigate } from "react-router-dom";
import { Container, Typography, Button, Box } from "@mui/material";

export const AuthChoicePage = () => {
  const navigate = useNavigate();

  return (
    <Container maxWidth="sm" sx={{ textAlign: "center", mt: 20 }}>
      <Typography variant="h3" gutterBottom>
        Welcome to Cinema App
      </Typography>
      <Typography variant="h6" gutterBottom>
        Please choose an option:
      </Typography>

      <Box sx={{ mt: 4 }}>
        <Button
          variant="contained"
          color="primary"
          onClick={() => navigate("/login")}
          sx={{ m: 1, px: 4, py: 1.5, fontSize: "16px" }}
        >
          Login
        </Button>
        <Button
          variant="outlined"
          color="primary"
          onClick={() => navigate("/register")}
          sx={{ m: 1, px: 4, py: 1.5, fontSize: "16px" }}
        >
          Register
        </Button>
      </Box>
    </Container>
  );
};
