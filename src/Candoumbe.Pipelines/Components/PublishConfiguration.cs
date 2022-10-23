using System;

namespace Candoumbe.Pipelines;

/// <summary>
/// Wraps configuration used to publish packages
/// </summary>
/// <param name="Name">Name of the configuration. Should uniquely identifies a configuration and </param>
/// <param name="Key">API key used to interact with the API</param>
/// <param name="Source"><see langword="Uri"/> ot the package repository</param>
/// <param name="CanBeUsed">Lazily indicates if the configuration can be used.</param>
public abstract record PublishConfiguration(string Name, string Key, Uri Source, Func<bool> CanBeUsed);
