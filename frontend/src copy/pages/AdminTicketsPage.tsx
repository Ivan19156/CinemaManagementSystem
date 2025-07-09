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
  InputLabel,
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
import {TicketDto,  CreateTicketDto, UpdateTicketDto} from "../types/ticket.types.ts";

export const AdminTicketsPage = () => {
  const [tickets, setTickets] = useState<TicketDto[]>([]);
  const [searchId, setSearchId] = useState("");
  const [newTicket, setNewTicket] = useState<CreateTicketDto>({
    sessionId: 0,
    userId: 0,
    saleId: 0,
    seatNumber: "",
    price: 0,
    time: "",
    movie: "",
    hall: "",
    email: "",
  });
  const [editForm, setEditForm] = useState<{ id: number } & UpdateTicketDto | null>(null);
  const [editDialogOpen, setEditDialogOpen] = useState(false);

  const fetchAll = async () => {
    try {
      const res = await api.get<TicketDto[]>("/tickets");
      setTickets(res.data);
    } catch {
      toast.error("Помилка отримання квитків");
    }
  };

  const fetchById = async () => {
    if (!searchId) return toast.warn("Введіть ID");
    try {
      const res = await api.get<TicketDto>(`/tickets/${searchId}`);
      setTickets([res.data]);
    } catch {
      toast.error("Квиток не знайдено");
    }
  };

  const createTicket = async () => {
    try {
      await api.post("/tickets", newTicket);
      toast.success("Квиток створено");
      setNewTicket({
        sessionId: 0,
        userId: 0,
        saleId: 0,
        seatNumber: "",
        price: 0,
        time: "",
        movie: "",
        hall: "",
        email: "",
      });
      fetchAll();
    } catch {
      toast.error("Помилка створення квитка");
    }
  };

  const deleteTicket = async (id: number) => {
    try {
      await api.delete(`/tickets/${id}`);
      toast.success("Квиток видалено");
      fetchAll();
    } catch {
      toast.error("Помилка видалення");
    }
  };

  const openEditDialog = (ticket: TicketDto) => {
    setEditForm({
      id: ticket.id,
      seatNumber: ticket.seatNumber,
      price: ticket.price,
    });
    setEditDialogOpen(true);
  };

  const handleUpdate = async () => {
    if (!editForm) return;
    try {
      const { id, ...dto } = editForm;
      await api.put(`/tickets/${id}`, dto);
      toast.success("Квиток оновлено");
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
        Управління квитками
      </Typography>

      {/* Пошук */}
      <Stack direction="row" spacing={2} mb={4}>
        <TextField
          label="ID квитка"
          variant="outlined"
          value={searchId}
          onChange={(e) => setSearchId(e.target.value)}
          size="small"
        />
        <Button variant="contained" onClick={fetchById}>
          Отримати за ID
        </Button>
        <Button variant="outlined" onClick={fetchAll}>
          Отримати всі
        </Button>
      </Stack>

      {/* Форма створення нового квитка */}
      <Box mb={4} maxWidth={400}>
        <Typography variant="h6" gutterBottom>
          Створити новий квиток
        </Typography>

        <Stack spacing={2}>
          <TextField
            label="ID сеансу"
            type="number"
            value={newTicket.sessionId}
            onChange={(e) =>
              setNewTicket({ ...newTicket, sessionId: Number(e.target.value) })
            }
          />
          <TextField
            label="ID користувача"
            type="number"
            value={newTicket.userId}
            onChange={(e) =>
              setNewTicket({ ...newTicket, userId: Number(e.target.value) })
            }
          />
          <TextField
            label="ID знижки (опційно)"
            type="number"
            value={newTicket.saleId}
            onChange={(e) =>
              setNewTicket({ ...newTicket, saleId: Number(e.target.value) })
            }
          />
          <TextField
            label="Номер місця"
            value={newTicket.seatNumber}
            onChange={(e) =>
              setNewTicket({ ...newTicket, seatNumber: e.target.value })
            }
          />
          <TextField
            label="Ціна"
            type="number"
            value={newTicket.price}
            onChange={(e) =>
              setNewTicket({ ...newTicket, price: Number(e.target.value) })
            }
          />
          <TextField
            label="Час"
            type="datetime-local"
            value={newTicket.time}
            onChange={(e) =>
              setNewTicket({ ...newTicket, time: e.target.value })
            }
            InputLabelProps={{ shrink: true }}
          />
          <TextField
            label="Назва фільму"
            value={newTicket.movie}
            onChange={(e) =>
              setNewTicket({ ...newTicket, movie: e.target.value })
            }
          />
          <TextField
            label="Назва залу"
            value={newTicket.hall}
            onChange={(e) =>
              setNewTicket({ ...newTicket, hall: e.target.value })
            }
          />
          <TextField
            label="Email"
            type="email"
            value={newTicket.email}
            onChange={(e) =>
              setNewTicket({ ...newTicket, email: e.target.value })
            }
          />

          <Button variant="contained" onClick={createTicket}>
            Створити
          </Button>
        </Stack>
      </Box>

      {/* Список квитків */}
      <Typography variant="h5" gutterBottom>
        Список квитків
      </Typography>
      <List>
        {tickets.map((t) => (
          <ListItem
            key={t.id}
            secondaryAction={
              <>
                <IconButton edge="end" aria-label="edit" onClick={() => openEditDialog(t)}>
                  <EditIcon />
                </IconButton>
                <IconButton edge="end" aria-label="delete" onClick={() => deleteTicket(t.id)}>
                  <DeleteIcon />
                </IconButton>
              </>
            }
            divider
          >
            <ListItemText
              primary={`Квиток #${t.id} — Місце: ${t.seatNumber} — Ціна: ${t.price} грн`}
              secondary={
                <>
                  Користувач ID: {t.userId} | Сеанс ID: {t.sessionId} <br />
                  Куплено: {new Date(t.purchaseDate).toLocaleString()} <br />
                  Скасовано: {t.isCanceled ? "Так" : "Ні"}
                </>
              }
            />
          </ListItem>
        ))}
      </List>

      {/* Діалог оновлення */}
      <Dialog open={editDialogOpen} onClose={() => setEditDialogOpen(false)}>
        <DialogTitle>Оновити квиток</DialogTitle>
        <DialogContent>
          <Stack spacing={2} sx={{ mt: 1, minWidth: 300 }}>
            <TextField
              label="Номер місця"
              value={editForm?.seatNumber || ""}
              onChange={(e) =>
                setEditForm((prev) => prev && { ...prev, seatNumber: e.target.value })
              }
            />
            <TextField
              label="Ціна"
              type="number"
              value={editForm?.price || 0}
              onChange={(e) =>
                setEditForm((prev) =>
                  prev ? { ...prev, price: Number(e.target.value) } : null
                )
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
