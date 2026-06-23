# Exodust — Roadmap (human-owned, NOT Claude Code memory)

This file is deliberately kept out of CLAUDE.md. Plans change; memory files should
not become fossils. Edit this freely. Claude Code does not need it loaded every session.

## North star

Nintendo-grade *feel and charm*, not Nintendo-grade scope. One person.
Every mechanic, even a moving square, should feel good before moving on.

## Phases

**Years 1–2 — 2D, free, ad-supported, expect bugs**
- Go authoritative server + TypeScript client.
- Canvas 2D while learning; migrate to Pixi.js before end of year 2.
- Build the art pipeline and learn sound in parallel with the code.
- Goal: something genuinely playable and likable. Bugs acceptable.

**Years 3–8 — 3D indie, formalized**
- Frontend is largely *rewritten* for 3D (likely Three.js). Note: this is a rewrite
  of the client, not an upgrade. The Go server and /sim should survive nearly intact
  precisely because /sim is render-agnostic — that's the payoff of the isolation rule.
- Trademark/brand registration, legal formalization, monetization. Right time for
  this is here, not earlier — no point protecting a free buggy prototype.

## Open decisions (revisit when relevant, do not pre-solve)

- 2D engine: Canvas -> Pixi confirmed. Phaser considered but it wants to own the
  game loop, which fights the authoritative-server model. Avoid unless disciplined.
- Anti-cheat / PvP hardening: explicitly the LAST phase. Authoritative server already
  removes most client-side cheating by design; deep hardening waits, possibly a hire.
- Hosting: Azure App Service (already paid) for Fase 0 learning, accepting that it
  recycles and loses in-memory state. Move to a fixed-price VPS with a US-East data
  center (DigitalOcean has $200/60-day credit + US regions = good latency from Colombia)
  when there's something playable. Not serverless — the server is stateful/long-lived.

## Reminders to self

- Don't adopt AAA complexity (microservices, UDP, multiple languages) before a real
  problem demands it. That complexity is the answer to scale you don't have yet.
- Hire only after enough success. Solo until then.
