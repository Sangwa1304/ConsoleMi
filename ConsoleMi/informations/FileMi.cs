using ConsoleMi;

namespace ConsoleMi.Informations;

public static class FileMi
{
    private static List<string> AllsFolders = new();

    private static string[] Dirs(string source)
    {
        try
        {
            return ElementVide(Directory.GetDirectories(source));
        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
            return [];
        }
    }
    private static string[] Files(string source)
    {
        try
        {
            return ElementVide(Directory.GetFiles(source));
        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
            return [];
        }

    }
    
    private static string[] DirsNames(string source)
    {
        return ElementVide([.. from string dir in Dirs(source)
                    select Path.GetFileNameWithoutExtension(dir)]);
    }

    private static string[] FilesNames(string source)
    {
        return ElementVide([.. from string dir in Files(source)
                    select Path.GetFileNameWithoutExtension(dir)]);
    }
    private static string[] FilesDirs(string source)
    {
        // cette methode se charge de retourner les path des fichiers et dossiers dans la source.
        return AddPath(Dirs(source),Files(source));
    }
    public static string[] FilesDirsNames(string source)
    {
        return ElementVide([..from string path in FilesDirs(source)
                    select Path.GetFileName(path)]);
    }
    private static string[] AddPath(string[] pathsA,string[] pathsB)
    {
        List<string> Paths = [..pathsA];
        foreach(string path in pathsB)
        {
            Paths.Add(path);
        }
        return [..Paths];
    }
    private static string[] ElementVide(string[] paths)
    {

        return [.. from string path in paths
                    where path != null
                    where path.Trim() != ""
                    select path];
    }
    private static string[] SearchFileDir(string source, string word)
    {
        string[] dirs = Dirs(source);
        if (dirs.Length > 0)
        {
            foreach (string dir in dirs)
            {
                SearchFileDir(dir, word);
            }
        }
        return [.. from string path in FilesDirs(source)
                    where Path.GetFileName(path).Contains(word)
                    where Path.GetFileName(path).ToUpper().Trim().Contains(word.ToUpper().Trim())
                    select Add(path)];
    }

    private static string Add(string m)
    {
        AllsFolders.Add(m);
        return m;
    }
    public static List<string> Searching(string source, string word)
    {
        AllsFolders = new();
        SearchFileDir(source, word);
        return AllsFolders = new();

    }
    public static bool DeletteFile(string path)
    {
        try
        {
            File.Delete(path);
            return true;
        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
            return false;
        }
    }

    public static bool DeletteDirectory(string path, bool recursive = false)
    {
        try
        {
            Directory.Delete(path, recursive);
            return true;
        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
            return false;
        }
    }
    public static bool CreateDirectory(string path)
    {
        try
        {
            Directory.CreateDirectory(path);
            return true;
        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
            return false;
        }
    }
    public static bool CreateFile(string path)
    {
        try
        {
            File.Create(path).Close();
            return true;
        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
            return false;
        }
    }
    public static bool CopyFile(string source, string destination, bool overwrite = false)
    {
        try
        {
            File.Copy(source, destination, overwrite);
            return true;
        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
            return false;
        }
    }
    public static bool MoveFile(string source, string destination)
    {
        try
        {
            File.Move(source, destination);
            return true;
        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
            return false;
        }
    }
    public static bool MoveDirectory(string source, string destination)
    {
        try
        {
            Directory.Move(source, destination);
            return true;
        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
            return false;
        }
    }
    public static bool RenameFile(string source, string newName)
    {
        try
        {
            string destination = Path.Combine(Path.GetDirectoryName(source) ?? "", newName);
            File.Move(source, destination);
            return true;
        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
            return false;
        }
    }
    public static bool RenameDirectory(string source, string newName)
    {
        try
        {
            string destination = Path.Combine(Path.GetDirectoryName(source) ?? "", newName);
            Directory.Move(source, destination);
            return true;
        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
            return false;
        }
    }
    public static string ReadFileTxt(string path)
    {
        try
        {
            return File.ReadAllText(path);
        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
            return "";
        }
    }
    public static bool WriteFileTxt(string path, string content, bool append = false)
    {
        try
        {
            if (append)
            {
                File.AppendAllText(path, content);
            }
            else
            {
                File.WriteAllText(path, content);
            }
            return true;
        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
            return false;
        }
    }
    public static bool LaunchFile(string path)
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            });
            return true;
        }
        catch (Exception e)
        {
            GestionnaireException.Add(e);
            return false;
        }
    }
}

public class PathInfo
{
    public string Path { get; set; }
    public bool IsDirectory { get; set; }
    public bool IsFile { get; set; }
    public long SizeInBytes { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastAccessTime { get; set; }
    public DateTime LastWriteTime { get; set; }

    public PathInfo(string path)
    {
        Path = path;
        IsDirectory = Directory.Exists(path);
        IsFile = File.Exists(path);

        if (IsFile)
        {
            FileInfo fi = new FileInfo(path);
            SizeInBytes = fi.Length;
            CreationTime = fi.CreationTime;
            LastAccessTime = fi.LastAccessTime;
            LastWriteTime = fi.LastWriteTime;
        }
        else if (IsDirectory)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            SizeInBytes = 0; // Directories do not have a size in the same way files do
            CreationTime = di.CreationTime;
            LastAccessTime = di.LastAccessTime;
            LastWriteTime = di.LastWriteTime;
        }
        else
        {
            throw new FileNotFoundException("The specified path does not exist.", path);
        }
    }
    public static PathInfo GetPathInfo(string path)
    {
        return new PathInfo(path);
    }
}