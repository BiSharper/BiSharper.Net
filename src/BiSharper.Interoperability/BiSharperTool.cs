using Terminal.Gui;

namespace BiSharper.Interoperability;

public abstract class BiSharperTool : IDisposable
{
    private bool _disposed;
    private readonly bool _closeApplication;
    public Window? ToolWindow { get; protected set; }
    public abstract string ToolName { get; }
    public abstract string ToolDescription { get; }

    protected BiSharperTool(bool closeApplication = false)
    {
        _closeApplication = closeApplication;
    }
    
    public override string ToString() => $"{ToolName,-16} ({ToolDescription})";

    public virtual void InitializeGui(ColorScheme? colorScheme)
    {
        Application.Init();

        ToolWindow = new Window
        {
            Title = $"BiSharper: {ToolName}",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            ColorScheme = colorScheme
        };

        ToolWindow.Closed += OnToolWindowClosed;

        Application.Top.Add(ToolWindow);
    }

    private void OnToolWindowClosed(object? sender, ToplevelEventArgs e)
    {
        Dispose(true);
    }

    public virtual void ExecuteHeadless(string[] arguments) => throw new NotSupportedException();
    
    public virtual void RequestStop() => Application.RequestStop();

    public virtual void Run() => Application.Run(Application.Top);

    protected virtual void SetupGui()
    {
        
    }
    
    protected virtual void ReleaseUnmanagedResources()
    {
    }
    
    protected virtual void ReleaseManagedResources()
    {
        if(_closeApplication) Application.Shutdown();
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;
            ReleaseUnmanagedResources();
            if (disposing) ReleaseManagedResources();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~BiSharperTool()
    {
        Dispose(false);
    }
}