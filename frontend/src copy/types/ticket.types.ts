export interface TicketDto {
  id: number;
  sessionId: number;
  userId: number;
  seatNumber: string;
  purchaseDate: string;
  isCanceled: boolean;
  price: number;
}

export interface CreateTicketDto {
  sessionId: number;
  userId: number;
  saleId: number;
  seatNumber: string;
  price: number;
  time: string;
  movie: string;
  hall: string;
  email: string;
}

export interface UpdateTicketDto {
  seatNumber: string;
  price: number;
}