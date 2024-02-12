using System.DirectoryServices;

namespace Generator.Models;

public class Login
{
    private DirectoryEntry? _entry;
    private DirectorySearcher? _searcher;

    public Login()
    {

    }

    public string LogIn(string ldapPath, string userName, string password)
    {
        string info = "";

        try
        {
            _entry = new DirectoryEntry(ldapPath, userName, password);
            object nativeObject = _entry.NativeObject;

            _searcher = new DirectorySearcher(_entry);

            info = "Logged in";
        }
        catch (Exception e)
        {
            Console.WriteLine($"Chyba prihlasenia {e.Message}");
            info = $"Error: {e.Message}";
        }

        return info;
    }

    public SearchResultCollection? Search(string filter)
    {
        SearchResultCollection? structure = null;

        try
        {
            if (_searcher != null)
            {
                _searcher.Filter = filter;
                structure = _searcher.FindAll();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return structure;
    }

    public string LogOut()
    {
        if (_entry != null)
        {
            _entry.Close();
            _entry.Dispose();
        }

        if (_searcher != null)
            _searcher.Dispose();

        return "Logged out";
    }

    internal bool ExistsSearcher()
    {
        if (_searcher != null)
            return true;
        else
            return false;
    }
}
