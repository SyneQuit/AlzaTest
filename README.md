AlzaTest – Products API
=======================================

Overview
--------
Simple .NET API demonstrating layered architecture, API versioning (v1/v2), Swagger docs,
EF Core InMemory database with seed data, and an asynchronous queue for v2 stock updates.

Project Structure
-----------------
Api/                      Web API host (controllers, DI, Swagger, background worker, messaging demonstration logic)
Dtos/                     Request/response DTOs
Data/                     EF Core DbContext + entity configuration
Repositories/             Repository abstraction + EF implementation
Services/                 Application services (business rules)
Tests/                    Unit tests (controllers/services/repo)

Run API
---
1) Set startup project: Api
2) Start the app; Swagger UI is at /swagger (or /docs if configured).
3) Seed data is added on run.

Run Tests 
---
1) Simply run all tests from IDE

Versioning
----------
• v1 endpoints (classic behavior):
  - GET   /api/v1/product                       → all products (no paging)
  - GET   /api/v1/product/{id}                  → product by id
  - POST  /api/v1/product                       → create product
  - PATCH /api/v1/product/stock                 → synchronous stock quantity update (204 or 404)

• v2 endpoints (enhancements):
  - GET   /api/v2/product?pageNumber=&pageSize= → paged list (defaults: 1, 10)
  - GET   /api/v2/product/{id}                  → product by id
  - POST  /api/v2/product                       → create product
  - PATCH /api/v2/product/stock                 → ASYNCHRONOUS stock update (202 Accepted)

Data & Rules (short)
--------------------
• Entity: Product { Id, Name, Url, Description?, Price, StockQuantity }
• Duplicate name check on create (service-level); unique index would be configured for relational DBs.

Async Stock Updates (v2)
------------------------
• Controller enqueues StockUpdateMessage via IStockUpdateQueue and returns 202 with correlationId.
• BackgroundService (StockUpdateProcessor) consumes queue and updates stock via repository.
• In-memory queue uses Channel<T> (bounded); swap later for RabbitMQ/Kafka by replacing the IStockUpdateQueue implementation only.

Testing (very brief)
--------------------
• Controller tests mock services/queue.
• Service tests mock repository.
• Repository tests use EF InMemory.

