# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### 🚀 New features
- Added `Filter` option to specifiy which benchmarks to perform
- Added `NuGetApiKey` parameter to `IPushNugetPackages` component


## [0.13.4] / 2025-08-18
### 🛠️ Fixes
- Fixed incorrect rendering of `IDotNetFormat` command line arguments.

## [0.13.3] / 2025-07-28
### 🛠️ Fixes
- Removed  redundant dependency on `IIntegrationTest` default component which caused circular dependency.
- Fixed incorrect `--with-baseline` option when running `IMutationTest` component from a `hotfix/*` branch in a GitFlow workflow.
- Run `IBenchmark.Benchmarks` target in `Configuration.Release` by default

### 🚀 New features
- Added `HTML` reporter by default when running `IMutation.MutationTests` target locally.


## [0.13.2] / 2025-04-13
### 🛠️ Fixes
- Fixed artifact names collision for code coverage and code coverage history.

### 🚨 Breaking changes
- `IReportCoverage` component no longer provide any target for reporting code coverage. Instead, you should :
  - use [`IReportUnitTestCoverage`](./src/Candoumbe.Pipelines/Components/IReportUnitTestCoverage.cs) component for reporting unit tests coverage
  - use [`IReportIntegrationTestCoverage`](./src/Candoumbe.Pipelines/Components/IReportIntegrationTestCoverage.cs) component for reporting integration tests coverage

## [0.13.1] / 2025-04-12
### 🛠️ Fixes

- Fixed the incorrect version number when creating a hotfix branch ([#96](https://github.com/candoumbe/Pipelines/issues/96))
- Replaced the commit number with the branch name when submitting versioned mutation test reports ([#183](https://github.com/candoumbe/Pipelines/issues/183))

## [0.13.0] / 2025-04-11
### 🚀 New features

- Added `IIntegrationTest` component that can run integration tests ([#173](https://github.com/candoumbe/Pipelines/issues/173))
- Added `IReportUnitTestCodeCoverage` component
- Added `IReportIntegrationTestCodeCoverage` component
- Added `IRestore.RestoreToolSettings` to configure the behavior of `dotnet tool restore` command.
- Added "native" stryker tool support

### 🚨 Breaking changes
- Dropped `net6.0` support
- Separated directories for code coverage and coverage history between unit and integration tests.
- `IPushNuGetPackages` component no longer add `--skip-duplicate` option by default.
- Replaced `Configure<DotNetRunSettings> StrykerArgumentSettings` is now `Configure<StrykerSettings>`

### 🛠️ Fixes
- `IBuildDockerImage` component should run AFTER `IUnitTest`, `IIntegrationTest` and `IMutationTest` components.

## [0.12.1] / 2024-11-12
### 🛠️ Fixes
- Fixed incorrect verbosity of the default implementation of the `IUnitTest` component 

## [0.11.0] / 2024-09-14
### 🧹 Housekeeping
- Bumped `Nuke.Common` to 8.1.0 and updated build scripts.
- Bumped required SDK to 8.0.401
- Bumped `Gitversion.Tool` to 6.0.2
- Bumped `Roslynator.Analyzers` to 4.12.5
- Replaced `NuGetVersion` usage with `SemVer` variable as nuget package version number 

## [0.10.0] / 2024-07-11
### 🚀 New features

- Enabled `TerminalLogger` ([#138](https://github.com/candoumbe/Pipelines/issues/138))
- Added a new `--skip-confirmation` option to skip asking confirmations ([#145](https://github.com/candoumbe/Pipelines/issues/145)).
This can improve usability in non-interactive environments.
- Introduced [`EnhancedNukeBuild`] that can be used as drop-in replacement of the `NukeBuild`.
[`EnhancedNukeBuild`] class adds support for some options to the `NukeBuild` such as the new `--skip-confirmation` option.

### 🛠️ Fixes

- Fixed `Nuke.Common` dependency used when targeting `net6.0`

### 🧹 Housekeeping

- Added `$schema` property for better completion in renovate configuration file. 

## [0.9.0] / 2024-01-25
### 🚀 New features

- Added `IDotnetFormat` component that format code using `dotnet-format` tool

### 💪🏾 Miscellaneous

- Enhanced the way titles of pull requests are computed.

### 🧹 Housekeeping

- Update Nuke to 8.0.0

## [0.8.0] / 2023-12-15
### 🚨 Breaking changes

- Dropped `IHaveSecret` component ([#94](https://github.com/candoumbe/Pipelines/issues/94))

### 🧹 Housekeeping

- Adopted the new C#12 syntax
- Added support for net8.0
- Replaced `<Compile .../>` tags in the build `csproj` file with a regular project dependency on `Candoumbe.Pipelines` ([#125](https://github.com/candoumbe/Pipelines/issues/125)).

## [0.7.0] / 2023-09-23
### 🚀 New features

- Added `IDoHotfixWorkflow` component which represents the `hotfix` workflow
- Added `IDoFeatureWorkflow` component which represents the `feature` workflow
- Added `IDoColdfixWorkflow` component which represents the `coldfix` workflow
- `ICreateGithubRelease.Assets` property can be used to specify which artifacts to associate with a GitHub release ([#103](https://github.com/candoumbe/Pipelines/issues/103))
- `IMutationTest` component can send `--config-file` option to Stryker CLI ([#90](https://github.com/candoumbe/Pipelines/issues/90))

### 🚨 Breaking changes

- Removed `Github.IPullRequest.Token` property. This property was previously used by `GitHub.IGitFlowWithPullRequest` and `GitHub.IGitFlowWithPullRequest` when finishing a feature/coldfix is.
- Refactored `IWorkflow` component and removed inheritance from `IHaveMainBranch` component
- Refactored `ICompile` component to no longer extend `IRestore` component
- Refactored `IPack` component to no longer extend `ICompile` component
- Refactored `IMutationTest.MutationTestsProjects` type from `(Project SourceProject, IEnumerable<Project> TestsProjects)` to [`MutationTestProjectConfiguration`](./src/Candoumbe.Pipelines/Components/IMutationTest.cs):
this new type allows to specify the path to the configuration file to use during each mutation test run.
- `IMutationTest` component no longer implements `IUnitTest` but only `IHaveTests`

### 🛠️ Fixes

- Removed `nofetch` option (when calling `gitversion` tool) in order to compute semver version number more accurately ([#96](https://github.com/candoumbe/Pipelines/issues/96)).
- `IMutationTest.MutationTests` output the value of the `--project` parameter **with** the filename extension ([#109](https://github.com/candoumbe/Pipelines/issues/109))
- Marked `IMutationTest.StrykerDashboardApiKey` property as a secret ([#110](https://github.com/candoumbe/Pipelines/issues/110)).

### 🧹 Housekeeping

- Refactoring of `IMutationTest` component to improve maintainability : the way CLI options required by Stryker are computed is now centralized.
- Added `GitHubToken` value in `parameters.local.json` : this value will be consumed directly to interact with the GitHub repository.
- `IGitflow`, `IGithubflow` components extends `IDoHotfixWorkflow` component
- `IGitflow`, `IGithubflow` components extends `IDoHotfixWorkflow` component
- Added `NugetApi` value in `parameters.local.json` to interact directly with NuGet from local environment
- Added documentation for `IMutationTest` classes
- Deprecated `IHaveSecret` component ([#94](https://github.com/candoumbe/Pipelines/issues/94))

## [0.6.1] / 2023-08-31
### 🔧 Fixes
- Fixed the way arguments sent to nuke CLI are rendered when calling `IHaveSecret.ManageSecrets` target.

## [0.6.0] / 2023-08-15
### 🚀 New features
- Added CLI options to push mutation reports to stryker dashboard when possible.

### 🔧 Fixes
- Reduced the number of CLI options sent when `IUnitTest.UnitTests` target runs and the current build does not need to collect code coverage.
- Made `IMutationTest.MutationTests` target to run before `IPack.Pack` does.
- Added CLI option `--project` to the options output when running mutation tests
- Changed output directory for mutation report files to save them into a directory named after the source project that was mutated.
So now `{MutationTestDirectory}/[{framework}]` is now changed to `{MutationTestDirectory}/{ProjectName}/[{framework}]` ([#88](https://github.com/candoumbe/pipelines/issues/88))
- Fixed the name of the parameters emitted for the [project version](https://stryker-mutator.io/docs/stryker-net/configuration/#project-infoversion-committish) when using `dashboard` reporter  ([#89](https://github.com/candoumbe/pipelines/issues/89))
- Removed `--dashboard.module` parameter as it cannot be defined at CLI level with Stryker version 3.10.0 ([#89](https://github.com/candoumbe/pipelines/issues/89))


## [0.5.0] / 2023-07-24
### 🚀 New features
- Added `ConfigName` parameter to specify the name of the configuration to use when pushing nuget packages ([#37](https://github.com/candoumbe/Pipelines/issues/37)).
- Added [`--with-baseline`](https://stryker-mutator.io/docs/stryker-net/configuration/#baseline) and [`--version`](https://stryker-mutator.io/docs/stryker-net/configuration/#project-infoversion-committish) arguments to run mutation tests with Stryker faster
- Added `--reporter dashboard` option to the CLI generated when running `IMutationTests.MutationTests` with `StrykerDashboardApiKey` is not null.
- Added nuget package requirement for `IHaveGitVersion` component ([#77](https://github.com/candoumbe/pipelines/issues/77))
- Added nuget package requirement for `IReportCoverage` component

### 🔧 Fixes
- Changed requirements of `IPushNugetPackages.Publish` target to make it runnable locally

### 🧹 Housekeeping
- Updated `Candoumbe.MiscUtilities` to `0.11.1`.
- Added configuration file for better computation of artifact semver

## [0.4.5] / 2023-07-17
### 🔧 Fixes
- Reverted changes made to output tests result in a folder that is named after the current branch (if any).


## [0.4.4] / 2023-07-15
### 🔧 Fixes
- Fixed prompt to set the title of the PR to accept spaces ([#74](https://github.com/candoumbe/Pipelines/issues/74))

## [0.4.3] / 2023-07-10
### 🔧 Fixes
- Fixed `NullReferenceException` thrown when calling `IMutationTest.MutationTests` target ([#69](https://github.com/candoumbe/Pipelines/issues/69))

## [0.4.2] / 2023-07-05
### 🔧 Fixes
- Default implementation of `IWorkflow.Hotfix` does not always compute the version number of the hotfix branch accurately.


## [0.4.1] / 2023-07-05
### 🚀 New features
- Default implementation of `IPushNugetPackages.Publish` publishes nuget packages when on a `hotfix/*` branch

### 🔧 Fixes
- `IUnitTest.UnitTests` and `IMutationTest.MutationTests` target publish test results to a folder named after the current branch in the artifacts

## [0.4.0] / 2023-07-03
### 🚀 New features
- Added `Docker.IBuildDockerImage` component
- Added `IPushDockerImage` component
- Added `PushDockerImageConfiguration`
- Added `DockerFile` class

### ⚠️ Breaking changes
- Renamed `Candoumbe.Pipelines.IPublish` to `Candoumbe.Pipelines.IPushNugetPackages`
- Renamed `Candoumbe.Pipelines.GitHub.GitHubPublishConfiguration` to `Candoumbe.Pipelines.NuGet.GitHubPushNugetConfiguration`
- Renamed `Candoumbe.Pipelines.GitHub.NugetPublishConfiguration` to `Candoumbe.Pipelines.NuGet.GitHubPushNugetConfiguration`
- `IMutationTest.MutationTests` target can now output to separate folder when unit tests targets at least 2 distinct frameworks.
- Changed `PushNugetConfiguration.Source` type from `Uri` to `string`

### 🧹 Housekeeping
- Dropped explicit `net7.0` framework support

## [0.3.0] / 2023-02-05
### 🔧 Fixes
- Added `EnableNoSymbols` switch on `IPublish.Publish` target
- No more pull request for finishing a `hotfix/*` or  `release/*` branches when using IGitFlowPullRequest or IGithubFlowPullRequest.
- Missing `Source` parameter when running `IPublish.Publish` target ([#46](https://github.com/candoumbe/Pipelines/issues/46)).
- `IPublish.Publish` no longer publishes the `develop` branch

### 🧹 Housekeeping
- Bumped `Candoumbe.Miscutilities` from `0.10.0` to `0.11.0`

## [0.2.0] / 2023-01-20
### 🚀 New features
- Added `IGithubFlowWithPullRequest`
- Added `IGitFlowWithPullRequest`
- Added `IPullRequest` component which extends `IWorkflow` and create pull requests instead or merging back to `develop` (respectively `main`) when finishing a feature / coldfix (resp. release / hotfix) branch.
- Added `IGitHubFlow` ([#15](https://github.com/candoumbe/pipelines/issues/15))
- Added `IPullRequest.Issues` parameter which allows to specify issues a pull request fixes ([#9](https://github.com/candoumbe/pipelines/issues/9))
- Added execution of `IPublish.Publish` target on `integration` workflow
- Added `IHaveReport` component that can be used by pipelines that output reports of any kind (code coverage, performance tests, ...)
- Added `IUnitTest.UnitTestsResultsDirectory` which defines where to output unit test result files
- Added `IUnitTest.ProjectUnitTestSettings` to customize/override the unit tests settings.
- Added `IMutationTest.MutationTestResultsDirectory` which defines where to output mutation test result files
- Added `IBenchmark.BenchmarkTestResultsDirectory` which defines where to output benchmarks test result files
- Added `IHaveGitHubRepository` which extends `IHaveGitRepository` and specific properties related to GitHub repositories.
- Promoted `IPullRequest.DeleteLocalOnSuccess` as parameter
- Promoted `IPullRequest.Draft` as parameter
- Newly created pull request open in the default browser after creation ([#10](https://github.com/candoumbe/pipelines/issues/10))
- Changed `IWorkflow.Changelog` target to autocommit changes when running on a build server ([#39](https://github.com/candoumbe/pipelines/issues/39))

### ⚠️ Breaking changes
- Renamed `IBenchmarks` to `IBenchmark`
- Renamed `IMutationTests` to `IMutationTest`
- Moved `IGitFlow` to `Candoumbe.Pipelines.Components.Workflows` namespace
- Made `IGitFlow.FinishFeature` async
- Made `IGitFlow.FinishReleaseOrHotfix` async
- Made `IGitFlow.FinishColdfix` async
- Moved `GitHubPublishConfiguration` to `Candoumbe.Pipelines.Components.GitHub` namespace
- Moved `ICreateGitHubRelease` to `Candoumbe.Pipelines.Components.GitHub` namespace
- Made `IPublish.PublishConfigurations` mandatory

### 🔧 Fixes
- Fixed directory path used by `IUnitTest` target to output unit tests results.
- Fixed argument format used to define reporters used when running mutation tests.
- Fixed `SourceName` not displayed when running `IPublish.Publish` target

## [0.1.0] / 2022-10-23
- Initial release

[Unreleased]: https://github.com/candoumbe/Pipelines/compare/0.13.4...HEAD
[0.13.4]: https://github.com/candoumbe/Pipelines/compare/0.13.3...0.13.4
[0.13.3]: https://github.com/candoumbe/Pipelines/compare/0.13.2...0.13.3
[0.13.2]: https://github.com/candoumbe/Pipelines/compare/0.13.1...0.13.2
[0.13.1]: https://github.com/candoumbe/Pipelines/compare/0.13.0...0.13.1
[0.13.0]: https://github.com/candoumbe/Pipelines/compare/0.12.1...0.13.0
[0.12.1]: https://github.com/candoumbe/Pipelines/compare/0.11.0...0.12.1
[0.11.0]: https://github.com/candoumbe/Pipelines/compare/0.10.0...0.11.0
[0.10.0]: https://github.com/candoumbe/Pipelines/compare/0.9.0...0.10.0
[0.9.0]: https://github.com/candoumbe/Pipelines/compare/0.8.0...0.9.0
[0.8.0]: https://github.com/candoumbe/Pipelines/compare/0.7.0...0.8.0
[0.7.0]: https://github.com/candoumbe/Pipelines/compare/0.6.1...0.7.0
[0.6.1]: https://github.com/candoumbe/Pipelines/compare/0.6.0...0.6.1
[0.6.0]: https://github.com/candoumbe/Pipelines/compare/0.5.0...0.6.0
[0.5.0]: https://github.com/candoumbe/Pipelines/compare/0.4.5...0.5.0
[0.4.5]: https://github.com/candoumbe/Pipelines/compare/0.4.4...0.4.5
[0.4.4]: https://github.com/candoumbe/Pipelines/compare/0.4.3...0.4.4
[0.4.3]: https://github.com/candoumbe/Pipelines/compare/0.4.2...0.4.3
[0.4.2]: https://github.com/candoumbe/Pipelines/compare/0.4.1...0.4.2
[0.4.1]: https://github.com/candoumbe/Pipelines/compare/0.4.0...0.4.1
[0.4.0]: https://github.com/candoumbe/Pipelines/compare/0.3.0...0.4.0
[0.3.0]: https://github.com/candoumbe/Pipelines/compare/0.2.0...0.3.0
[0.2.0]: https://github.com/candoumbe/Pipelines/compare/0.1.0...0.2.0
[0.1.0]: https://github.com/candoumbe/Pipelines/tree/0.1.0