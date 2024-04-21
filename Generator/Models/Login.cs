using System.DirectoryServices;
using System.Windows;

namespace GeneratorApp.Models;

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

            info = (string)Application.Current.FindResource("loggedIn"); // koment pre testy
            //info = "loggedIn"; // pre testy
        }
        catch (DirectoryServicesCOMException e)
        {
            string errorMessage;
            if (e.ErrorCode == -2147023570) // -2147023570  "The user name or password is incorrect." // koment pre testy
                errorMessage = (string)Application.Current.FindResource("IncorrectCredentialsError"); // koment pre testy
            else if (e.ErrorCode == -2147023174) // -2147023174 "The server is not operational."      // koment pre testy
                errorMessage = (string)Application.Current.FindResource("ServerNotOperationalError"); // koment pre testy
            else                                                                                      // koment pre testy
                errorMessage = e.Message;                                                             // koment pre testy
            //errorMessage = "message"; // pre testy

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
