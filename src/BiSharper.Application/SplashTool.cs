using BiSharper.Interoperability;
using Terminal.Gui;

namespace BiSharper.Application;

public sealed class SplashTool : BiSharperTool
{
    
    public override string ToolName => "Tool Selector";
    public override string ToolDescription => "Used to select which tool you want to use in the toolset.";
    
    public SplashTool(string[] arguments, ColorScheme? colorScheme = null) : base(true)
    {
        if (arguments.Length != 0 || arguments.Contains("-gui"))
        {
            ExecuteHeadless(arguments);
        }
        else
        {
            InitializeGui(colorScheme);
            SetupGui();
            Run();
        }
    }

    public override void ExecuteHeadless(string[] arguments)
    {
        base.ExecuteHeadless(arguments);
    }
}