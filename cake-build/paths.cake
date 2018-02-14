public static class Paths
{
	public static FilePath SolutionFile => "CakeSample.sln";
	public static FilePath ProjectDirectory => "./src/Site/";
	public static FilePath TestProjectDirectory => "./src/Site.Tests/";
}

public static FilePath Combine(DirectoryPath directory, FilePath file)
{
	return directory.CombineWithFilePath(file);
}