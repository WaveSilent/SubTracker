# 📊 SubTracker - Управление подписками

Веб-приложение для контроля ежемесячных подписок. Помогает не забыть, сколько денег уходит на Netflix, Spotify и другие сервисы.

## 🚀 Технологии

- **Backend:** ASP.NET Core 9.0 Razor Pages
- **Database:** PostgreSQL + Entity Framework Core
- **Frontend:** Bootstrap 5 + Font Awesome
- **Container:** Docker + Docker Compose

## ✨ Возможности

- ✅ Полный CRUD для подписок
- ✅ Сортировка по названию, цене, дню списания
- ✅ Индикатор дней до следующего платежа (цветная метка)
- ✅ Темная тема с сохранением выбора
- ✅ Автоматические миграции при запуске
- ✅ Валидация на клиенте и сервере

## 🐳 Быстрый старт (Docker)

### Требования
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Запуск
```bash
# Клонируем репозиторий
git clone https://github.com/твой-аккаунт/SubTracker.git
cd SubTracker

# Запускаем контейнеры
docker-compose up -d

# Открываем в браузере
http://localhost:5000