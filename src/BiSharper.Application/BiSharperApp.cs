namespace BiSharper.Application;

internal static class BiSharperApp
{
    public static SplashTool? SplashScreen;

    private static void Main(string[] args) => SplashScreen = new SplashTool(args);
}