# Базовый образ для сборки (используем .NET 9.0)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем файл проекта и восстанавливаем зависимости
COPY ["SubscriptionManager.csproj", "."]
RUN dotnet restore "SubscriptionManager.csproj"

# Копируем все остальные файлы и публикуем приложение
COPY . .
RUN dotnet publish "SubscriptionManager.csproj" -c Release -o /app/publish

# Финальный образ с рантаймом (тоже .NET 9.0)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Копируем собранное приложение
COPY --from=build /app/publish .

# Запускаем приложение
ENTRYPOINT ["dotnet", "SubscriptionManager.dll"]