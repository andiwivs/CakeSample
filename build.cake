#load cake-build/paths.cake

var target = Argument<string>("Target", "Build");
var configuration = Argument<string>("Configuration", "Release");

var outputDir = Argument<string>("OutputDir", "./artifacts/");
var releasePackageOutputPath = Argument<string>("releasePackageOutputPath", "./packages/");

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

Task("Remove-Artifacts")
	.Does(() =>
	{
		CleanDirectory(outputDir);

		Information("Cleaned " + outputDir);
	});

Task("Remove-Release-Packages")
	.Does(() =>
	{
		CleanDirectory(releasePackageOutputPath);

		Information("Cleaned " + releasePackageOutputPath);
	});

/* THIS DOES NOT WORK :(
Task("Package-NuGet")
	//.IsDependentOn("Build")
	//.IsDependentOn("Test")
	.IsDependentOn("Remove-Release-Packages")
	.Does(() =>
	{
		DotNetCorePack(Paths.ProjectDirectory.ToString(), new DotNetCorePackSettings
		{
			Configuration = configuration,
			Id = "Site",
			Version = "0.1.0",
			Description = "test",
			Authors = new[] {"anders"},
			OutputDirectory = releasePackageOutputPath
		});

		Information("Packaged " + Paths.ProjectDirectory.ToString());
	});
*/

Task("Package-Zip")
	.IsDependentOn("Test")
	.IsDependentOn("Remove-Artifacts")
	.IsDependentOn("Remove-Release-Packages")
	.Does(() =>
	{
		Information("Publishing...");
		DotNetCorePublish(Paths.ProjectDirectory.ToString(), new DotNetCorePublishSettings
		{
			Configuration = configuration,
			OutputDirectory = outputDir
		});
		
		Information("Compressing " + outputDir + "...");
		Zip(
			outputDir,
			Combine(new DirectoryPath(releasePackageOutputPath), new FilePath("Site.zip"))
		);
	});

RunTarget(target);