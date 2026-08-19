# ============================
# Etapa de ejecución
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

# Instalar dependencia necesaria para Kerberos/GSSAPI
RUN apt-get update && \
    apt-get install -y libgssapi-krb5-2 && \
    rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# Puerto interno
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "invernaderoInteligenteBackend.dll"]