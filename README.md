# Currency Converter API

A robust, secure, and scalable Currency Converter API built with ASP.NET Core. This project serves as a backend solution for converting currencies and retrieving exchange rates using the [Frankfurter API](https://www.frankfurter.app/docs/).

---

## Requirements
1. .NET 8.0 SDK
2. Docker

## Setup Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/eynarhaji/FrankfurterApp.git

2. **Run docker compose**
   ```bash
   docker compose -f infrastructure.yml up -d
   
NOTE: If you don't want to use docker, you can run project in Local environment.
      If you want to use docker, you can run project in Development environment.

3. **Run the application**
   ```bash
    dotnet run --project 'FrankfurterApp/FrankfurterApp.csproj' --launch-profile Local

## Assumptions made

1. Made Redis cache optional to eliminate the need for a Redis server.
2. Made Elasticsearch optional to eliminate the need for an Elasticsearch server.
3. Added global error handler and translator for exceptions.
4. Added execution context accessor to access user and request data.
5. Didn't cover additional added files with tests. That's why test coverage is low. Covered only success scenarios.

## Possible future enhancements
1. Add unit tests for all services and controllers.
2. Make project more modular by separating the API and services into different projects.
3. Add SSE for real-time updates.
4. Add persistent storage for exchange rates.

