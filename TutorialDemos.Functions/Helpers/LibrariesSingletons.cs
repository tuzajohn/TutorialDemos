namespace TutorialDemos.Functions.Helpers;

public static class LibrariesSingletons
{
    public static Logger Logger { get; set; } = new LoggerConfiguration().WriteTo.Logentries("77d4527f-e9ee-4cad-80c4-a91b3d2a80d2").CreateLogger();
}
