using BiSharper.ApplicationFramework.Tui;
using Terminal.Gui;

namespace BiSharper.Application.Tui;

internal sealed class SplashTui : BiSharperTui<SplashExecutor>
{
    public SplashTui(string[] arguments, SplashExecutor tool, ColorScheme? colorScheme) : base(arguments, tool, colorScheme)
    {
    }
}