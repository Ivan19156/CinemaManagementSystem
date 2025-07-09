import { useState, useEffect } from "react";
import {
  Button,
  Container,
  TextField,
  Typography,
  Paper,
  Box,
  List,
  ListItem,
  ListItemText,
  IconButton,
  Tooltip,
  Collapse,
} from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import AssignmentIndIcon from "@mui/icons-material/AssignmentInd";
import DoneIcon from "@mui/icons-material/Done";
import CancelIcon from "@mui/icons-material/Cancel";
import { api } from "../api/axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import {DiscountDto, EditForm} from "../types/dicount.types.ts";


export const AdminDiscountPage = () => {
  const [discounts, setDiscounts] = useState<DiscountDto[]>([]);
  const [searchId, setSearchId] = useState("");
  const [newDiscount, setNewDiscount] = useState({
    name: "",
    description: "",
    percentage: "",
    startDate: "",
    endDate: "",
  });
  const [editForm, setEditForm] = useState<EditForm | null>(null);
  const [assignUserId, setAssignUserId] = useState("");
  const [assigningDiscountId, setAssigningDiscountId] = useState<number | null>(null);

  useEffect(() => {
    fetchAll();
  }, []);

  const fetchAll = async () => {
    try {
      const res = await api.get<DiscountDto[]>("/discounts");
      setDiscounts(res.data);
    } catch {
      toast.error("Помилка завантаження знижок");
    }
  };

  const handleCreate = async () => {
    const { name, description, percentage, startDate, endDate } = newDiscount;
    if (!name || !percentage || !startDate || !endDate) {
      toast.warn("Заповніть усі поля");
      return;
    }
    try {
      await api.post("/discounts", {
        name,
        description,
        percentage: parseFloat(percentage),
        startDate,
        endDate,
      });
      toast.success("Знижку створено");
      setNewDiscount({ name: "", description: "", percentage: "", startDate: "", endDate: "" });
      fetchAll();
    } catch {
      toast.error("Помилка створення");
    }
  };

  const handleUpdate = async () => {
    if (!editForm) return;
    const { id, name, percentage, startDate, endDate, description } = editForm;
    try {
      await api.put(`/discounts/${id}`, {
        id,
        name,
        percentage: parseFloat(percentage),
        startDate,
        endDate,
        description,
      });
      toast.success("Знижку оновлено");
      setEditForm(null);
      fetchAll();
    } catch {
      toast.error("Помилка оновлення");
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await api.delete(`/discounts/${id}`);
      toast.success("Знижку видалено");
      fetchAll();
    } catch {
      toast.error("Помилка видалення");
    }
  };

  const handleAssignToUser = async () => {
    if (!assignUserId || !assigningDiscountId) {
      toast.warn("Введіть ID користувача");
      return;
    }
    try {
      await api.post(`/discounts/${assigningDiscountId}/assign/${assignUserId}`);
      toast.success("Знижку призначено");
      setAssignUserId("");
      setAssigningDiscountId(null);
      fetchAll();
    } catch {
      toast.error("Помилка призначення");
    }
  };

  return (
    <Container>
      <ToastContainer position="top-right" autoClose={3000} />
      <Typography variant="h4" gutterBottom>Управління знижками</Typography>

      <Box mb={4}>
        <Typography variant="h6">Створити знижку</Typography>
        <Paper sx={{ p: 2, mb: 2 }}>
          <TextField fullWidth label="Назва" value={newDiscount.name} onChange={(e) => setNewDiscount({ ...newDiscount, name: e.target.value })} sx={{ mb: 2 }} />
          <TextField fullWidth label="Опис" value={newDiscount.description} onChange={(e) => setNewDiscount({ ...newDiscount, description: e.target.value })} sx={{ mb: 2 }} multiline rows={2} />
          <TextField fullWidth label="Відсоток" type="number" value={newDiscount.percentage} onChange={(e) => setNewDiscount({ ...newDiscount, percentage: e.target.value })} sx={{ mb: 2 }} />
          <TextField fullWidth type="date" label="Початок" InputLabelProps={{ shrink: true }} value={newDiscount.startDate} onChange={(e) => setNewDiscount({ ...newDiscount, startDate: e.target.value })} sx={{ mb: 2 }} />
          <TextField fullWidth type="date" label="Кінець" InputLabelProps={{ shrink: true }} value={newDiscount.endDate} onChange={(e) => setNewDiscount({ ...newDiscount, endDate: e.target.value })} sx={{ mb: 2 }} />
          <Button variant="contained" color="primary" onClick={handleCreate}>Створити</Button>
        </Paper>
      </Box>

      {editForm && (
        <Box mb={4}>
          <Typography variant="h6">Редагувати знижку</Typography>
          <Paper sx={{ p: 2, mb: 2 }}>
            <TextField fullWidth label="Назва" value={editForm.name} onChange={(e) => setEditForm({ ...editForm, name: e.target.value })} sx={{ mb: 2 }} />
            <TextField fullWidth label="Опис" value={editForm.description} onChange={(e) => setEditForm({ ...editForm, description: e.target.value })} sx={{ mb: 2 }} multiline rows={2} />
            <TextField fullWidth label="Відсоток" type="number" value={editForm.percentage} onChange={(e) => setEditForm({ ...editForm, percentage: e.target.value })} sx={{ mb: 2 }} />
            <TextField fullWidth type="date" label="Початок" InputLabelProps={{ shrink: true }} value={editForm.startDate} onChange={(e) => setEditForm({ ...editForm, startDate: e.target.value })} sx={{ mb: 2 }} />
            <TextField fullWidth type="date" label="Кінець" InputLabelProps={{ shrink: true }} value={editForm.endDate} onChange={(e) => setEditForm({ ...editForm, endDate: e.target.value })} sx={{ mb: 2 }} />
            <Box display="flex" gap={2}>
              <Button variant="contained" onClick={handleUpdate}>Зберегти</Button>
              <Button variant="outlined" color="secondary" onClick={() => setEditForm(null)}>Скасувати</Button>
            </Box>
          </Paper>
        </Box>
      )}

      <Typography variant="h6">Список знижок</Typography>
      <List>
        {discounts.map((d) => (
          <ListItem key={d.id} sx={{ border: "1px solid #ddd", mb: 1, borderRadius: 1, bgcolor: d.isUsed ? "#ffe6e6" : "white" }}>
            <ListItemText primary={`${d.code} – ${d.percentage}%`} secondary={d.description} />
            <Tooltip title="Призначити користувачу">
              <IconButton onClick={() => setAssigningDiscountId(d.id)}><AssignmentIndIcon /></IconButton>
            </Tooltip>
            <Tooltip title="Редагувати">
              <IconButton onClick={() => setEditForm({
                id: d.id,
                name: d.name,
                percentage: d.percentage.toString(),
                startDate: "",
                endDate: "",
                description: d.description
              })}><EditIcon /></IconButton>
            </Tooltip>
            <Tooltip title="Видалити">
              <IconButton onClick={() => handleDelete(d.id)}><DeleteIcon /></IconButton>
            </Tooltip>
          </ListItem>
        ))}
      </List>

      {assigningDiscountId && (
        <Paper sx={{ p: 2, maxWidth: 400, mt: 4 }}>
          <Typography variant="subtitle1">Призначити знижку ID {assigningDiscountId}</Typography>
          <TextField
            fullWidth
            label="User ID"
            type="number"
            value={assignUserId}
            onChange={(e) => setAssignUserId(e.target.value)}
            sx={{ mb: 2 }}
          />
          <Box display="flex" gap={2}>
            <Button variant="contained" onClick={handleAssignToUser}>Призначити</Button>
            <Button variant="outlined" color="secondary" onClick={() => setAssigningDiscountId(null)}>Скасувати</Button>
          </Box>
        </Paper>
      )}
    </Container>
  );
};