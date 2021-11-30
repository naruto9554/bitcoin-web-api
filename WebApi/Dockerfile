FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY WebApi.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/ ./
ENTRYPOINT ["dotnet", "WebApi.dll"]