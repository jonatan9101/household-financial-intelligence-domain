# Local Development

How to run the MVP API against the local PostgreSQL database.

## Prerequisites

- Docker Desktop
- .NET SDK 10
- (Migrations only) EF Core tools installed: `dotnet tool install --global dotnet-ef`

## Start the database

```sh
docker compose up -d
```

Starts Postgres 16 in the container `hfi-postgres` on port `5432` (volume `hfi-postgres-data`). Wait until it is healthy:

```sh
docker ps --filter name=hfi-postgres
```

## Apply migrations

Migrations are **not** applied automatically at startup — the application never mutates the database. Apply the schema explicitly:

```sh
dotnet ef database update \
  --project src/HouseholdFinancialIntelligence.Infrastructure \
  --startup-project tools/PersistenceSmoke
```

## Run the API

```sh
dotnet run --project src/HouseholdFinancialIntelligence.Api
```

The API listens on `http://localhost:5114` in Development. Development reads the connection string from `appsettings.Development.json`. Use the `ConnectionStrings__Default` environment variable to override it (for example, a Supabase database) without changing the committed configuration.

## Smoke test

Register a movement:

```sh
curl -X POST http://localhost:5114/api/financial-movements \
  -H "Content-Type: application/json" \
  -d '{"householdId":"<guid>","financialAccountId":"<guid>","amount":150.00,"currency":"USD","movementType":"Purchase","transactionDate":"2026-08-01","evidenceReference":"receipt-001","occurredAt":"2026-08-01T10:30:00Z"}'
```

Expected responses:

- Valid request → `201` with `{ "id": "<guid>" }` and a `Location` header.
- Same `evidenceReference` again → `409` with ProblemDetails `code: "FM-001"` (duplicate movement).
- Invalid/missing data (bad currency, missing `OccurredAt`, malformed body) → `400` with ProblemDetails.

## Troubleshooting

- Relation/column not found when calling the API → migrations were not applied; run the `dotnet ef database update` step above.
- Connection refused → Docker is not running or `hfi-postgres` is not healthy.