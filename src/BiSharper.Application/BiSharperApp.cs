// See https://aka.ms/new-console-template for more information

using System.Text;

namespace BiSharper.Application;

static class BiSharperApp
{
    public static SplashTool? SplashScreen;

    private static void Main(string[] args) => SplashScreen = new SplashTool(args);
}