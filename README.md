# exodust
vivecoding MMORPG Action Gacha Shooter Web Chicken Game

## Stack
- ASP.NET Core 10 Razor Pages
- Full scaffolded ASP.NET Core Identity + EF Core (SQLite)
- SignalR real-time game updates
- Bootstrap UI (English)
- Google external login support (configure `Authentication:Google`)
- SendGrid email sender integration (configure `SendGrid`)

## User domain
- `UserOptions` table for free/paid plan selection
- `UserProgress` table for level/XP persistence
- Free plan shows a Google Ads banner placeholder on the home page
