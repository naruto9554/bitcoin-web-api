[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Tsingis_bitcoin-web-api&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Tsingis_bitcoin-web-api) [![Deploy Status](https://github.com/tsingis/bitcoin-web-api/actions/workflows/deploy.yml/badge.svg)](https://github.com/Tsingis/bitcoin-web-api/actions/workflows/deploy.yml)

# About

API based on [GoinGecko's public API.](https://www.coingecko.com/en/api/documentation) to get initial data for bitcoin with euro as currency. API attempts to give answer to following questions with given date range.

1. How many days is the longest downward trend?
2. Which date has the highest trading volume?
3. What are the best days to buy and sell?

API documentation

-   Swagger [here](https://bitcoin-web-api.onrender.com/swagger)
-   Scalar [here](https://bitcoin-web-api.onrender.com/scalar)

Tools used:

-   .NET 9 SDK
-   .NET report generator `dotnet tool install -g dotnet-reportgenerator-globaltool`
-   Docker
