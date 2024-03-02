using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using Generator.Stores;
using System.Windows.Input;

namespace Generator.ViewModels;

internal class LoginViewModel : ObservableObject
{
    private readonly Login _loginModel;

    public ICommand ConnectCommand { get; }
    //public ICommand DisconnectCommand { get; }
    //public ICommand NextCommand { get; }

    public string Domain { get => _loginModel.Domain; set { _loginModel.Domain = value; OnPropertyChanged(nameof(Domain)); } }
    public string UserInput { get => _loginModel.UserName; set { _loginModel.UserName = value; OnPropertyChanged(nameof(UserInput)); } }
    public string Password { private get; set; } = string.Empty;
    public string Info { get => _loginModel.Info; set { _loginModel.Info = value; OnPropertyChanged(nameof(Info)); } }

    public NavigationBarViewModel NavigationBarViewModel { get; }

    public LoginViewModel(/*NavigationStore navigator, */NavigationBarViewModel navigationBarViewModel, IS iss)
    {
        _loginModel = iss.GetLogin();

        //navigationBarViewModel.Collapsed();
        //navigationBarViewModel.Visible();
        NavigationBarViewModel = navigationBarViewModel;

        ConnectCommand = new RelayCommand(Connect);
        //DisconnectCommand = new RelayCommand(() => Info = _loginModel.LogOut());
        //NextCommand = new RelayCommand(() => navigator.CurrentViewModel = new SourcesManagerViewModel(navigator,/* mystructure, _loginModel,*/ navigationBarViewModel, iss));
    }

    private async void Connect()
    {
        await Task.Run(() => _loginModel.LogIn(Domain, UserInput, Password));
        Info = _loginModel.Info;
    }
}
