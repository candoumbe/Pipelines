# Contributing to Candoumbe.Pipelines

Thank you for your interest in contributing! This document outlines the coding conventions, style rules, and best practices you should follow when working on this project.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Branching Model](#branching-model)
- [Naming Conventions](#naming-conventions)
- [Code Style](#code-style)
- [Component Design](#component-design)
- [Testing](#testing)
- [Package Management](#package-management)
- [Versioning & Changelog](#versioning--changelog)
- [CI/CD Workflows](#cicd-workflows)
- [Build Commands](#build-commands)

---

## Prerequisites

- **.NET SDK 10.0** (also targets .NET 8.0)
- **C# latest** language version
- An editor that respects [`.editorconfig`](.editorconfig) (VS Code, Visual Studio, Rider…)

---

## Project Structure

```
src/
  Candoumbe.Pipelines/                          # Core library (NuGet package)
    Components/                                  # All component interfaces
      Docker/                                    # Docker-related components
      Formatting/                                # Code formatting components
      GitHub/                                    # GitHub integration
      NuGet/                                     # NuGet publishing
      Workflows/                                 # Git branching strategies
  Candoumbe.Pipelines.Components.AzureDevOps/    # Azure DevOps extension (separate package)
build/
  Pipeline.cs                                    # Self-hosting build pipeline
test/
  Candoumbe.Pipelines.Tests/                     # Unit tests
```

**Rules:**

- New components go in the appropriate subdirectory under `Components/`.
- New provider-specific integrations go in a **separate project** (like `Candoumbe.Pipelines.Components.AzureDevOps`).
- **1 file per type, 1 type per file.**
- **Namespace must match folder structure.**

---

## Branching Model

This project uses **GitFlow**. Branch prefixes:

| Branch type   | Prefix        | Source branch |
|---------------|---------------|---------------|
| Feature       | `feature/`    | `develop`     |
| Hotfix        | `hotfix/`     | `main`        |
| Coldfix       | `coldfix/`    | `develop`     |
| Release       | `release/`    | `develop`     |
| Chore         | `chore/`      | `develop`     |

> **Never commit directly to `main` or `develop`.**

---

## Naming Conventions

### Interfaces

| Category       | Prefix     | Purpose                         | Examples                                      |
|----------------|------------|----------------------------------|-----------------------------------------------|
| Infrastructure | `IHave`    | Expose paths, settings, resources | `IHaveArtifacts`, `IHaveSolution`             |
| Action         | `I` + Verb | Define executable build targets   | `IClean`, `ICompile`, `IPack`, `IRestore`     |
| Workflow       | `IDo`      | Define branching strategies       | `IDoFeatureWorkflow`, `IDoChoreWorkflow`      |

### Fields

| Visibility              | Style            | Example                    |
|--------------------------|------------------|----------------------------|
| Private / Protected      | `_camelCase`     | `_outputDirectory`         |
| Private static readonly  | `s_camelCase`    | `s_defaultConfig`          |
| Public / Internal        | `PascalCase`     | `OutputDirectory`          |
| Constants                | `PascalCase`     | `DevelopBranchName`        |

### General

- Classes, structs, enums, delegates: `PascalCase`
- Interfaces: `IPascalCase` (prefix with `I`)
- Parameters: `camelCase`
- Methods, properties, events: `PascalCase`

---

## Code Style

### Use explicit types — `var` is **not** allowed

This is **enforced as an error** in the `.editorconfig`.

```csharp
// ✅ DO — use explicit types
AbsolutePath directory = RootDirectory / "src";
string name = "example";
int count = 42;
IEnumerable<AbsolutePath> paths = Enumerable.Empty<AbsolutePath>();

// ❌ DON'T — never use var
var directory = RootDirectory / "src";
var name = "example";
var count = 42;
var paths = Enumerable.Empty<AbsolutePath>();
```

### Indentation and spacing

- **4 spaces** for indentation (no tabs).
- **2 spaces** for `.csproj`, `.json`, `.props`, `.xml`, `.yaml`, `.yml` files.

### Braces

Always use braces, even for single-line blocks (enforced as warning).

```csharp
// ✅ DO
if (condition)
{
    DoSomething();
}

// ❌ DON'T
if (condition)
    DoSomething();
```

### Brace placement

Allman style — opening braces on a new line.

```csharp
// ✅ DO
public class MyClass
{
    public void MyMethod()
    {
        // ...
    }
}

// ❌ DON'T
public class MyClass {
    public void MyMethod() {
        // ...
    }
}
```

### Switch expressions preferred

Use switch expressions over switch statements (enforced as error).

```csharp
// ✅ DO
string result = status switch
{
    Status.Active => "active",
    Status.Inactive => "inactive",
    _ => "unknown"
};

// ❌ DON'T
string result;
switch (status)
{
    case Status.Active:
        result = "active";
        break;
    case Status.Inactive:
        result = "inactive";
        break;
    default:
        result = "unknown";
        break;
}
```

### Using directives

- Place `using` directives **outside** the namespace (enforced as error).
- Sort `System` directives first.

```csharp
// ✅ DO
using System;
using System.Collections.Generic;
using Nuke.Common;

namespace Candoumbe.Pipelines.Components;

// ❌ DON'T
namespace Candoumbe.Pipelines.Components
{
    using System;
    using Nuke.Common;
}
```

### File-scoped namespaces

Prefer file-scoped namespace declarations.

```csharp
// ✅ DO
namespace Candoumbe.Pipelines.Components;

public interface IHaveArtifacts : IHaveOutputDirectory
{
    // ...
}

// ❌ DON'T (unless existing code uses block-scoped)
namespace Candoumbe.Pipelines.Components
{
    public interface IHaveArtifacts : IHaveOutputDirectory
    {
        // ...
    }
}
```

### Null handling

- Use null propagation (`?.`) — enforced as warning.
- Use null coalescing (`??`) — enforced as warning.
- Prefer `is null` / `is not null` over `== null` / `!= null`.

```csharp
// ✅ DO
string value = input ?? "default";
int? length = text?.Length;
if (obj is not null) { ... }

// ❌ DON'T
string value = input != null ? input : "default";
int? length = text != null ? text.Length : null;
if (obj != null) { ... }
```

### Object initializers and collection expressions

Prefer object initializers and collection expressions (enforced as warning).

```csharp
// ✅ DO
Person person = new() { Name = "Alice", Age = 30 };
IEnumerable<Project> projects = [];

// ❌ DON'T
Person person = new Person();
person.Name = "Alice";
person.Age = 30;
List<Project> projects = new List<Project>();
```

### Predefined types

Use predefined types (`string`, `int`, `bool`) instead of framework names (enforced as error for locals/parameters).

```csharp
// ✅ DO
string name = "example";
int count = 0;

// ❌ DON'T
String name = "example";
Int32 count = 0;
```

---

## Component Design

### Infrastructure interface (`IHave*`)

Infrastructure interfaces expose paths, settings, or external resources. They inherit from `INukeBuild` and provide default implementations.

```csharp
// ✅ DO — simple, composable infrastructure interface
using Nuke.Common;
using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that can specify a folder for source files
/// </summary>
public interface IHaveSourceDirectory : INukeBuild
{
    /// <summary>
    /// Directory of source code projects
    /// </summary>
    AbsolutePath SourceDirectory => RootDirectory / "src";
}
```

### Action interface (`I` + Verb)

Action interfaces define executable build targets. They inherit from the infrastructure interfaces they need.

```csharp
// ✅ DO — action interface with default target implementation
using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that supports cleaning workflow.
/// </summary>
public interface IClean : INukeBuild
{
    IEnumerable<AbsolutePath> DirectoriesToDelete => Enumerable.Empty<AbsolutePath>();

    IEnumerable<AbsolutePath> DirectoriesToClean => Enumerable.Empty<AbsolutePath>();

    IEnumerable<AbsolutePath> DirectoriesToEnsureExistence => Enumerable.Empty<AbsolutePath>();

    public Target Clean => _ => _
       .TryBefore<IRestore>(x => x.Restore)
       .OnlyWhenDynamic(() => DirectoriesToClean.AtLeastOnce()
                              || DirectoriesToDelete.AtLeastOnce()
                              || DirectoriesToEnsureExistence.AtLeastOnce())
       .Executes(() =>
       {
           DirectoriesToDelete.ForEach(directory => directory.DeleteDirectory());
           DirectoriesToClean.ForEach(directory => directory.CreateOrCleanDirectory());
           DirectoriesToEnsureExistence.ForEach(directory => directory.CreateDirectory());
       });
}
```

### Key design rules

```csharp
// ✅ DO — compose interfaces to build capabilities
public interface ICompile : IHaveSolution, IHaveConfiguration { ... }

// ✅ DO — provide sealed base settings and overridable settings
public sealed Configure<DotNetBuildSettings> CompileSettingsBase => _ => _
    .SetProjectFile(Solution)
    .SetConfiguration(Configuration);

public Configure<DotNetBuildSettings> CompileSettings => _ => _;

// ✅ DO — use TryDependsOn / TryBefore for optional dependencies
public Target Compile => _ => _
    .TryDependsOn<IRestore>()
    .TryDependsOn<IFormat>()
    .Executes(() => { ... });

// ❌ DON'T — tightly couple components
public interface ICompile : IRestore, IFormat, IClean { ... }  // Too many hard dependencies
```

---

## Testing

### Framework

- **xUnit** for test framework
- **FluentAssertions** for assertions
- **NSubstitute** for mocking

### Test naming convention

Use descriptive method names following the pattern:
`Given_<precondition>_When_<action>_Then_<expected result>`

```csharp
// ✅ DO
[Fact]
public void Given_DefaultImplementation_When_AccessingDirectoriesToDelete_Then_ReturnsEmptyCollection()
{
    // Arrange
    IClean sut = new CleanBuild();

    // Act
    IEnumerable<AbsolutePath> actual = sut.DirectoriesToDelete;

    // Assert
    actual.Should().BeEmpty();
}

// ❌ DON'T — vague or non-descriptive names
[Fact]
public void Test1() { ... }

[Fact]
public void CleanWorks() { ... }
```

### Test structure

Use the **Arrange / Act / Assert** pattern with comments.

```csharp
// ✅ DO
[Fact]
public void Given_DefaultImplementation_When_AccessingSourceDirectory_Then_ReturnsExpectedPath()
{
    // Arrange
    IHaveSourceDirectory sut = new SourceDirectoryBuild();

    // Act
    AbsolutePath actual = sut.SourceDirectory;

    // Assert
    actual.Should().NotBeNull();
    actual.ToString().Should().EndWith("src");
}
```

### Test build stubs

Create minimal stubs in `TestBuild.cs` for testing interface default implementations.

```csharp
// ✅ DO — minimal test stubs
internal class SourceDirectoryBuild : NukeBuild, IHaveSourceDirectory;

internal class CleanBuild : NukeBuild, IClean;

// Stubs that require non-default members implement only what's needed
internal class PackBuild : NukeBuild, IPack
{
    public IEnumerable<AbsolutePath> PackableProjects => [];
}
```

### Test file naming

Test files are named after the component they test: `I{Component}Tests.cs`

| Component file                | Test file                   |
|-------------------------------|-----------------------------|
| `IClean.cs`                   | `ICleanTests.cs`            |
| `IHaveArtifacts.cs`           | `IHaveArtifactsTests.cs`    |
| `Configuration.cs`            | `ConfigurationTests.cs`     |

---

## Package Management

This project uses **Central Package Management**. All package versions are defined in [`Directory.Packages.props`](Directory.Packages.props).

```xml
<!-- ✅ DO — add version in Directory.Packages.props -->
<PackageVersion Include="FluentAssertions" Version="8.3.0"/>

<!-- ✅ DO — reference without version in .csproj -->
<PackageReference Include="FluentAssertions"/>

<!-- ❌ DON'T — specify version in .csproj files -->
<PackageReference Include="FluentAssertions" Version="8.3.0"/>
```

---

## Versioning & Changelog

### Versioning

- Uses **[GitVersion](https://gitversion.net/)** for automatic semantic versioning.
- **Never set versions manually** in `.csproj` files or elsewhere.

### Changelog

- Follows the **[Keep a Changelog](https://keepachangelog.com/en/1.0.0/)** format.
- Update [`CHANGELOG.md`](CHANGELOG.md) with every user-facing change under the `[Unreleased]` section.
- Use the standard section headers:

| Section                  | When to use                                    |
|--------------------------|------------------------------------------------|
| `### 💥 Breaking changes`| Backward-incompatible API changes              |
| `### 🚀 New features`    | New functionality                              |
| `### 🚨 Fixes`           | Bug fixes                                      |
| `### 🧹 Housekeeping`    | Internal changes, dependency updates, tooling  |

---

## CI/CD Workflows

- GitHub workflow files (`.github/workflows/*.yml`) are **auto-generated** from `build/Pipeline.cs` via `ICanRegenerateGitHubWorkflows`.
- **Do NOT edit workflow YAML files manually.** They will be overwritten.
- To change CI behavior, modify `Pipeline.cs` and regenerate.

---

## Build Commands

```bash
# Build the solution
./build.sh compile

# Create NuGet packages
./build.sh pack

# Run all default targets
./build.sh
```

---

## Summary of Do's and Don'ts

| ✅ Do | ❌ Don't |
|-------|---------|
| Use explicit types everywhere | Use `var` |
| Use 4-space indentation | Use tabs |
| Place braces on their own line (Allman style) | Use K&R brace style |
| Always use braces for control flow | Omit braces for single-line blocks |
| Use `is null` / `is not null` | Use `== null` / `!= null` |
| Use switch expressions | Use switch statements |
| Use file-scoped namespaces | Use block-scoped namespaces (unless existing) |
| Use predefined types (`string`, `int`) | Use framework types (`String`, `Int32`) |
| Follow `IHave*` / `I{Verb}` / `IDo*` naming | Invent new interface prefix patterns |
| Put `using` directives outside namespace | Put `using` inside namespace |
| Sort `System` usings first | Use unsorted usings |
| Add package versions in `Directory.Packages.props` | Add versions in `.csproj` files |
| Update `CHANGELOG.md` for every change | Skip changelog updates |
| Name tests `Given_When_Then` | Use vague test names |
| Use Arrange/Act/Assert pattern | Mix test phases |
| Create branches from `develop` | Commit directly to `main` or `develop` |
| Let GitVersion manage versions | Set versions manually |
| Modify `Pipeline.cs` for CI changes | Edit `.github/workflows/*.yml` manually |
| 1 file per type | Multiple types in one file |
| Match namespace to folder structure | Use arbitrary namespace names |
