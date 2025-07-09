import { useState } from "react";
import { api } from "../api/axios";
import {
  Container,
  Typography,
  Button,
  TextField,
  List,
  ListItem,
  ListItemText,
  Box,
  IconButton,
  Stack,
} from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import {UserDto} from "../types/user.types.ts";

export const AdminUsersPage = () => {
  const [users, setUsers] = useState<UserDto[]>([]);
  const [userId, setUserId] = useState("");
  const [hoveredId, setHoveredId] = useState<number | null>(null);
  const [editUserId, setEditUserId] = useState<number | null>(null);
  const [editName, setEditName] = useState("");
  const [editEmail, setEditEmail] = useState("");

  const fetchAllUsers = async () => {
    const res = await api.get<UserDto[]>("/users");
    setUsers(res.data);
  };

  const fetchUserById = async () => {
    if (!userId) return;
    const res = await api.get<UserDto>(`/users/${userId}`);
    setUsers([res.data]);
  };

  const deleteUser = async (id: number) => {
    await api.delete(`/users/${id}`);
    setUsers(users.filter((u) => u.id !== id));
  };

  const startEdit = (user: UserDto) => {
    setEditUserId(user.id);
    setEditName(user.name);
    setEditEmail(user.email);
  };

  const submitEdit = async () => {
    if (!editUserId) return;

    await api.put(`/users/${editUserId}`, {
      name: editName,
      email: editEmail,
    });

    setUsers((prev) =>
      prev.map((u) =>
        u.id === editUserId ? { ...u, name: editName, email: editEmail } : u
      )
    );

    setEditUserId(null);
    setEditName("");
    setEditEmail("");
  };

  return (
    <Container sx={{ py: 4 }}>
      <Typography variant="h4" gutterBottom>
        Управління користувачами
      </Typography>

      <Stack direction="row" spacing={2} alignItems="center" mb={3}>
        <Button variant="contained" onClick={fetchAllUsers}>
          Отримати всіх
        </Button>

        <TextField
          label="ID користувача"
          variant="outlined"
          size="small"
          value={userId}
          onChange={(e) => setUserId(e.target.value)}
          sx={{ width: 150 }}
        />
        <Button variant="contained" onClick={fetchUserById}>
          Отримати за ID
        </Button>
      </Stack>

      {users.length === 0 && (
        <Typography variant="body1">Немає користувачів для показу</Typography>
      )}

      <List>
        {users.map((user) => (
          <ListItem
            key={user.id}
            onMouseEnter={() => setHoveredId(user.id)}
            onMouseLeave={() => setHoveredId(null)}
            sx={{
              bgcolor: "#f4f4f4",
              borderRadius: 2,
              mb: 1,
              position: "relative",
              "&:hover": { bgcolor: "#e0e0e0" },
            }}
            secondaryAction={
              hoveredId === user.id && editUserId !== user.id ? (
                <>
                  <IconButton
                    edge="end"
                    aria-label="edit"
                    onClick={() => startEdit(user)}
                  >
                    <EditIcon />
                  </IconButton>
                  <IconButton
                    edge="end"
                    aria-label="delete"
                    onClick={() => deleteUser(user.id)}
                    sx={{ color: "red" }}
                  >
                    <DeleteIcon />
                  </IconButton>
                </>
              ) : null
            }
          >
            <ListItemText
              primary={`${user.id} — ${user.name}`}
              secondary={user.email}
            />

            {/* Форма оновлення */}
            {editUserId === user.id && (
              <Box
                sx={{
                  display: "flex",
                  flexDirection: "row",
                  gap: 1,
                  alignItems: "center",
                  width: "100%",
                  mt: 1,
                  position: "absolute",
                  bottom: -55,
                  left: 0,
                }}
              >
                <TextField
                  label="Нове ім’я"
                  size="small"
                  value={editName}
                  onChange={(e) => setEditName(e.target.value)}
                  sx={{ flexGrow: 1 }}
                />
                <TextField
                  label="Новий email"
                  size="small"
                  value={editEmail}
                  onChange={(e) => setEditEmail(e.target.value)}
                  sx={{ flexGrow: 1 }}
                />
                <Button variant="contained" onClick={submitEdit}>
                  Зберегти
                </Button>
              </Box>
            )}
          </ListItem>
        ))}
      </List>
    </Container>
  );
};
