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

            info = (string)Application.Current.FindResource("loggedIn");
        }
        catch (System.DirectoryServices.DirectoryServicesCOMException e)
        {
            string errorMessage;
            if (e.ErrorCode == -2147023570) // -2147023570 je kód chyby pre "The user name or password is incorrect."
                errorMessage = (string)Application.Current.FindResource("IncorrectCredentialsError");
            else if (e.ErrorCode == -2147023174) // -2147023174 je kód chyby pre "The server is not operational."
                errorMessage = (string)Application.Current.FindResource("ServerNotOperationalError");
            else
                errorMessage = e.Message;

            info = $"Error: {errorMessage}";
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
