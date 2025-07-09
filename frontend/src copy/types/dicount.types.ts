export interface DiscountDto {
  id: number;
  code: string;
  name: string;
  percentage: number;
  description: string;
  isUsed: boolean;
}

export interface EditForm {
  id: number;
  name: string;
  percentage: string;
  startDate: string;
  endDate: string;
  description: string;
}