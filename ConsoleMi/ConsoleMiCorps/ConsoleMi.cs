using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json;
using ConsoleMi;
namespace ConsoleMi.ConsoleMiCorps;

// Creation d'une console Avec Special Folder.
// à mettre à jour vers une folder enregistrer et dynamique.

public class ConsoleM
{
    private string FolderActuel { get; set; }
    // private ThreadLance? 
    public string GetFolder { get => FolderActuel; }
    private string GetString() => "Mi " + GetFolder + " : ";
    public string Marquage { get => GetString(); }
    internal ConsoleM()
    {

        string folderActuel = Deserialize();
        FolderActuel = ValiditeFolder(folderActuel);
    }

    public void SetFolder(string folder)
    {
        FolderActuel = ValiditeFolder(folder);
        Serialize();
    }
    private static void Fini()
    {
        Console.WriteLine("enregistrement sur !");
    }

    private string ValiditeFolder(string? folder)
    {
        if (Directory.Exists(folder))
            return folder;
        else if (FolderActuel == null)
            return Info.FolderUser.Home;
        else
            return FolderActuel;
    }
    public static string Deserialize()
    {
        string jsonPath = Path.Combine(AppContext.BaseDirectory, "pathActuel.json");
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            try
            {
                var folder = JsonConvert.DeserializeObject<FolderMi>(json);
                if (folder == null || string.IsNullOrWhiteSpace(folder.Folder))
                    return "";
                else
                    return folder.Folder;
            }
            catch (Exception e)
            {
                ProgramMi.Exceptions.Add(e);
                return " ";
            }

        }
        return "";
    }

    private void Serialize()
    {
        FolderMi folder = new(FolderActuel);
        var json = JsonConvert.SerializeObject(folder);
        string jsonPath = Path.Combine(AppContext.BaseDirectory, "pathActuel.json");
        File.WriteAllText(jsonPath, json);
    }
}

class FolderMi
{
    public string Folder { get; }
    public FolderMi(string path) { Folder = path; }
}

// Variables Constantes
public static class Info
{
    public static char Signer { get; } = '\\';
    public static class FolderUser
    {
        public static string Home { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); }
        public static string Downloads { get => Home + Signer + "Downloads"; }
        public static string Documents { get => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); }
        public static string Desktop { get => Environment.GetFolderPath(Environment.SpecialFolder.Desktop); }
        public static string Music { get => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic); }
        public static string Pictures { get => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures); }
        public static string Movies { get => Environment.GetFolderPath(Environment.SpecialFolder.MyVideos); }
    }
}

public class ThreadLance
{
    public delegate void Function();
    public Action Action1;
    public event Function? Finition;
    public Thread? thread;
    public ThreadLance(Action action)
    {
        Action1 = action;
    }
    public void Lance()
    {
        thread = new Thread(() =>
        {
            Action1();
            var fini = Finition;
            fini?.Invoke();
        });
        thread.Start();
    }
}
public class ThreadLance<T>(Func<T> func)
{
    public Func<T> action = func;
    private Thread? thread;
    public event Action<T>? Finition;
    public T? Result { get; private set; }
    public void Lance()
    {
        thread = new Thread(() =>
        {
            Result = action();
            var fini = Finition;
            fini?.Invoke(Result);
        });
        thread.Start();
    }
}

public class Logging
{
    public string pathLogging { get; private set; }
    private string Pathpath { get => Path.Combine(AppContext.BaseDirectory, pathLogging); }
    public Logging(string path = "Logging.txt")
    {
        if (Path.Exists(path))
        {
            if (Path.GetExtension(path) == ".txt")
            {
                pathLogging = path;
            }
            else
            {
                IOException iO = new("le fichier doit etre un fichier text.");
                throw iO;
            }
        }
        else
        {
            if (path.Trim().EndsWith(".txt"))
            {
                File.Create(path);
                pathLogging = path;
            }
            else
            {
                IOException iO = new("le fichier n'existe pas.");
                throw iO;
            }
        }
    }
    private bool Change(string path) // experimentale methode
    {
        if (path.EndsWith(".txt"))
        {
            if (Path.Exists(path))
            {
                return false;
            }
            else
            {
                FileMi.RenameFile(Pathpath, Path.Combine(AppContext.BaseDirectory, path));
                return true;
            }
        }
        else
            return false;
    }

    private void WriteLog(string text)
    {
        File.AppendAllText(pathLogging, text);
    }
    public void WriteError(Exception e)
    {
        string toWrite = $"{DateTime.Now}, [Error ] : {e.Message},{e.Source}";
        WriteLog(toWrite);
    }
    public void WriteInfo(string message, string source)
    {
        string toWrite = $"{DateTime.Now}, [Info ] : {message}";
        WriteLog(toWrite);
    }
    public void WriteWarning(string message, string source)
    {
        string toWrite = $"{DateTime.Now}, [Info ] : {message}, {source}";
        WriteLog(toWrite);
    }
}