using System;
using Nuke.Common;
using Nuke.Common.ProjectModel;

namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Extension method
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Performs a ternary
    /// </summary>
    /// <typeparam name="T">Type of the default object returned if <paramref name="obj"/> is <see langword="null"/>.</typeparam>
    /// <typeparam name="TObject">Type of the object under test.</typeparam>
    /// <param name="settings"></param>
    /// <param name="obj">The object under test</param>
    /// <param name="configurator">a function that can extract <typeparamref name="T"/> from <paramref name="obj"/></param>
    /// <returns>
    /// The result of <paramref name="configurator"/> invocation when <paramref name="obj"/> is <see langword="null"/>
    /// or <paramref name="settings"/> otherwise.
    /// </returns>
    public static T WhenNotNull<T, TObject>(this T settings, TObject obj, Func<T, TObject, T> configurator)
        => obj is not null ? configurator.Invoke(settings, obj) : settings;

    /// <summary>
    /// Gives access to <typeparamref name="T"/> properties and methods
    /// </summary>
    /// <typeparam name="T">the type that the current instance should be casted into.</typeparam>
    /// <exception cref="InvalidCastException">if <paramref name="nukeBuild"/> is not convertible to <typeparamref name="T"/>.</exception>
    public static T Get<T>(this INukeBuild nukeBuild) where T : INukeBuild
        => (T)(object)nukeBuild;

    /// <summary>
    /// Uses various heuristics to tests if <paramref name="project"/> has <see href="https://github.com/dotnet/sourcelink">SourceLink</see>
    /// enabled .
    /// </summary>
    /// <param name="project"></param>
    /// <returns><see langword="true"/> if <see href="https://github.com/dotnet/sourcelink">SourceLink</see>
    /// enabled and <see langword="false"/> otherwise</returns>
#pragma warning disable RCS1175 // Unused 'this' parameter
    public static bool IsSourceLinkEnabled(this Project project) => false;
#pragma warning restore RCS1175 // Unused 'this' parameter
}