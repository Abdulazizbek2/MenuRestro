# # Используем официальный образ .NET
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# WORKDIR /app
# EXPOSE 5264

# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src
# COPY ["MenuRestro.csproj", "./"]
# RUN dotnet restore "./MenuRestro.csproj"
# COPY . .
# WORKDIR "/src/."
# RUN dotnet build "MenuRestro.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "MenuRestro.csproj" -c Release -o /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .

# # Выполнение миграции базы данных при запуске контейнера
# ENTRYPOINT ["dotnet", "MenuRestro.dll"]
# CMD ["sh", "-c", "dotnet ef database update && dotnet MenuRestro.dll"]

# Ступень 1: Сборка приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Копируем файл проекта и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем все остальные файлы и собираем приложение
COPY . ./
RUN dotnet publish -c Release -o /out

# Ступень 2: Создание конечного образа
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Копируем собранное приложение из этапа сборки
COPY --from=build-env /out .

# Открываем порт для доступа к приложению
EXPOSE 80

# Запускаем приложение
ENTRYPOINT ["dotnet", "MenuRestro.dll"]