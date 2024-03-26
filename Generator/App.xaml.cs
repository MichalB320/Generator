using Generator.Models;
using Generator.Stores;
using Generator.ViewModels;
using System.Windows;

namespace Generator;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IS _is;

    private readonly NavigationStore _navigationStore;
    private readonly NavigationBarViewModel _navigationBarViewModel;

    public App()
    {


        _is = new IS();

        _navigationStore = new NavigationStore();
        _navigationBarViewModel = new NavigationBarViewModel(_navigationStore, _is);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        //_navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore, _navigationBarViewModel, _is);//new SourcesManagerViewModel(_manager, _navigationStore);

        MainWindow = new MainWindow()
        {
            DataContext = new MainViewModel(_navigationStore)
        };
        MainWindow.Show();

        base.OnStartup(e);
    }
}
