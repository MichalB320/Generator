using GeneratorApp.Models;

namespace Test;

public class LoginTest
{
    private readonly Login _login;
    private readonly string _userName = "";
    private readonly string _password = "";

    public LoginTest()
    {
        _login = new Login();
    }

    [Fact]
    public void LogIn_CorrectCredentials_ReturnsLoggedIn()
    {
        string ldapPath = "LDAP://pegasus.fri.uniza.sk";
        string userName = _userName;
        string password = _password;

        string result = _login.LogIn(ldapPath, userName, password);

        Assert.Equal("loggedIn", result);
    }

    [Fact]
    public void LogIn_IncorrectCredentials_ReturnsErrorMessage()
    {
        string ldapPath = "LDAP://pegasus.fri.uniza.sk";
        string userName = "milos";
        string password = "heslo_milosa";

        string result = _login.LogIn(ldapPath, userName, password);

        Assert.StartsWith("Error:", result);
        Assert.Contains("Error", result);
    }

    [Fact]
    public void LogIn_ServerNotOperational_ReturnsErrorMessage()
    {
        string ldapPath = "LDAP://nonexistent-server";
        string userName = _userName;
        string password = _password;

        string result = _login.LogIn(ldapPath, userName, password);

        Assert.StartsWith("Error:", result);
        Assert.Contains("The server is not operational", result);
    }

    [Fact]
    public void LogOut_ClosesEntryAndSearcher()
    {
        _login.LogIn("LDAP://pegasus.fri.uniza.sk", _userName, _password);

        string result = _login.LogOut();

        Assert.Equal("Logged out", result);
    }
}
