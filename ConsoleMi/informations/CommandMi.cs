namespace ConsoleMi.Informations;

static class CommandeMi
{
    public static List<Command> commands = new();
    public static void NormalizeEcriture(string[] textes, string? source = null)
    {
        if (source != null)
            Console.WriteLine(Ecrit("-", (60 - source.Length) / 2) + source + Ecrit("-", (60 - source.Length) / 2));
        Console.WriteLine(Ecrit("*", 60));

        foreach (string texte in textes)
        {
            Console.WriteLine("* \t\t\t" + texte);
        }
        Console.WriteLine(Ecrit("*", 60));
    }
    private static string Ecrit(string terme, int nombre)
    {
        string t = terme;

        for (int i = 0; i < nombre; i++)
        {
            t += terme;
        }
        return t;
    }
    public static void SetFolder(string path)
    {
        Info.Mi.SetFolder(UserFolder(path));
    }
    public static string UserFolder(string path)
    {
        string pathName = path;
        string pathScan = path.Trim();
        switch (pathScan)
        {
            case "DOCUMENTS":
            case "Documents":
            case "document":
            case "documents":
            case "Document":
                pathName = Info.FolderUser.Documents;
                break;

            case "Image":
            case "Images":
            case "image":
            case "img":
            case "IMAGE":
            case "IMAGES":
            case "Pictures":
            case "pictures":
                pathName = Info.FolderUser.Pictures;
                break;

            case "Videos":
            case "Video":
            case "Movies":
            case "videos":
            case "video":
            case "movie":
            case "movies":
                pathName = Info.FolderUser.Movies;
                break;

            case "music":
            case "musique":
            case "Musique":
            case "Music":
                pathName = Info.FolderUser.Music;
                break;

            case "Downloads":
            case "downloads":
            case "download":
            case "Download":
            case "DOWNLOADS":
                pathName = Info.FolderUser.Downloads;
                break;

            case "home":
            case "Home":
                pathName = Info.FolderUser.Home;
                break;

            case "Desktop":
            case "Bureau":
            case "bureau":
            case "desktop":
                pathName = Info.FolderUser.Desktop;
                break;

            default:
                break;
        }
        return pathName;
    }
}

struct Command(string commande, List<string>? commandes = null)
{
    private List<string>? Commandes { get; } = commandes;
    private string Commande { get; } = commande;
    public bool IsCommand(string commande)
    {
        if (commande == Commande)
            return true;
        if (Commandes == null)
            return false;
        else if (Commandes.Contains(commande))
            return true;
        return false;
    }
}