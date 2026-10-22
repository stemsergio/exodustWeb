# exodustWeb

Website, accounts and user database for Exodust, a 3D multiplayer game.
This repo is ONLY the web layer. The game simulation lives in a separate Go server.
Assume this repo may be public: no secrets, no data files, no internal URLs.

## Stack

- ASP.NET Core 10, MVC (Identity UI stays as Razor Pages in `Areas/Identity`)
- ASP.NET Core Identity with roles + EF Core on SQL Server (`.\SQLEXPRESS`, database `ExodustTest`, in development)
- `Data/DbInitializer` applies pending migrations on startup, then seeds roles (`SuperAdmin`, `User`)
  and the super admin from `Seed:*` config. New users get the `User` role; `Home/Index` sends them
  to `Game/Index`.
- Google login (optional), SendGrid email
- SignalR: `NotificationsHub` (`/hubs/notifications`) for site notifications only

## Responsibilities

This app owns:
- Users, login, registration, plans (free/paid), progress persistence.
- The database. No other service writes to it.
- Serving the web game client (built from a separate TypeScript project into
  `wwwroot/game/`, which is gitignored).

This app does NOT:
- Run gameplay or real-time game state. That is the Go server, over its own WebSocket.
- Decide game results. Progress (XP, levels, rewards) must come from the Go server.

## Integration with the Go game server (planned)

1. Logged-in player requests a short-lived signed token from this app.
2. The web client connects to the Go server's WebSocket with that token.
3. The Go server verifies the signature with a shared key. It never reads this DB.
4. The Go server reports results to an internal HTTP endpoint here; this app persists them.

## Rules

- **DB migrations are written by the human, never by Claude.** Model changes may be
  proposed; the migration itself is not generated or applied by Claude.
- Secrets via `dotnet user-secrets` in development and environment variables in production.
  `appsettings*.json` keeps empty placeholders only.
- Never commit `*.db`.
- Code and identifiers in English. Player-facing text targets Colombian Spanish users.
- Minimal, scoped changes. When two approaches are viable, present both and let the human choose.

## Known temporary code

- `Game/Index` has a "Gain 10 XP" button that lets the client grant itself XP.
  Demo only; to be replaced by results reported by the Go server.
