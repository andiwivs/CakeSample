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
		DotNetCoreBuild("CakeSample.sln", new DotNetCoreBuildSettings
		{
			Configuration = configuration,
			OutputDirectory = outputDir
		});
	});

RunTarget(target);