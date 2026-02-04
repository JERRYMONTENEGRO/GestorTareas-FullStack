# 1. Usar la imagen de .NET 8 para construir
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 2. Copiar archivos y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# 3. Copiar el resto y publicar
COPY . ./
RUN dotnet publish -c Release -o out

# 4. Imagen final para ejecutar (más ligera)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Configuración de puerto para Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MiApiDotNet.dll"]

