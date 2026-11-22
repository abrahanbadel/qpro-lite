# ---------- Etapa 1: compilación ----------
# Imagen con el SDK completo de .NET; solo se usa para compilar y no llega a producción.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Se copia primero el archivo de proyecto para aprovechar la caché de capas:
# mientras no cambien las dependencias, Docker no vuelve a ejecutar la restauración.
COPY src/QproLite.Api/QproLite.Api.csproj src/QproLite.Api/
RUN dotnet restore src/QproLite.Api/QproLite.Api.csproj

# Se copia el resto del código y se publica la aplicación en modo Release.
COPY src/QproLite.Api/ src/QproLite.Api/
RUN dotnet publish src/QproLite.Api/QproLite.Api.csproj -c Release -o /app/publish --no-restore

# ---------- Etapa 2: ejecución ----------
# Imagen liviana que solo trae el runtime de ASP.NET Core.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Carpeta de datos para la base SQLite, propiedad del usuario sin privilegios de la imagen.
RUN mkdir /datos && chown $APP_UID /datos
VOLUME /datos

# Solo se traen los binarios publicados en la etapa anterior.
COPY --from=build /app/publish .

# La API escucha en el puerto 8080 y guarda la base de datos en el volumen.
ENV ASPNETCORE_HTTP_PORTS=8080 \
    ConnectionStrings__Qpro="Data Source=/datos/qpro.db"
EXPOSE 8080

# El contenedor no se ejecuta como root.
USER $APP_UID

ENTRYPOINT ["dotnet", "QproLite.Api.dll"]
