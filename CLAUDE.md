# Exodust

Authoritative-server multiplayer game. One-person project, learning-as-building.
Start minimal and 2D; the architecture must scale to MMO-class later without rewrites.

## Tech stack

- **Server:** Go. Single binary. Serves a WebSocket endpoint for the game and
  HTTP endpoints for account/services. No microservices yet — one process.
- **Client:** TypeScript in the browser. Canvas 2D now; migrate to Pixi.js when
  sprite counts demand it. Thin client: renders state and sends input, nothing else.
- **Transport:** WebSocket (game loop) + HTTP/JSON (services). No UDP.
- **Serialization:** JSON now. Move to a binary/generated protocol later if needed.

## Architecture (this is the core invariant)

The game is a pure function: `tick(state, inputs, dt) -> newState`, run at a fixed
rate on the server. The server is the single source of truth. Clients never decide
game outcomes; they send inputs and render snapshots.

```
/sim      ECS core: entities, components, systems, tick(). Pure. Headless.
/server   wraps /sim: WebSocket, HTTP, fixed-tick loop, snapshot broadcast
/client   TypeScript thin renderer + input capture
/shared   protocol/message type definitions shared in spirit by client & server
```

ECS model inside /sim:
- **Entity** = an id.
- **Component** = flat data attached to an entity (Position, Velocity, Health...).
- **System** = a pure function over entities that have given components.
- New mechanic = new component + new system. Never edit unrelated systems.

## Non-negotiable rules

1. **/sim imports nothing from /server, /client, network, engine, or DB.**
   If a /sim file imports a websocket/db/engine package, the architecture is broken.
2. **tick() is deterministic.** Same state + same inputs => same output, always.
   No unseeded randomness, no reading system time inside the simulation.
3. **Server is authoritative.** Client input is a request, never a decision.
4. **Data is flat and separate from logic.** Components are data; systems are behavior.
5. **No premature scaling.** One process, in-memory state, monolith until a real
   problem forces otherwise. The isolated /sim is what makes later scaling cheap.

## Persistence

- MVP: state lives in memory. No DB yet.
- **The user/account/progress data model is the one thing worth designing carefully
  early** — it's the only piece with real long-term migration cost once real users exist.
- **DB migrations are written by the human, never by Claude Code.**

## Conventions

- Code and identifiers in English. UI strings target Colombian Spanish users.
- Go: handle errors explicitly; run `go test -race` (shared mutable state =
  race-condition risk, the main hazard of this design).
- Keep changes minimal and scoped. Do not refactor unrelated code.
- When unsure between two approaches, explain both and let the human choose.

## Current phase

First milestone: "the authoritative square" — one entity with a Position component,
one MovementSystem, tick() running headless in a test, then wired through WebSocket
to a Canvas client. Two browser tabs see the same server-driven square move.
Build /sim first, then /server, then /client. Do not skip ahead.
