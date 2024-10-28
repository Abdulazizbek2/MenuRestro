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
# Используем официальный образ .NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5264

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MenuRestro.csproj", "./"]
RUN dotnet restore "./MenuRestro.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MenuRestro.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MenuRestro.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Установите переменную окружения для использования в миграциях, если нужно
# ENV ASPNETCORE_ENVIRONMENT Development

# Выполнение миграции базы данных при запуске контейнера
ENTRYPOINT ["dotnet", "ef", "database", "update", "--no-build", "--context", "YourDbContextName"]
CMD ["dotnet", "MenuRestro.dll"]