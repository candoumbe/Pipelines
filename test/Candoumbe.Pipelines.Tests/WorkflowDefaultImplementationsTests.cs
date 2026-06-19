using System;
using System.Linq;
using System.Reflection;
using Candoumbe.Pipelines.Components;
using Candoumbe.Pipelines.Components.Workflows;
using FluentAssertions;
using Xunit;

namespace Candoumbe.Pipelines.Tests;

public class WorkflowDefaultImplementationsTests
{
    [Theory]
    [InlineData(typeof(IGitFlow), "IDoHotfixWorkflow.FinishHotfix")]
    [InlineData(typeof(IGitFlow), "IDoFeatureWorkflow.FinishFeature")]
    [InlineData(typeof(IGitHubFlow), "IDoHotfixWorkflow.FinishHotfix")]
    [InlineData(typeof(IGitHubFlow), "IDoFeatureWorkflow.FinishFeature")]
    [InlineData(typeof(IGitHubFlow), "IDoChoreWorkflow.FinishChore")]
    public void Given_WorkflowInterface_When_InspectingDefaultMethod_Then_MethodHasImplementation(Type workflowType, string methodSuffix)
    {
        // Act
        MethodInfo method = workflowType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .SingleOrDefault(candidate => candidate.Name.EndsWith(methodSuffix, StringComparison.Ordinal));

        // Assert
        method.Should().NotBeNull($"{workflowType.Name} should implement '{methodSuffix}' as a default interface method");
        method!.GetMethodBody().Should().NotBeNull();
    }
}
