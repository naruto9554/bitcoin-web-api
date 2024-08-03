FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR /app

COPY App .

RUN dotnet restore

RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine

COPY --from=build /app .

# Use default non-root user
USER app

ENTRYPOINT ["dotnet", "App.dll"]