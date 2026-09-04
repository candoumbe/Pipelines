# Squad Decisions

## Active Decisions

### 2026-09-04: Align NuGet.Frameworks with .NET SDK 10
**By:** McManus
**What:** Pin `NuGet.Frameworks` centrally to `7.9.0`.
**Why:** `Fallout.Common 10.3.49` brings `NuGet.Packaging 6.14.3` and `NuGet.Frameworks 6.14.3`, while SDK `10.0.400` evaluates `net10.0` through `NuGet.Frameworks 7.9.0.0`. Central transitive pinning keeps the build process on the SDK-compatible assembly without changing generated workflows or the launcher.

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction
