import { useEffect, useState } from "react";
import { api } from "../api/axios";
import { toast, ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import {
  Box,
  Button,
  Container,
  TextField,
  Typography,
  List,
  ListItem,
  Paper,
  Dialog,
} from "@mui/material";

interface TicketDto {
  id: number;
  sessionId: number;
  saleId?: number;
  price: number;
  status: string;
}

interface FilmDto {
  id: number;
  title: string;
  genre: string;
  director: string;
  releaseDate: string;
}

export const UsersHomePage = () => {
  const [films, setFilms] = useState<FilmDto[]>([]);
  const [searchDirector, setSearchDirector] = useState("");
  const [searchTitle, setSearchTitle] = useState("");
  const [searchDate, setSearchDate] = useState("");
  const [userId, setUserId] = useState<number | null>(null);
  const [myTickets, setMyTickets] = useState<TicketDto[]>([]);

  const [hoveredFilmId, setHoveredFilmId] = useState<number | null>(null);
  const [trailerUrl, setTrailerUrl] = useState<string | null>(null);
  const [openDialog, setOpenDialog] = useState(false);

  useEffect(() => {
    fetchAll();
    fetchUserId();
  }, []);

  const fetchAll = async () => {
    try {
      const res = await api.get<FilmDto[]>("/films");
      setFilms(res.data);
    } catch {
      toast.error("Помилка отримання фільмів");
    }
  };

  const fetchUserId = async () => {
    try {
      const res = await api.get("/users/me");
      setUserId(res.data.sub);
    } catch {
      toast.error("Не вдалося отримати ID користувача");
    }
  };

  const fetchMyTickets = async () => {
    if (!userId) return toast.warn("Користувач не визначений");
    try {
      const res = await api.get<TicketDto[]>(`/ticket/by-user/${userId}`);
      setMyTickets(res.data);
      toast.info(`Знайдено ${res.data.length} квитків`);
    } catch {
      toast.error("Не вдалося отримати квитки");
    }
  };

  const fetchByDirector = async () => {
    if (!searchDirector) return toast.warn("Введіть режисера");
    try {
      const res = await api.get<FilmDto[]>(`/films/director/${searchDirector}`);
      setFilms(res.data);
    } catch {
      toast.error("Фільми не знайдено");
    }
  };

  const fetchByTitle = async () => {
    if (!searchTitle) return toast.warn("Введіть назву фільму");
    try {
      const res = await api.get<FilmDto[]>(`/films/search?title=${searchTitle}`);
      setFilms(res.data);
    } catch {
      toast.error("Фільми не знайдено");
    }
  };

  const fetchByDate = async () => {
    if (!searchDate) return toast.warn("Введіть дату релізу");
    try {
      const res = await api.get<FilmDto[]>(`/films/release-date?date=${searchDate}`);
      setFilms(res.data);
    } catch {
      toast.error("Фільми не знайдено");
    }
  };

  const fetchTrailer = async (filmId: number) => {
    try {
      const res = await api.get<{ url: string }>(`/video/trailers/${filmId}`);
      setTrailerUrl(res.data.url);
      setOpenDialog(true);
    } catch {
      toast.error("Не вдалося отримати трейлер");
    }
  };

  return (
    <Container>
      <ToastContainer />
      <Typography variant="h4" gutterBottom>Сторінка користувача: Фільми</Typography>

      <Box mb={2}>
        <Button variant="contained" color="primary" onClick={fetchMyTickets}>
          Мої квитки
        </Button>
      </Box>

      <Box display="flex" alignItems="center" gap={2} mb={2}>
        <Button variant="outlined" onClick={fetchAll}>Усі фільми</Button>

        <TextField label="Режисер" value={searchDirector} onChange={(e) => setSearchDirector(e.target.value)} />
        <Button variant="outlined" onClick={fetchByDirector}>За режисером</Button>

        <TextField label="Назва" value={searchTitle} onChange={(e) => setSearchTitle(e.target.value)} />
        <Button variant="outlined" onClick={fetchByTitle}>За назвою</Button>

        <TextField label="Дата релізу" type="date" InputLabelProps={{ shrink: true }}
          value={searchDate} onChange={(e) => setSearchDate(e.target.value)} />
        <Button variant="outlined" onClick={fetchByDate}>За датою</Button>
      </Box>

      <Typography variant="h5" gutterBottom>Список фільмів</Typography>
      <List>
  {films.map((film) => (
    <ListItem key={film.id}>
      <Paper
        elevation={3}
        style={{ padding: "1rem", width: "100%", position: "relative" }}
        onMouseEnter={() => setHoveredFilmId(film.id)}
        onMouseLeave={() => setHoveredFilmId(null)}
      >
        <Typography variant="h6">{film.title}</Typography>
        <Typography>Жанр: {film.genre}</Typography>
        <Typography>Режисер: {film.director}</Typography>
        <Typography>
          Дата релізу: {new Date(film.releaseDate).toLocaleDateString()}
        </Typography>

        {hoveredFilmId === film.id && (
          <Button
            variant="contained"
            size="small"
            style={{ position: "absolute", top: 10, right: 10 }}
            onClick={() => fetchTrailer(film.id)}
          >
            Переглянути трейлер
          </Button>
        )}
      </Paper>
    </ListItem>
  ))}
</List>

      {myTickets.length > 0 && (
        <>
          <Typography variant="h5" gutterBottom>Ваші квитки</Typography>
          <List>
            {myTickets.map((t) => (
              <ListItem key={t.id}>
                <Paper elevation={2} style={{ padding: "0.5rem", width: "100%" }}>
                  🎟️ Квиток #{t.id} | Сеанс #{t.sessionId} | Ціна: {t.price} | Статус: {t.status}
                </Paper>
              </ListItem>
            ))}
          </List>
        </>
      )}

      <Dialog open={openDialog} onClose={() => setOpenDialog(false)} maxWidth="md" fullWidth>
        <Box p={2}>
          <Typography variant="h6" gutterBottom>Трейлер</Typography>
          {trailerUrl && (
            <video width="100%" controls>
              <source src={trailerUrl} type="video/mp4" />
              Ваш браузер не підтримує відтворення відео.
            </video>
          )}
        </Box>
      </Dialog>
    </Container>
  );
};
