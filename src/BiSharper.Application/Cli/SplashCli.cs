using BiSharper.Interoperability.Cli;
using Terminal.Gui;

namespace BiSharper.Application.Cli;

internal sealed class SplashCli : BiSharperCli<SplashExecutor>
{
    public SplashCli(string[] arguments, SplashExecutor tool, ColorScheme? colorScheme) : base(arguments, tool, colorScheme)
    {
    }
}