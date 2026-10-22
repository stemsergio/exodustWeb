# exodust
vivecoding MMORPG Action Gacha Shooter Web Chicken Game

## Stack
- ASP.NET Core 10 MVC
- Full scaffolded ASP.NET Core Identity with roles + EF Core (SQL Server)
- SignalR for site notifications only (gameplay runs on the Go game server)
- Bootstrap UI (English)
- Google external login support (configure `Authentication:Google`)
- SendGrid email sender integration (configure `SendGrid`)

## User domain
- `UserOptions` table for free/paid plan selection
- `UserProgress` table for level/XP persistence
- Free plan shows a Google Ads banner placeholder on the game page

## Local setup
```
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.\SQLEXPRESS;Database=ExodustTest;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
dotnet user-secrets set "Seed:SuperAdminEmail" "<email>"
dotnet user-secrets set "Seed:SuperAdminPassword" "<password>"
dotnet ef migrations add InitialCreate -o Data/Migrations
```
On startup, `DbInitializer` applies pending migrations and creates the `SuperAdmin` and `User`
roles and the super admin.
