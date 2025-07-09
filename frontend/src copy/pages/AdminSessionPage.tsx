import React, { useEffect, useState } from "react";
import {
  Box,
  Button,
  Container,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  IconButton,
  List,
  ListItem,
  ListItemText,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import { toast, ToastContainer } from "react-toastify";
import { api } from "../api/axios";
import {SessionDto, CreateSessionDto } from "../types/session.types.ts";

export const AdminSessionsPage = () => {
  const [sessions, setSessions] = useState<SessionDto[]>([]);
  const [searchId, setSearchId] = useState("");
  const [searchFilmId, setSearchFilmId] = useState("");
  const [editForm, setEditForm] = useState<SessionDto | null>(null);
  const [editDialogOpen, setEditDialogOpen] = useState(false);
  const [newSession, setNewSession] = useState<CreateSessionDto>({
    filmId: 0,
    hallId: 0,
    startTime: "",
    ticketPrice: 0,
  });

  const fetchAll = async () => {
    try {
      const res = await api.get<SessionDto[]>("/session");
      setSessions(res.data);
    } catch {
      toast.error("Помилка отримання сеансів");
    }
  };

  const fetchById = async () => {
    if (!searchId) return toast.warn("Введіть ID");
    try {
      const res = await api.get<SessionDto>(`/session/${searchId}`);
      setSessions([res.data]);
    } catch {
      toast.error("Сеанс не знайдено");
    }
  };

  const fetchByFilmId = async () => {
    if (!searchFilmId) return toast.warn("Введіть Film ID");
    try {
      const res = await api.get<SessionDto[]>(`/session/film/${searchFilmId}`);
      setSessions(res.data);
    } catch {
      toast.error("Сеанси не знайдено");
    }
  };

  const createSession = async () => {
    try {
      await api.post("/session", newSession);
      toast.success("Сеанс створено");
      setNewSession({ filmId: 0, hallId: 0, startTime: "", ticketPrice: 0 });
      fetchAll();
    } catch {
      toast.error("Помилка створення сеансу");
    }
  };

  const deleteSession = async (id: number) => {
    try {
      await api.delete(`/session/${id}`);
      toast.success("Сеанс видалено");
      fetchAll();
    } catch {
      toast.error("Помилка видалення");
    }
  };

  const openEditDialog = (session: SessionDto) => {
    setEditForm(session);
    setEditDialogOpen(true);
  };

  const handleUpdate = async () => {
    if (!editForm) return;
    try {
      const { id, ...dto } = editForm;
      await api.put(`/session/${id}`, dto);
      toast.success("Сеанс оновлено");
      setEditDialogOpen(false);
      setEditForm(null);
      fetchAll();
    } catch {
      toast.error("Помилка оновлення");
    }
  };

  useEffect(() => {
    fetchAll();
  }, []);

  return (
    <Container sx={{ py: 4 }}>
      <ToastContainer />
      <Typography variant="h4" gutterBottom>
        Управління сеансами
      </Typography>

      {/* Пошук */}
      <Stack direction="row" spacing={2} mb={4} flexWrap="wrap" alignItems="center">
        <TextField
          label="ID сеансу"
          variant="outlined"
          value={searchId}
          onChange={(e) => setSearchId(e.target.value)}
          size="small"
        />
        <Button variant="contained" onClick={fetchById}>
          Отримати за ID
        </Button>

        <TextField
          label="Film ID"
          variant="outlined"
          value={searchFilmId}
          onChange={(e) => setSearchFilmId(e.target.value)}
          size="small"
        />
        <Button variant="contained" onClick={fetchByFilmId}>
          Отримати за FilmId
        </Button>

        <Button variant="outlined" onClick={fetchAll}>
          Отримати всі
        </Button>
      </Stack>

      {/* Форма створення нового сеансу */}
      <Box mb={4} maxWidth={400}>
        <Typography variant="h6" gutterBottom>
          Створити новий сеанс
        </Typography>

        <Stack spacing={2}>
          <TextField
            label="ID фільму"
            type="number"
            value={newSession.filmId}
            onChange={(e) => setNewSession({ ...newSession, filmId: Number(e.target.value) })}
          />
          <TextField
            label="ID залу"
            type="number"
            value={newSession.hallId}
            onChange={(e) => setNewSession({ ...newSession, hallId: Number(e.target.value) })}
          />
          <TextField
            label="Час початку"
            type="datetime-local"
            value={newSession.startTime}
            onChange={(e) => setNewSession({ ...newSession, startTime: e.target.value })}
            InputLabelProps={{ shrink: true }}
          />
          <TextField
            label="Ціна квитка"
            type="number"
            value={newSession.ticketPrice}
            onChange={(e) => setNewSession({ ...newSession, ticketPrice: Number(e.target.value) })}
          />

          <Button variant="contained" onClick={createSession}>
            Створити
          </Button>
        </Stack>
      </Box>

      {/* Список сеансів */}
      <Typography variant="h5" gutterBottom>
        Список сеансів
      </Typography>
      <List>
        {sessions.map((s) => (
          <ListItem
            key={s.id}
            secondaryAction={
              <>
                <IconButton edge="end" aria-label="edit" onClick={() => openEditDialog(s)}>
                  <EditIcon />
                </IconButton>
                <IconButton edge="end" aria-label="delete" onClick={() => deleteSession(s.id)}>
                  <DeleteIcon />
                </IconButton>
              </>
            }
            divider
          >
            <ListItemText
              primary={`Сеанс #${s.id}`}
              secondary={
                <>
                  🎬 Фільм ID: {s.filmId} <br />
                  🏛️ Зал ID: {s.hallId} <br />
                  🕒 Початок: {new Date(s.startTime).toLocaleString()} <br />
                  🎟️ Ціна: {s.ticketPrice} грн
                </>
              }
            />
          </ListItem>
        ))}
      </List>

      {/* Діалог оновлення */}
      <Dialog open={editDialogOpen} onClose={() => setEditDialogOpen(false)}>
        <DialogTitle>Оновити сеанс</DialogTitle>
        <DialogContent>
          <Stack spacing={2} sx={{ mt: 1, minWidth: 300 }}>
            <TextField
              label="ID фільму"
              type="number"
              value={editForm?.filmId || 0}
              onChange={(e) =>
                setEditForm((prev) => (prev ? { ...prev, filmId: Number(e.target.value) } : null))
              }
            />
            <TextField
              label="ID залу"
              type="number"
              value={editForm?.hallId || 0}
              onChange={(e) =>
                setEditForm((prev) => (prev ? { ...prev, hallId: Number(e.target.value) } : null))
              }
            />
            <TextField
              label="Час початку"
              type="datetime-local"
              value={editForm?.startTime || ""}
              onChange={(e) =>
                setEditForm((prev) => (prev ? { ...prev, startTime: e.target.value } : null))
              }
              InputLabelProps={{ shrink: true }}
            />
            <TextField
              label="Ціна квитка"
              type="number"
              value={editForm?.ticketPrice || 0}
              onChange={(e) =>
                setEditForm((prev) => (prev ? { ...prev, ticketPrice: Number(e.target.value) } : null))
              }
            />
          </Stack>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setEditDialogOpen(false)}>Скасувати</Button>
          <Button variant="contained" onClick={handleUpdate}>
            Оновити
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};
