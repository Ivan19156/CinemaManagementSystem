import { useState, useEffect } from "react";
import { api } from "../api/axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import {
  Container,
  TextField,
  Button,
  Typography,
  Box,
  List,
  ListItem,
  Paper,
  Stack,
  Divider,
} from "@mui/material";

import {FilmDto, CreateFilmDto, UpdateFilmDto } from "../types/film.types.ts";


export const AdminFilmsPage = () => {
  const [films, setFilms] = useState<FilmDto[]>([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [genre, setGenre] = useState("");
  const [director, setDirector] = useState("");
  const [releaseDate, setReleaseDate] = useState("");
  const [filmId, setFilmId] = useState("");

  const [newFilm, setNewFilm] = useState<CreateFilmDto>({
    title: "",
    genre: "",
    director: "",
    releaseDate: "",
    description: "",
  });

  const [editingFilmId, setEditingFilmId] = useState<number | null>(null);
  const [editForm, setEditForm] = useState<UpdateFilmDto>({
    id: 0,
    title: "",
    genre: "",
    director: "",
    releaseDate: "",
    description: "",
  });

  useEffect(() => {
    fetchAll();
  }, []);

  const fetchAll = async () => {
    try {
      const res = await api.get<FilmDto[]>("/films");
      setFilms(res.data);
    } catch {
      toast.error("Помилка завантаження фільмів");
    }
  };

  const fetchById = async () => {
    if (!filmId) return toast.warn("Введіть ID фільму.");
    try {
      const res = await api.get<FilmDto>(`/films/${filmId}`);
      setFilms([res.data]);
    } catch {
      toast.error("Фільм не знайдено або помилка отримання");
    }
  };

  const handleSearch = async () => {
    if (!searchTerm) return toast.warn("Введіть пошуковий термін.");
    try {
      const res = await api.get<FilmDto[]>(`/films/search?term=${searchTerm}`);
      setFilms(res.data);
    } catch {
      toast.error("Помилка пошуку фільмів");
    }
  };

  const handleGenre = async () => {
    if (!genre) return toast.warn("Введіть жанр.");
    try {
      const res = await api.get<FilmDto[]>(`/films/genre/${genre}`);
      setFilms(res.data);
    } catch {
      toast.error("Помилка пошуку за жанром");
    }
  };

  const handleDirector = async () => {
    if (!director) return toast.warn("Введіть режисера.");
    try {
      const res = await api.get<FilmDto[]>(`/films/director/${director}`);
      setFilms(res.data);
    } catch {
      toast.error("Помилка пошуку за режисером");
    }
  };

  const handleReleaseDate = async () => {
    if (!releaseDate) return toast.warn("Оберіть дату релізу.");
    try {
      const res = await api.get<FilmDto[]>(`/films/release-date?date=${releaseDate}`);
      setFilms(res.data);
    } catch {
      toast.error("Помилка пошуку за датою релізу");
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm("Ви впевнені, що хочете видалити цей фільм?")) return;
    try {
      await api.delete(`/films/${id}`);
      toast.success("Фільм видалено");
      fetchAll();
    } catch {
      toast.error("Помилка видалення фільму");
    }
  };

  const handleCreate = async () => {
    try {
      await api.post("/films", newFilm);
      toast.success("Фільм створено");
      setNewFilm({ title: "", genre: "", director: "", releaseDate: "", description: "" });
      fetchAll();
    } catch {
      toast.error("Помилка створення фільму");
    }
  };

  const saveEdit = async () => {
    try {
      await api.put(`/films/${editForm.id}`, editForm);
      toast.success("Фільм оновлено");
      setEditingFilmId(null);
      fetchAll();
    } catch {
      toast.error("Помилка оновлення фільму");
    }
  };

  return (
    <Container>
      <ToastContainer />
      <Typography variant="h4" gutterBottom>Управління фільмами</Typography>

      <Box my={2}>
        <Typography variant="h6">Пошук</Typography>
        <Stack direction="row" spacing={2}>
          <TextField label="Назва" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
          <Button variant="contained" onClick={handleSearch}>Пошук</Button>
          <TextField label="Жанр" value={genre} onChange={(e) => setGenre(e.target.value)} />
          <Button variant="contained" onClick={handleGenre}>За жанром</Button>
          <TextField label="Режисер" value={director} onChange={(e) => setDirector(e.target.value)} />
          <Button variant="contained" onClick={handleDirector}>За режисером</Button>
          <TextField type="date" value={releaseDate} onChange={(e) => setReleaseDate(e.target.value)} />
          <Button variant="contained" onClick={handleReleaseDate}>За датою</Button>
          <TextField label="ID" value={filmId} onChange={(e) => setFilmId(e.target.value)} />
          <Button variant="contained" onClick={fetchById}>За ID</Button>
          <Button variant="outlined" onClick={fetchAll}>Усі фільми</Button>
        </Stack>
      </Box>

      <Box my={4} maxWidth={400}>
        <Typography variant="h6">Створити новий фільм</Typography>
        <Stack spacing={2}>
          <TextField label="Назва" value={newFilm.title} onChange={(e) => setNewFilm({ ...newFilm, title: e.target.value })} />
          <TextField label="Жанр" value={newFilm.genre} onChange={(e) => setNewFilm({ ...newFilm, genre: e.target.value })} />
          <TextField label="Режисер" value={newFilm.director} onChange={(e) => setNewFilm({ ...newFilm, director: e.target.value })} />
          <TextField type="date" label="Дата релізу" value={newFilm.releaseDate} onChange={(e) => setNewFilm({ ...newFilm, releaseDate: e.target.value })} InputLabelProps={{ shrink: true }} />
          <TextField label="Опис" value={newFilm.description} onChange={(e) => setNewFilm({ ...newFilm, description: e.target.value })} multiline rows={3} />
          <Button variant="contained" onClick={handleCreate}>Створити</Button>
        </Stack>
      </Box>

      <Typography variant="h6">Список фільмів</Typography>
      <List>
        {films.map(film => (
          <ListItem key={film.id}>
            <Paper elevation={3} style={{ padding: "1rem", width: "100%" }}>
              {editingFilmId === film.id ? (
                <Stack spacing={2}>
                  <TextField label="Назва" value={editForm.title} onChange={(e) => setEditForm({ ...editForm, title: e.target.value })} />
                  <TextField label="Жанр" value={editForm.genre} onChange={(e) => setEditForm({ ...editForm, genre: e.target.value })} />
                  <TextField label="Режисер" value={editForm.director} onChange={(e) => setEditForm({ ...editForm, director: e.target.value })} />
                  <TextField type="date" label="Дата" value={editForm.releaseDate.split("T")[0]} onChange={(e) => setEditForm({ ...editForm, releaseDate: e.target.value })} InputLabelProps={{ shrink: true }} />
                  <TextField label="Опис" value={editForm.description} onChange={(e) => setEditForm({ ...editForm, description: e.target.value })} multiline rows={3} />
                  <Stack direction="row" spacing={2}>
                    <Button variant="contained" onClick={saveEdit}>Зберегти</Button>
                    <Button variant="outlined" onClick={() => setEditingFilmId(null)}>Скасувати</Button>
                  </Stack>
                </Stack>
              ) : (
                <Box>
                  <Typography variant="h6">{film.title}</Typography>
                  <Typography>Жанр: {film.genre}</Typography>
                  <Typography>Режисер: {film.director}</Typography>
                  <Typography>Реліз: {new Date(film.releaseDate).toLocaleDateString()}</Typography>
                  <Typography>{film.description}</Typography>
                  <Stack direction="row" spacing={2} mt={1}>
                    <Button variant="outlined" onClick={() => { setEditingFilmId(film.id); setEditForm(film); }}>Редагувати</Button>
                    <Button variant="contained" color="error" onClick={() => handleDelete(film.id)}>Видалити</Button>
                  </Stack>
                </Box>
              )}
            </Paper>
          </ListItem>
        ))}
      </List>
    </Container>
  );
};