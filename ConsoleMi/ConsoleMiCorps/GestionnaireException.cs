using System.ComponentModel;

namespace ConsoleMi.ConsoleMiCorps;

// Type Exceptions

public struct EnregistreException(Exception exception)
{
    public Exception Exception { get; } = exception;
    public DateTime DateTimes { get; } = DateTime.Now;
}
public struct EnregistreWarning(WarningException warning)
{
    public WarningException Warning { get; } = warning;
    public DateTime DateTimes { get; } = DateTime.Now;
}

internal class ExceptionsGeree
{
    private List<EnregistreException> Exceptions{get;set;} = new();
    public void Add(Exception exception)
    {
        EnregistreException except = new(exception);
        Exceptions.Add(except);
    }
    public void ViewException()
    {
        foreach (var except in Exceptions)
        {
            Console.WriteLine(except.Exception.HResult + " : " + except.Exception.Message);
            Console.WriteLine("At time : " + except.DateTimes);
            
        }
        Exceptions = new();
    }
}

public class GestionnaireException
{
    public delegate void VoidFunc(Exception e);
    public event VoidFunc? InformLogging;
    private ExceptionsGeree Excepts = new();
    public  void Add(Exception e)
    {
        Excepts.Add(e);
        InformLogging?.Invoke(e);
    }
    public  void ViewException(bool remove = false)
    {
        Excepts.ViewException();
    }
}

