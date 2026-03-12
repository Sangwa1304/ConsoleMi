using ConsoleMi.Informations;

class Program
{
    public static void Main(string[] args)
    {

        try
        {
            Console.Clear();

        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
        }
        string? shell = "This is the Origine";
        Console.WriteLine(Info.Mi.GetFolder);
        Info.Mi.SetFolder(Info.FolderUser.Documents);
        Console.WriteLine(Info.Mi.GetFolder);
        Info.Mi.SetFolder(Info.FolderUser.Downloads);
        Console.WriteLine(Info.Mi.GetFolder);
        Info.Mi.SetFolder(Info.FolderUser.Desktop);
        Console.WriteLine(Info.Mi.GetFolder);
        Info.Mi.SetFolder(Info.FolderUser.Pictures);
        Environment.Exit(1);
        while(shell != "exit")
        {
            try
            {
                string? arg;
                Console.Write(Info.Mi.Marquage);
                shell = Console.ReadLine();

                if(shell == null || shell == "Exit")
                    continue;
                
                if(shell.StartsWith("set"))
                {
                    arg = shell.Split("set ").ToString().Trim();
                }
            }
            catch (Exception e)
            {
                GestionnaireException.Add(e);
            }
        }
        GestionnaireException.ViewException();
    }
}

