FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /app

COPY App/App.csproj .
RUN dotnet restore

COPY App .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine as runtime
WORKDIR /app
COPY --from=build /app .

# RUN addgroup -S appgroup && adduser -S appuser -G appgroup
# USER appuser

ENTRYPOINT ["dotnet", "App.dll"]