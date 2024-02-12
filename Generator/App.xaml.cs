using ClassLibrary;
using Generator.Stores;
using Generator.ViewModels;
using System.Windows;

namespace Generator;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private Mystructure _list;
    //private Login _lgi;
    //private Manager _manager;
    //private Generator.Models.Generator _generator;
    private NavigationStore _navigationStore;
    private NavigationBarViewModel _navigationBarViewModel;

    public App()
    {
        _list = new Mystructure();
        //_lgi = new Login();
        //_manager = new Manager(ref _list);


        _navigationStore = new NavigationStore();
        _navigationBarViewModel = new NavigationBarViewModel(_navigationStore);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore, _list, _navigationBarViewModel);//new SourcesManagerViewModel(_manager, _navigationStore);

        MainWindow = new MainWindow()
        {
            DataContext = new MainViewModel(_navigationStore)
        };
        MainWindow.Show();

        base.OnStartup(e);
    }
}
