export interface SaleDto {
  id: number;
  userId: number;
  ticketId: number;
  amount: number;
  ticketsCount: number;
  totalPrice: number;
  saleDate: string;
  ticketIds: number[];
}

export interface CreateSaleDto {
  userId: number;
  ticketId: number;
  amount: number;
  ticketsCount: number;
  totalPrice: number;
  saleDate: string;
}

export interface UpdateSaleDto {
  id: number;
  userId: number;
  ticketsCount: number;
  totalPrice: number;
  saleDate: string;
}