FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR /app

COPY App .
COPY *.props .

RUN dotnet restore ./Api/Api.csproj

RUN dotnet publish ./Api/Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine

COPY --from=build /app/publish .

# Use default non-root user
USER app

ENTRYPOINT ["dotnet", "Api.dll"]