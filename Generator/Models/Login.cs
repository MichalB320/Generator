using System.DirectoryServices;
using System.Windows;

namespace Generator.Models;

public class Login
{
    public string Domain { get; set; }
    public string UserName { get; set; }
    public string Info { get; set; }


    private DirectoryEntry? _entry;
    private DirectorySearcher? _searcher;

    public Login()
    {
        Domain = "";
        UserName = "";
        Info = "";
    }

    public Login(string domain, string userName, string info)
    {
        Domain = domain;
        UserName = userName;
        Info = info;
    }

    public void LogIn(string ldapPath, string userName, string password)
    {
        try
        {
            _entry = new DirectoryEntry(ldapPath, userName, password);
            object nativeObject = _entry.NativeObject;

            _searcher = new DirectorySearcher(_entry);

            Info = "Logged in";
        }
        catch (Exception e)
        {
            Console.WriteLine($"Chyba prihlasenia {e.Message}");
            Info = $"Error: {e.Message}";
        }
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
            MessageBox.Show(e.Message);
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
