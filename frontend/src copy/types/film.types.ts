export interface FilmDto {
  id: number;
  title: string;
  genre: string;
  director: string;
  releaseDate: string;
  description: string;
}

export interface CreateFilmDto {
  title: string;
  genre: string;
  director: string;
  releaseDate: string;
  description: string;
}

export interface UpdateFilmDto extends CreateFilmDto {
  id: number;
}