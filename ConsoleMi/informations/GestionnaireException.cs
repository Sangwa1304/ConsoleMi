namespace ConsoleMi.Informations;

// Type Exceptions

public struct EnregistreException(Exception exception)
{
    public Exception Exceptions { get; } = exception;
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
            Console.WriteLine(except.Exceptions.HResult + " : " + except.Exceptions.Message);
            Console.WriteLine("At time : " + except.DateTimes);
            
        }
        Exceptions = new();
    }
}

public static class GestionnaireException
{
    private static ExceptionsGeree Excepts = new();
    public static void Add(Exception e)
    {
        Excepts.Add(e);
    }
    public static void ViewException(bool remove = false)
    {
        Excepts.ViewException();
    }
}

