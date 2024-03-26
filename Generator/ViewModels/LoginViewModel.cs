using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using System.Windows.Input;

namespace Generator.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly IS _is;

    public ICommand ConnectCommand { get; }

    public string Domain { get; set; }
    public string UserInput { get; set; }
    public string Password { private get; set; } = string.Empty;
    public string Info { get => _is.Info; set { _is.Info = value; OnPropertyChanged(nameof(Info)); } }
    public string InfoV { get; set; }

    public NavigationBarViewModel NavigationBarViewModel { get; }

    public LoginViewModel(NavigationBarViewModel navigationBarViewModel, IS iss)
    {
        _is = iss;
        Domain = "LDAP://pegasus.fri.uniza.sk";
        UserInput = "bezo1";
        NavigationBarViewModel = navigationBarViewModel;
        ConnectCommand = new RelayCommand(Connect);

        InfoV = "logged out";
    }

    private async void Connect() => Info = await Task.Run(() => _is.Login(Domain, UserInput, Password));
}
