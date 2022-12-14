# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Breaking changes
- Renamed `IBenchmarks` to `IBenchmark`
- Renamed `IMutationTests` to `IMutationTest`
- Moved `IGitFlow` to `Candoumbe.Pipelines.Components.Workflows` namespace
- Made `IGitFlow.FinishFeature` async
- Made `IGitFlow.FinishReleaseOrHotfix` async
- Made `IGitFlow.FinishColdfix` async
- Moved `GitHubPublishConfiguration` to `Candoumbe.Pipelines.Components.GitHub` namespace
- Moved `ICreateGitHubRelease` to `Candoumbe.Pipelines.Components.GitHub` namespace
- Made `IPublish.PublishConfigurations` mandatory

### New features
- Added `IGithubFlowWithPullRequest`
- Added `IGitFlowWithPullRequest`
- Added `IPullRequest` component which extends `IWorkflow` and create pull requests instead or merging back to `develop` (respectiveley `main`) when finishing a feature / coldfix (resp. release / hotfix) branch.
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

### Fixes

- Fixed directory path used by `IUnitTest` target to output unit tests results.
- Fixed argument format used to define reporters used when running mutation tests.

## [0.1.0] / 2022-10-23
- Initial release

[Unreleased]: https://github.com/candoumbe/Pipelines/compare/0.1.0...HEAD
[0.1.0]: https://github.com/candoumbe/Pipelines/tree/0.1.0

