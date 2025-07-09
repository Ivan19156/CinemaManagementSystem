import { api } from "./axios";
import { LoginDto, RegisterUserDto } from "../types/auth";

// ✅ правильний шлях: /api/auth/login
export const login = (dto: LoginDto) => 
  api.post<string>("/users/login", dto);

// ✅ правильний шлях: /api/auth/register
export const register = (dto: RegisterUserDto) =>
  api.post<number>("/users/register", dto);
