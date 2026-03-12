using System.Diagnostics;
using ConsoleMi.ConsoleMiCorps;
using System.Speech.Synthesis;
class ProgramMi
{
    private static ConsoleM Mi = new();
    public static string Folder{get => Mi.GetFolder;}
    public static Logging log = new();
    public static GestionnaireException Exceptions{get;private set;} = new();
    public static void Main(string[] args)
    {

        try
        {
            Console.Title = "ConsoleMi";
            Console.Clear();
            Console.ResetColor();
        }
        catch (Exception e)
        {
            Exceptions.Add(e);
        }

        string? shell;
        string arg;
        
        //Environment.Exit(2);
        while(true)
        {
            try
            {
                shell = ReadLine(Mi.Marquage);

                if(shell != null)
                {
                    if(shell.StartsWith("get "))
                    {
                        arg = Parsing(shell, "get ");
                        CommandeMi.CommandSet(arg);
                        
                    }
                    else if(shell.StartsWith("show "))
                    {
                        arg = Parsing(shell, "show ");
                        if(arg.StartsWith("select "))
                        {
                            arg = Parsing(arg, "select ");
                            
                        }
                        else if(arg.Trim() != "")
                        {
                            var file  = Path.Combine(Mi.GetFolder,arg);
                            if(Path.Exists(file))
                            {
                                var info = PathInfo.GetPathInfoString(file);
                                CommandeMi.NormalizeEcriture(info);
                            }
                        }
                    }
                    else if(shell.StartsWith("select "))
                    {
                        arg = Parsing(shell, "select ");
                    }
                }
            }
            catch (Exception e)
            {
                Exceptions.Add(e);
            }
        }
        //Exceptions.ViewException();
    }

    public static string? ReadLine(string marquage)
    {
        WriteAtColor(ConsoleColor.White,marquage);
        return ReadChar();
    }

    private static string ReadChar()
    {
        string input = "";
        int? space = 0;
        bool color = true;
        ConsoleColor col;
        while(true)
        {
            var key = Console.ReadKey(true);
            if(key.Key == ConsoleKey.Enter)
                break;
            if(key.Key == ConsoleKey.Backspace)
            {
                if (input.Length <1)
                {
                    continue;
                }
                if(space == input.Length -1)
                {
                    color = true;
                    space = 0;
                }
                input = input[..^1];
                Console.Write("\b \b");
                
                continue;
            }
            
            if(key.Key == ConsoleKey.Spacebar)
            {   
                
                if (input.Length <3)
                {
                    continue;
                }
                if(space == 0)
                {
                    space = input.Length + 1;
                    color = false;
                }
            }
        
            input += key.KeyChar;
            if(color)
            {
                col = ConsoleColor.Blue;
            }
            else
            {
                col = ConsoleColor.White;
            }
            ProgramMi.WriteAtColor(col,key.KeyChar.ToString());
        }

        Console.WriteLine();
        return input;
    }

    public static void WriteError(string text)
    {
        WriteAtColor(Console.ForegroundColor = ConsoleColor.Red,text);
    }
    public static void WriteWarning(string text)
    {
        WriteAtColor(Console.ForegroundColor = ConsoleColor.Yellow,text);
    }
    public static void WriteInfo(string text)
    {
        WriteAtColor(Console.ForegroundColor = ConsoleColor.Green,text);
    }
    public static void WriteErrorLine(string text)
    {
        WriteAtColorLine(Console.ForegroundColor = ConsoleColor.Red,text);
    }
    public static void WriteWarningLine(string text)
    {
        WriteAtColorLine(Console.ForegroundColor = ConsoleColor.Yellow,text);
    }
    public static void WriteInfoLine(string text)
    {
        WriteAtColorLine(Console.ForegroundColor = ConsoleColor.Green,text);
    }
    public static void WriteAtColor(ConsoleColor consoleColor, string text)
    {
        {
        Console.ForegroundColor = consoleColor;
        Console.Write(text);
        Console.ResetColor();
    }
    }

    public static void WriteAtColorLine(ConsoleColor consoleColor, string text)
    {
        Console.ForegroundColor = consoleColor;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static string Parsing(string shell, string parse)
    {
        List<string> text = [.. shell.Split(' ')];
        text.Remove(parse.Trim());
        return String.Join(" ",text);
    }
    public static string ParserCombine(string arg)
    {
        return Path.Combine(Mi.GetFolder, arg);
    }

    internal static void SetFolder(string arg)
    {
        Mi.SetFolder(arg);
        log.WriteInfo($"modification du path",Mi.GetFolder);
    }
}

