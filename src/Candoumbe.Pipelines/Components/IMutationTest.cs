namespace Candoumbe.Pipelines.Components;

/// <summary>
/// Marks a pipeline that can perform mutation tests using <see href="https://stryker-mutator.io/">Stryker</see>.
/// </summary>
/// <remarks>
/// <see cref="MutationTests"/> target required <see href="https://www.nuget.org/packages/dotnet-stryker">Stryker dotnet tool</see> to be referenced in order to run.
/// </remarks>
public interface IMutationTest : IUnitTest
{
    /// <summary>
    /// Directory where mutattion test results should be published
    /// </summary>
    AbsolutePath MutationTestResultDirectory => TestResultDirectory / "mutation-tests";

    /// <summary>
    /// Api Key us
    /// </summary>
    [Parameter]
    public string StrykerDashboardApiKey => TryGetValue(() => StrykerDashboardApiKey);

    /// <summary>
    /// Defines projects onto which mutation tests will be performed
    /// </summary>
    /// <remarks>
    /// The source project has
    /// </remarks>
    IEnumerable<(Project SourceProject, IEnumerable<Project> TestProjects)> MutationTestsProjects { get; }

    /// <summary>
    /// Executes mutation tests for the specified <see cref="MutationTestsProjects"/>.
    /// </summary>
    /// <remarks></remarks>
    public Target MutationTests => _ => _
        .Description("Runs mutation tests using Stryker tool")
        .TryDependsOn<IClean>(x => x.Clean)
        .DependsOn(Compile)
        .Produces(MutationTestResultDirectory / "*")
        .Executes(() =>
        {
            int mutationProjectCount = 0;

            // List of frameworks that source project are tested against
            string[] frameworks = MutationTestsProjects.Select(csprojAndTest => csprojAndTest.TestProjects)
                                                       .SelectMany(projects => projects)
                                                       .Select(csproj => csproj.GetTargetFrameworks())
                                                       .SelectMany(x => x)
                                                       .Select(targetFramework => targetFramework.ToLower().Trim())
                                                       .AsParallel()
                                                       .Distinct()
                                                       .ToArray();

            if (frameworks.AtLeast(2))
            {
                MutationTestsProjects.ForEach(tuple =>
                {
                    mutationProjectCount++;
                    (Project sourceProject, IEnumerable<Project> testsProjects) = tuple;
                    IReadOnlyCollection<string> testedFrameworks = testsProjects.Select(csproj => csproj.GetTargetFrameworks())
                                                                                   .SelectMany(x => x)
                                                                                   .Distinct()
                                                                                   .ToArray();
                    Arguments args;
                    if (testedFrameworks.AtLeast(2))
                    {
                        testedFrameworks.ForEach(framework =>
                        {
                            args = new();
                            args.Apply(StrykerArgumentsSettingsBase)
                                .Apply(StrykerArgumentsSettings);

                            args.Add(@"--target-framework ""{0}""", framework)
                                .Add("--output {0}", this.Get<IMutationTest>().MutationTestResultDirectory / framework);

                            RunMutationTestsForTheProject(sourceProject, testsProjects, args);
                        });
                    }
                    else
                    {
                        args = new();
                        string framework = testedFrameworks.Single();
                        args.Add(@"--target-framework ""{0}""", framework)
                            .Add("--output {0}", this.Get<IMutationTest>().MutationTestResultDirectory / framework);

                        RunMutationTestsForTheProject(sourceProject, testsProjects, args);
                    }
                });
            }
            else
            {
                Arguments args = new();
                args.Apply(StrykerArgumentsSettingsBase)
                    .Apply(StrykerArgumentsSettings);

                args.Add(@"--target-framework ""{0}""", frameworks.Single())
                    .Add("--output {0}", this.Get<IMutationTest>().MutationTestResultDirectory);

                MutationTestsProjects.ForEach(tuple =>
                {
                    mutationProjectCount++;
                    RunMutationTestsForTheProject(tuple.SourceProject, tuple.TestProjects, args);
                });
            }
        });

    /// Run mutation tests for the specified project using the specified arguments
    private static void RunMutationTestsForTheProject(Project sourceProject, IEnumerable<Project> testsProjects, Arguments args)
    {
        Arguments strykerArgs = new();
        strykerArgs.Concatenate(args);

        testsProjects.ForEach(project => strykerArgs.Add(@"--test-project ""{0}""", project.Path));

        Verbose("{ProjetName} will run mutation tests for the following frameworks : {@Frameworks}", sourceProject.Name);
        DotNet($"stryker {strykerArgs.RenderForExecution()}", workingDirectory: sourceProject.Path.Parent);
    }


    internal Configure<Arguments> StrykerArgumentsSettingsBase => _ => _
           .Add("--open-report:html", IsLocalBuild)
           .Add($"--dashboard-api-key {StrykerDashboardApiKey}", IsServerBuild || StrykerDashboardApiKey is not null)
           .Add(@"--reporter ""markdown""")
           .Add(@"--reporter ""html""")
           .Add(@"--reporter ""progress""", IsLocalBuild);

    /// <summary>
    /// Configures arguments that will be used by when running Stryker tool
    /// </summary>
    Configure<Arguments> StrykerArgumentsSettings => _ => _;
}
