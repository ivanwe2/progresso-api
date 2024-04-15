FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY . .
WORKDIR /app/tests/Prime.Progreso.Services.Test
RUN dotnet test

WORKDIR /app/src/Prime.Progreso.Api
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

COPY --from=build /app/src/Prime.Progreso.Api/out ./
ENTRYPOINT ["dotnet","Prime.Progreso.Api.dll"]
