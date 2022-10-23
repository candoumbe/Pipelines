# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Breaking changes
- Renamed `IBenchmarks` to `IBenchmark`
- Renamed `IMutationTests` to `IMutationTest`

### New features

- Added execution of `IPublish.Publish` target on `integration` workflow
- Added `IHaveReport` interface that can be used by pipelines that output reports of any kind (code coverage, performance tests, ...)
- Added `IUnitTest.UnitTestsResultsDirectory` which defines where to output unit test result files
- Added `IMutationTest.MutationTestResultsDirectory` which defines where to output mutation test result files
- Added `IBenchmark.BenchmarkTestResultsDirectory` which defines where to output benchmarks test result files

### Fixes


## [0.1.0] / 2022-10-23
- Initial release

[Unreleased]: https://github.com/candoumbe/Pipelines/compare/0.1.0...HEAD
[0.1.0]: https://github.com/candoumbe/Pipelines/tree/0.1.0

