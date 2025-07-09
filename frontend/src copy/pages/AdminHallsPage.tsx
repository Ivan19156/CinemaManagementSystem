import { useEffect, useState } from "react";
import { api } from "../api/axios";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import {HallDto, CreateHallDto} from "../types/hall.types.ts";
import {
  Box,
  Button,
  MenuItem,
  Paper,
  Select,
  Stack,
  TextField,
  Typography,
} from "@mui/material";


export const AdminHallsPage = () => {
  const [halls, setHalls] = useState<HallDto[]>([]);
  const [searchId, setSearchId] = useState("");
  const [hoveredId, setHoveredId] = useState<number | null>(null);

  const [newHall, setNewHall] = useState<CreateHallDto>({
    name: "",
    capacity: 0,
    seatsCount: 0,
  });

  const [editForm, setEditForm] = useState<HallDto | null>(null);

  const fetchAll = async () => {
    try {
      const res = await api.get<HallDto[]>("/halls");
      setHalls(res.data);
    } catch {
      toast.error("Помилка отримання залів");
    }
  };

  const fetchById = async () => {
    if (!searchId) {
      toast.warn("Введіть ID");
      return;
    }
    try {
      const res = await api.get<HallDto>(`/halls/${searchId}`);
      setHalls([res.data]);
    } catch {
      toast.error("Зал не знайдено");
    }
  };

  const createHall = async () => {
    try {
      await api.post("/halls", newHall);
      toast.success("Зал створено");
      setNewHall({ name: "", capacity: 0, seatsCount: 0 });
      fetchAll();
    } catch {
      toast.error("Помилка створення залу");
    }
  };

  const deleteHall = async (id: number) => {
    try {
      await api.delete(`/halls/${id}`);
      toast.success("Зал видалено");
      fetchAll();
    } catch {
      toast.error("Помилка видалення");
    }
  };

  const handleUpdate = async () => {
    if (!editForm) return;
    try {
      await api.put(`/halls/${editForm.id}`, editForm);
      toast.success("Зал оновлено");
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
    <Box sx={{ p: 4 }}>
      <ToastContainer />
      <Typography variant="h4" gutterBottom>
        Управління залами
      </Typography>

      <Stack direction="row" spacing={2} alignItems="center" mb={3}>
        <Button variant="contained" onClick={fetchAll}>
          Отримати всі зали
        </Button>

        <TextField
          label="ID залу"
          variant="outlined"
          size="small"
          value={searchId}
          onChange={(e) => setSearchId(e.target.value)}
        />
        <Button variant="contained" onClick={fetchById}>
          Отримати за ID
        </Button>
      </Stack>

      <Stack direction="row" alignItems="center" mb={4}>
        <Typography>Список залів:</Typography>
        <Select
          size="small"
          sx={{ ml: 1, minWidth: 200 }}
          displayEmpty
          value=""
          onChange={() => {}}
        >
          <MenuItem value="">
            <em>-- Оберіть зал --</em>
          </MenuItem>
          {halls.map((h) => (
            <MenuItem key={h.id} value={h.id}>
              {h.name}
            </MenuItem>
          ))}
        </Select>
      </Stack>

      {/* Форма створення нового залу */}
      <Paper sx={{ p: 3, mb: 4, maxWidth: 400 }}>
        <Typography variant="h6" mb={2}>
          Створити новий зал
        </Typography>

        <Stack spacing={2}>
          <TextField
            label="Назва залу"
            fullWidth
            value={newHall.name}
            onChange={(e) => setNewHall({ ...newHall, name: e.target.value })}
          />

          <TextField
            label="Місткість (вся кількість людей)"
            type="number"
            fullWidth
            value={newHall.capacity}
            onChange={(e) =>
              setNewHall({ ...newHall, capacity: Number(e.target.value) })
            }
          />

          <TextField
            label="Кількість місць"
            type="number"
            fullWidth
            value={newHall.seatsCount}
            onChange={(e) =>
              setNewHall({ ...newHall, seatsCount: Number(e.target.value) })
            }
          />

          <Button
            variant="contained"
            color="success"
            onClick={createHall}
            sx={{ alignSelf: "flex-start" }}
          >
            Створити
          </Button>
        </Stack>
      </Paper>

      {/* Форма редагування залу */}
      {editForm && (
        <Paper sx={{ p: 3, mb: 4, maxWidth: 400 }}>
          <Typography variant="h6" mb={2}>
            Оновити зал
          </Typography>

          <Stack spacing={2}>
            <TextField
              label="Назва"
              fullWidth
              value={editForm.name}
              onChange={(e) =>
                setEditForm({ ...editForm, name: e.target.value })
              }
            />

            <TextField
              label="Місткість"
              type="number"
              fullWidth
              value={editForm.capacity}
              onChange={(e) =>
                setEditForm({ ...editForm, capacity: Number(e.target.value) })
              }
            />

            <TextField
              label="Кількість місць"
              type="number"
              fullWidth
              value={editForm.seatsCount}
              onChange={(e) =>
                setEditForm({ ...editForm, seatsCount: Number(e.target.value) })
              }
            />

            <Stack direction="row" spacing={2} mt={2}>
              <Button variant="contained" onClick={handleUpdate}>
                Оновити
              </Button>
              <Button variant="outlined" onClick={() => setEditForm(null)}>
                Скасувати
              </Button>
            </Stack>
          </Stack>
        </Paper>
      )}

      <Typography variant="h5" gutterBottom>
        Список залів
      </Typography>

      <Stack spacing={2}>
        {halls.map((h) => (
          <Paper
            key={h.id}
            onMouseEnter={() => setHoveredId(h.id)}
            onMouseLeave={() => setHoveredId(null)}
            sx={{
              p: 2,
              position: "relative",
              "&:hover .actions": { display: "flex" },
            }}
          >
            <Typography variant="h6">{h.name}</Typography>
            <Typography>Місткість: {h.capacity}</Typography>
            <Typography>Місць: {h.seatsCount}</Typography>

            {hoveredId === h.id && (
              <Stack
                direction="column"
                spacing={1}
                className="actions"
                sx={{
                  position: "absolute",
                  top: 16,
                  right: 16,
                  display: "flex",
                  zIndex: 10,
                }}
              >
                <Button
                  variant="contained"
                  color="error"
                  size="small"
                  onClick={() => deleteHall(h.id)}
                >
                  Видалити
                </Button>
                <Button
                  variant="contained"
                  color="warning"
                  size="small"
                  onClick={() => setEditForm(h)}
                >
                  Оновити
                </Button>
              </Stack>
            )}
          </Paper>
        ))}
      </Stack>
    </Box>
  );
};
