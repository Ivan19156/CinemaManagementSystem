# Етап 1: збірка Vite-проєкту
FROM node:18-alpine AS builder

WORKDIR /app

# Копіюємо package.json і lock-файл
COPY package*.json ./
# Встановлюємо залежності
RUN npm install

# Копіюємо весь проєкт
COPY . .

# Білд (білд-файли потраплять в /app/dist)
RUN npm run build

# Етап 2: запуск продакшен-версії на Nginx
FROM nginx:stable-alpine

# Копіюємо збірку у папку, яку сервить nginx
COPY --from=builder /app/dist /usr/share/nginx/html

# Копіюємо базовий nginx-конфіг, якщо потрібно (опціонально)
# COPY nginx.conf /etc/nginx/nginx.conf

# Відкриваємо порт 80 всередині контейнера
EXPOSE 80

# Стартуємо nginx у фореґраунді
CMD ["nginx", "-g", "daemon off;"]
