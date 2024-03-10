using System.DirectoryServices;
using System.Windows;

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
        string info;
        try
        {
            _entry = new DirectoryEntry(ldapPath, userName, password);
            object nativeObject = _entry.NativeObject;

            _searcher = new DirectorySearcher(_entry);

            info = "Logged in";
        }
        catch (Exception e)
        {
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
            MessageBox.Show(e.Message);
        }

        return structure;
    }

    public string LogOut()
    {
        _entry?.Close();
        _entry?.Dispose();

        _searcher?.Dispose();

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
