using System;

namespace Candoumbe.Pipelines.Components.NuGet;

/// <summary>
/// Wraps configuration used to push nuget packages
/// </summary>
/// <param name="Name">Name of the configuration. Should uniquely identifies a configuration and </param>
/// <param name="Key">API key used to interact with the API</param>
/// <param name="Source">Source to the package repository</param>
/// <param name="CanBeUsed">Lazily check if the current configuration can be used.</param>
public abstract record PushNugetPackageConfiguration(string Name, string Key, string Source, Func<bool> CanBeUsed);
