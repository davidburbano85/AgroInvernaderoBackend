# ============================
# Etapa de compilación
# ============================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copiar archivos del proyecto
COPY . .

# Restaurar dependencias
RUN dotnet restore "invernaderoInteligenteBackend.csproj"

# Publicar aplicación
RUN dotnet publish "invernaderoInteligenteBackend.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ============================
# Etapa de ejecución
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

# Puerto interno
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "invernaderoInteligenteBackend.dll"]