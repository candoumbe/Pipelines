# Project Context

- **Owner:** Cyrille NDOUMBE
- **Project:** Pipelines
- **Stack:** C#, .NET 10/.NET 8, NUKE, xUnit, GitHub Actions
- **Created:** 2026-04-26

## Learnings

- Project is a component-based CI/CD framework using composable interfaces.
- Infrastructure interfaces use `IHave*`; action interfaces use `I{Verb}`; workflow interfaces use `IDo*Workflow`.
- Core library lives in `src/Candoumbe.Pipelines`; provider-specific extensions live in separate projects.
