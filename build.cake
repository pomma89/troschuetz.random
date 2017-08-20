#tool nuget:?package=NUnit.ConsoleRunner&version=3.7.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

private string SolutionFile() { return "./Troschuetz.Random.sln"; }
private string ArtifactsDir() { return "./artifacts"; }
private string MSBuildLinuxPath() { return @"/usr/lib/mono/msbuild/15.0/bin/MSBuild.dll"; }

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(ArtifactsDir());
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    Restore();
});

Task("Build-Debug")
    .IsDependentOn("Restore")
    .Does(() => 
{
    Build("Debug");
});

Task("Build-Release")
    .IsDependentOn("Build-Debug")
    .Does(() => 
{
    Build("Release");
});

Task("Pack-Release")
    .IsDependentOn("Build-Release")
    .Does(() => 
{
    Pack("Release");
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Pack-Release");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

//////////////////////////////////////////////////////////////////////
// HELPERS
//////////////////////////////////////////////////////////////////////

private void Restore()
{
    //DotNetCoreRestore();

    MSBuild(SolutionFile(), settings =>
    {
        settings.SetMaxCpuCount(0);
        settings.SetVerbosity(Verbosity.Quiet);
        settings.WithTarget("restore");
        if (!IsRunningOnWindows())
        { 
            // Hack for Linux bug - Missing MSBuild path.
            settings.ToolPath = new FilePath(MSBuildLinuxPath());
        }
    });
}

private void Build(string cfg)
{
    //DotNetCoreBuild(SolutionFile(), new DotNetCoreBuildSettings
    //{
    //    Configuration = cfg,
    //    NoIncremental = true
    //});

    MSBuild(SolutionFile(), settings =>
    {
        settings.SetConfiguration(cfg);
        settings.SetMaxCpuCount(0);
        settings.SetVerbosity(Verbosity.Quiet);
        if (!IsRunningOnWindows())
        { 
            // Hack for Linux bug - Missing MSBuild path.
            settings.ToolPath = new FilePath(MSBuildLinuxPath());
        }
    });
}

private void Pack(string cfg)
{
    Parallel.ForEach(GetFiles("./src/**/*.csproj"), project =>
    {
        //DotNetCorePack(project.FullPath, new DotNetCorePackSettings
        //{
        //    Configuration = cfg,
        //    OutputDirectory = ArtifactsDir(),
        //    NoBuild = true,
        //    IncludeSource = true,
        //    IncludeSymbols = true
        //});

        MSBuild(project, settings =>
        {
            settings.SetConfiguration(cfg);
            settings.SetMaxCpuCount(0);
            settings.SetVerbosity(Verbosity.Quiet);
            settings.WithTarget("pack");
            settings.WithProperty("IncludeSource", new[] { "true" });
            settings.WithProperty("IncludeSymbols", new[] { "true" });
            if (!IsRunningOnWindows())
            { 
                // Hack for Linux bug - Missing MSBuild path.
                settings.ToolPath = new FilePath(MSBuildLinuxPath());
            }
        });

        var packDir = project.GetDirectory().Combine("bin").Combine(cfg);
        MoveFiles(GetFiles(packDir + "/*.nupkg"), ArtifactsDir());
    });
}