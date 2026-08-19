# ============================
# ETAPA 1: COMPILACIÓN
# ============================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copiar el proyecto
COPY . .

# Restaurar dependencias
RUN dotnet restore "invernaderoInteligenteBackend.csproj"

# Compilar y publicar
RUN dotnet publish "invernaderoInteligenteBackend.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# ============================
# ETAPA 2: EJECUCIÓN
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

# Instalar librería necesaria para GSSAPI/Kerberos
RUN apt-get update && \
    apt-get install -y --no-install-recommends libgssapi-krb5-2 && \
    rm -rf /var/lib/apt/lists/*

# Copiar aplicación publicada
COPY --from=build /app/publish .

# Puerto de la aplicación
EXPOSE 8080

# Configuración ASP.NET
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Iniciar aplicación
ENTRYPOINT ["dotnet", "invernaderoInteligenteBackend.dll"]