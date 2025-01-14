FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

WORKDIR /app

COPY App .
COPY *.props .

RUN dotnet restore ./Api/Api.csproj

RUN dotnet publish ./Api/Api.csproj -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine

COPY --from=build /app/publish .

# Use default non-root user
USER app

ENTRYPOINT ["dotnet", "Api.dll"]