using BiSharper.Interoperability;

namespace BiSharper.Application;

internal class SplashExecutor : IBiToolExecutor
{
    public static readonly SplashExecutor Instance = new();
    
    public string ToolName => "Tool Selector";
    public string ToolDescription => "Used to select which tool you want to use in the toolset.";

}