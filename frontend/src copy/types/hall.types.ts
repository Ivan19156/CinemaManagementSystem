export interface HallDto {
  id: number;
  name: string;
  capacity: number;
  seatsCount: number;
}

export interface CreateHallDto {
  name: string;
  capacity: number;
  seatsCount: number;
}