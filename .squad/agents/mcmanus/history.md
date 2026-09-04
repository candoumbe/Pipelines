# Project Context

- **Owner:** Cyrille NDOUMBE
- **Project:** Pipelines
- **Stack:** C#, .NET 10/.NET 8, NUKE, xUnit, GitHub Actions
- **Created:** 2026-04-26

## Learnings

- Build orchestration is centered on NUKE in `build/Pipeline.cs`.
- GitHub workflows are generated; do not manually edit workflow YAML files.
- Packaging and version strategy follow GitVersion and central package management conventions.
- `Fallout.Common 10.3.49` brings `NuGet.Frameworks 6.14.3`; pinning it centrally to `7.9.0` aligns the runtime assembly with SDK `10.0.400` during `net10.0` evaluation.

- 2026-09-04: The `feature/add-delete-package-target` release was split into three commits: dependency/configuration, GitHub container deletion component, and changelog. The solution passed build and 74 tests; publishing the branch worked, but PR creation requires GitHub CLI authentication.
