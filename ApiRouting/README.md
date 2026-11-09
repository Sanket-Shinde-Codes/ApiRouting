# User Management API

A .NET 10 minimal API for managing in-memory user records.

## Run

From this directory, run:

```powershell
dotnet run
```

In Development, Swagger UI is available at `http://localhost:5221/swagger`. Use
the request collection in [UserManagementAPI.http](./UserManagementAPI.http) to exercise the
API from VS Code or another compatible HTTP client.

## Endpoints

| Method | Route | Behavior |
|---|---|---|
| GET | `/api/users` | List users |
| GET | `/api/users/{id}` | Retrieve a user |
| POST | `/api/users` | Create a user (`201 Created`) |
| PUT | `/api/users/{id}` | Replace a user's details |
| DELETE | `/api/users/{id}` | Delete a user (`204 No Content`) |

Names, email addresses, and departments are validated. Email addresses must be
unique. A duplicate email returns `409 Conflict`; an unknown ID returns
`404 Not Found`. Invalid JSON returns `400 Bad Request` with Problem Details,
while unexpected server errors are logged and returned as generic Problem
Details without leaking exception messages. User data is held in memory and
resets when the application stops.

All `/api` routes require a bearer token. Set `Authentication__BearerToken` to
a strong secret in the environment or a secret store before starting the API;
the application never supplies a default token. Requests without a matching
`Authorization: Bearer <token>` header receive `401 Unauthorized`. The HTTP
request collection uses a placeholder token that must be replaced locally.

The middleware pipeline handles exceptions first, authenticates API requests
second, and logs request method, path, response status, elapsed time, and trace
ID last. Tokens and request/response bodies are deliberately not logged.
Authentication failures are logged by the authentication middleware so denied
requests are audited despite being rejected before the logging middleware.
In Development only, `/api/_diagnostics/throw` can be used with a valid token
to verify that unexpected exceptions produce sanitized `500` Problem Details.

The debug pass confirmed required/email validation and missing-user behavior
were already functioning. It identified malformed JSON being surfaced as
`500 Internal Server Error`; a centralized exception handler now maps that
input error to `400`. Copilot also optimized list retrieval to avoid sorting
the full collection on each request and added an email index for efficient
duplicate checks. The HTTP request collection includes blank-input and
malformed-JSON edge cases.

## How Microsoft Copilot assisted

- Scaffolded the ASP.NET Core entry-point setup and minimal API structure.
- Generated the user request/response contracts, service abstraction, and
  in-memory CRUD implementation.
- Added validation, OpenAPI endpoint descriptions, and consistent HTTP status
  behavior including resource locations for created users.
- Prepared the `.http` request collection for manual CRUD and error-path checks.
- Diagnosed malformed JSON producing a server error, added centralized safe
  exception mapping/logging, and improved in-memory user-list and email lookup
  performance.
- Added bearer-token authentication, request/response audit logging, ordered
  the middleware pipeline, and documented local token configuration and
  middleware checks.
