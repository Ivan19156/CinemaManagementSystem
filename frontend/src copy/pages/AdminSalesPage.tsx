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
import { SaleDto, CreateSaleDto, UpdateSaleDto} from "../types/sale.types.ts";


export const AdminSalesPage = () => {
  const [sales, setSales] = useState<SaleDto[]>([]);
  const [searchId, setSearchId] = useState("");
  const [editForm, setEditForm] = useState<UpdateSaleDto | null>(null);
  const [editDialogOpen, setEditDialogOpen] = useState(false);

  const [newSale, setNewSale] = useState<CreateSaleDto>({
    userId: 0,
    ticketId: 0,
    amount: 0,
    ticketsCount: 0,
    totalPrice: 0,
    saleDate: "",
  });

  const fetchAll = async () => {
    try {
      const res = await api.get<SaleDto[]>("/sales");
      setSales(res.data);
    } catch {
      toast.error("Помилка отримання продажів");
    }
  };

  const fetchById = async () => {
    if (!searchId) return toast.warn("Введіть ID");
    try {
      const res = await api.get<SaleDto>(`/sales/${searchId}`);
      setSales([res.data]);
    } catch {
      toast.error("Продаж не знайдено");
    }
  };

  const createSale = async () => {
    try {
      await api.post("/sales", newSale);
      toast.success("Продаж створено");
      setNewSale({
        userId: 0,
        ticketId: 0,
        amount: 0,
        ticketsCount: 0,
        totalPrice: 0,
        saleDate: "",
      });
      fetchAll();
    } catch {
      toast.error("Помилка створення продажу");
    }
  };

  const deleteSale = async (id: number) => {
    try {
      await api.delete(`/sales/${id}`);
      toast.success("Продаж видалено");
      fetchAll();
    } catch {
      toast.error("Помилка видалення");
    }
  };

  const openEditDialog = (sale: UpdateSaleDto) => {
    setEditForm(sale);
    setEditDialogOpen(true);
  };

  const handleUpdate = async () => {
    if (!editForm) return;
    try {
      await api.put(`/sales/${editForm.id}`, editForm);
      toast.success("Продаж оновлено");
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
        Управління продажами
      </Typography>

      {/* Пошук за ID */}
      <Stack direction="row" spacing={2} mb={4} flexWrap="wrap" alignItems="center">
        <TextField
          label="ID продажу"
          variant="outlined"
          size="small"
          value={searchId}
          onChange={(e) => setSearchId(e.target.value)}
        />
        <Button variant="contained" onClick={fetchById}>
          Отримати за ID
        </Button>
        <Button variant="outlined" onClick={fetchAll}>
          Отримати всі
        </Button>
      </Stack>

      {/* Форма створення нового продажу */}
      <Box mb={4} maxWidth={400}>
        <Typography variant="h6" gutterBottom>
          Створити новий продаж
        </Typography>
        <Stack spacing={2}>
          <TextField
            label="ID користувача"
            type="number"
            value={newSale.userId}
            onChange={(e) => setNewSale({ ...newSale, userId: Number(e.target.value) })}
          />
          <TextField
            label="ID квитка"
            type="number"
            value={newSale.ticketId}
            onChange={(e) => setNewSale({ ...newSale, ticketId: Number(e.target.value) })}
          />
          <TextField
            label="Сума за квиток"
            type="number"
            value={newSale.amount}
            onChange={(e) => setNewSale({ ...newSale, amount: Number(e.target.value) })}
          />
          <TextField
            label="Кількість квитків"
            type="number"
            value={newSale.ticketsCount}
            onChange={(e) => setNewSale({ ...newSale, ticketsCount: Number(e.target.value) })}
          />
          <TextField
            label="Загальна сума"
            type="number"
            value={newSale.totalPrice}
            onChange={(e) => setNewSale({ ...newSale, totalPrice: Number(e.target.value) })}
          />
          <TextField
            label="Дата продажу"
            type="datetime-local"
            value={newSale.saleDate}
            onChange={(e) => setNewSale({ ...newSale, saleDate: e.target.value })}
            InputLabelProps={{ shrink: true }}
          />
          <Button variant="contained" onClick={createSale}>
            Створити
          </Button>
        </Stack>
      </Box>

      {/* Список продажів */}
      <Typography variant="h5" gutterBottom>
        Список продажів
      </Typography>
      <List>
        {sales.map((s) => (
          <ListItem
            key={s.id}
            divider
            secondaryAction={
              <>
                <IconButton edge="end" aria-label="edit" onClick={() => openEditDialog({
                  id: s.id,
                  userId: s.userId,
                  ticketsCount: s.ticketsCount,
                  totalPrice: s.totalPrice,
                  saleDate: s.saleDate,
                })}>
                  <EditIcon />
                </IconButton>
                <IconButton edge="end" aria-label="delete" onClick={() => deleteSale(s.id)}>
                  <DeleteIcon />
                </IconButton>
              </>
            }
          >
            <ListItemText
              primary={`Продаж #${s.id}`}
              secondary={
                <>
                  👤 Користувач ID: {s.userId} <br />
                  🎟️ Квиток ID: {s.ticketId} <br />
                  🧾 Кількість: {s.ticketsCount}, Сума: {s.totalPrice} грн <br />
                  📅 Дата продажу: {new Date(s.saleDate).toLocaleString()} <br />
                  🧾 TicketIds: {s.ticketIds.join(", ")}
                </>
              }
            />
          </ListItem>
        ))}
      </List>

      {/* Діалог оновлення продажу */}
      <Dialog open={editDialogOpen} onClose={() => setEditDialogOpen(false)}>
        <DialogTitle>Оновити продаж</DialogTitle>
        <DialogContent>
          <Stack spacing={2} sx={{ mt: 1, minWidth: 300 }}>
            <TextField
              label="ID користувача"
              type="number"
              value={editForm?.userId || 0}
              onChange={(e) =>
                setEditForm((prev) => (prev ? { ...prev, userId: Number(e.target.value) } : null))
              }
            />
            <TextField
              label="Кількість квитків"
              type="number"
              value={editForm?.ticketsCount || 0}
              onChange={(e) =>
                setEditForm((prev) => (prev ? { ...prev, ticketsCount: Number(e.target.value) } : null))
              }
            />
            <TextField
              label="Загальна сума"
              type="number"
              value={editForm?.totalPrice || 0}
              onChange={(e) =>
                setEditForm((prev) => (prev ? { ...prev, totalPrice: Number(e.target.value) } : null))
              }
            />
            <TextField
              label="Дата продажу"
              type="datetime-local"
              value={editForm?.saleDate || ""}
              onChange={(e) =>
                setEditForm((prev) => (prev ? { ...prev, saleDate: e.target.value } : null))
              }
              InputLabelProps={{ shrink: true }}
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
