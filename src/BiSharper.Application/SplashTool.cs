using BiSharper.Application.Cli;
using BiSharper.Application.Tui;
using BiSharper.ApplicationFramework;
using Terminal.Gui;

namespace BiSharper.Application;

internal sealed class SplashTool : BiSharperTool<SplashTui, SplashCli, SplashExecutor>
{
    public SplashTool(string[] arguments, ColorScheme? colorScheme = null) : base(SplashExecutor.Instance)
    {
    }
}

