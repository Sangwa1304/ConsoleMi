using System.Text.Json;

namespace ConsoleMi.Informations;

// Creation d'une console Avec Special Folder.
// à mettre à jour vers une folder enregistrer et dynamique.
public class ConsoleM
{
    private string FolderActuel { get; set; }
    public string GetFolder { get => FolderActuel; }
    private string GetString() => "Mi " + GetFolder + " : ";
    public string Marquage{get =>GetString();}
    internal ConsoleM()
    {
        string folderActuel = Deserialize();
        FolderActuel = ValiditeFolder(folderActuel);
    }

    public void SetFolder(string folder)
    {
        FolderActuel = ValiditeFolder(folder);
        
        Serialize(FolderActuel);
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
    public static string Deserialize(string? exemple = null)
    {
        if(File.Exists("pathActuel.json"))
        {
            string json = File.ReadAllText("pathActuel.json");
            try
            {
                
                var folder =  JsonSerializer.Deserialize<FolderMi>(json);
                if(folder == null)
                    return "";
                else
                    return folder.Folder;
            }
            catch(Exception e)
            {
                GestionnaireException.Add(e);
                return " ";
            }
    
        }
        return "";
    }

    private static void Serialize(string path)
    {
        FolderMi folder = new(path);
        var json = JsonSerializer.Serialize(folder);
        File.WriteAllText("pathActuel.json",json);
    }
}

public class FolderMi(string path)
{
    public string Folder{get;} = path;
}

// Variables Constantes
public static class Info
{
    public static char Signer{get;} = '\\';
    public static class FolderUser
    {
        public static string Home { get => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); } 
        public static string Downloads { get => Home + Signer + "Downloads";}
        public static string Documents { get => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);}
        public static string Desktop { get => Environment.GetFolderPath(Environment.SpecialFolder.Desktop); }
        public static string Music { get => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic); }
        public static string Pictures { get => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures); }
        public static string Movies { get => Environment.GetFolderPath(Environment.SpecialFolder.MyVideos); }
    }
    internal static ConsoleM Mi = new();
}
