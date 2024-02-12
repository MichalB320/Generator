using ClassLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using Generator.Stores;
using System.Windows.Input;

namespace Generator.ViewModels;

internal class LoginViewModel : ObservableObject
{
    public ICommand ConnectComman { get; set; }
    public ICommand DisconnectCommand { get; set; }
    public ICommand NextCommand { get; set; }

    private string _domain;
    public string Domain { get => _domain; set { _domain = value; OnPropertyChanged(nameof(Domain)); } }

    private string _userInput;
    public string UserInput { get => _userInput; set { _userInput = value; OnPropertyChanged(nameof(UserInput)); } }

    private string _password;
    public string Password { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }

    private string _info;
    public string Info { get => _info; set { _info = value; OnPropertyChanged(nameof(Info)); } }

    public LoginViewModel(NavigationStore navigator, Mystructure mystructure, NavigationBarViewModel navigationBarViewModel)
    {
        _domain = "LDAP://pegasus.fri.uniza.sk";
        _userInput = "bezo1";
        _password = "";
        _info = "Logged out";

        Login lgi = new Login();

        ConnectComman = new RelayCommand(() => Info = lgi.LogIn(Domain, UserInput, Password));
        DisconnectCommand = new RelayCommand(() => Info = lgi.LogOut());
        NextCommand = new RelayCommand(() => navigator.CurrentViewModel = new SourcesManagerViewModel(navigator, mystructure, lgi, navigationBarViewModel));
    }

}
