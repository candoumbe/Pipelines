# Project Guidelines

## Architecture

This project is a **component-based CI/CD framework** built on top of [NUKE](https://nuke.build/). It provides reusable, composable pipeline components as C# interfaces with default implementations.

### Core Design Principles

- **Interface-based composition**: Each pipeline capability (clean, restore, compile, test, pack, publish…) is a separate interface (e.g., `IClean`, `ICompile`, `IPack`). Pipelines compose behavior by implementing multiple interfaces.
- **Convention over configuration**: Components provide opinionated defaults. Override only when necessary.
- **Infrastructure interfaces prefix with `IHave`** (e.g., `IHaveSourceDirectory`, `IHaveArtifacts`): these expose paths, settings, or external resources.
- **Action interfaces prefix with `I` + verb** (e.g., `ICompile`, `IRestore`, `IPack`): these define executable build targets.
- **Workflow interfaces prefix with `IDo`** (e.g., `IDoFeatureWorkflow`, `IDoChoreWorkflow`): these define branching strategies.

### Project Structure

```
src/
  Candoumbe.Pipelines/                          # Core library — NuGet package
    Components/                                  # All component interfaces
      IClean.cs, ICompile.cs, IRestore.cs, …    # Build target components
      IHaveSolution.cs, IHaveArtifacts.cs, …    # Infrastructure components
      Docker/                                    # Docker-related components
      Formatting/                                # Code formatting (IDotnetFormat)
      GitHub/                                    # GitHub integration (releases, PRs)
      NuGet/                                     # NuGet publishing
      Workflows/                                 # Git branching strategies (GitFlow, GitHubFlow)
    tools/Stryker/                               # Stryker.NET config for mutation testing
  Candoumbe.Pipelines.Components.AzureDevOps/    # Standalone Azure DevOps extension package
build/
  Pipeline.cs                                    # Self-hosting build — consumes the library
```

**Do not break this structure.** New components go in the appropriate subdirectory under `Components/`. New provider-specific integrations go in a separate project (like `Candoumbe.Pipelines.Components.AzureDevOps`).

### Adding a New Component

1. Create an interface in the appropriate `Components/` subdirectory.
2. Follow the naming conventions: `IHave*` for infrastructure, `I{Verb}` for build targets, `IDo*Workflow` for branching strategies.
3. The interface should inherit from the relevant base interfaces (e.g., `INukeBuild`, `IHaveSolution`).
4. Provide a default implementation via interface default methods.
5. Ensure the component is composable — avoid tight coupling with other components.

## Code Style

- **C# latest** language version with **.NET 10** (also targets .NET 8).
- Follow the [.editorconfig](.editorconfig): 4-space indentation, `var` only for inferred types (enforced as error), switch expressions preferred.
- Private fields use `_camelCase` prefix.
- Interfaces are named `IPascalCase`.
- Namespace must match folder structure.

## Build and Test

```bash
# Build the solution
./build.sh compile

# Create NuGet packages
./build.sh pack

# Run all default targets
./build.sh
```

The build uses NUKE via [build/Pipeline.cs](build/Pipeline.cs). The `Pipeline` class itself composes components from this library.

Package versions are managed centrally in [Directory.Packages.props](Directory.Packages.props).

## Conventions

- **Semantic versioning** via [GitVersion](https://gitversion.net/) — never set versions manually.
- **Changelog** follows [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) format in [CHANGELOG.md](CHANGELOG.md).
- **Branching model**: GitFlow — `feature/*`, `hotfix/*`, `release/*`, `chore/*`, `coldfix/*` branches from `develop`.
- **GitHub workflows** are auto-generated from `Pipeline.cs` via `ICanRegenerateGitHubWorkflows` — do not edit `.github/workflows/*.yml` manually.
- **Central package management** is enabled — add package versions in `Directory.Packages.props`, not in individual `.csproj` files.
