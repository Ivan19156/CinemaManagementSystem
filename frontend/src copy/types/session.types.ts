export interface SessionDto {
  id: number;
  filmId: number;
  hallId: number;
  startTime: string;
  ticketPrice: number;
}

export interface CreateSessionDto {
  filmId: number;
  hallId: number;
  startTime: string;
  ticketPrice: number;
}