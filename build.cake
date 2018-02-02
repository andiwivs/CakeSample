#load cake-build/paths.cake

var target = Argument<string>("Target", "Build");
var configuration = Argument<string>("Configuration", "Release");
var outputDir = Argument<string>("OutputDir", "./artifacts/");

Task("Restore")
	.Does(() =>
	{
		DotNetCoreRestore();
	});

Task("Build")
	.IsDependentOn("Restore")
	.Does(() =>
	{
		DotNetCoreBuild(Paths.SolutionFile.ToString(), new DotNetCoreBuildSettings
		{
			Configuration = configuration,
			OutputDirectory = outputDir
		});
	});

Task("Test")
	.IsDependentOn("Restore")	// for .net core, "build" is baked in to test runner, we just need to ensure packages have been restored
	.Does(() =>
	{
		DotNetCoreTest(Paths.TestProjectDirectory.ToString());
	});

RunTarget(target);