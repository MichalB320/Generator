using ClassLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using Generator.Stores;
using System.Windows;
using System.Windows.Input;

namespace Generator.ViewModels;

public class NavigationBarViewModel : ObservableObject
{
    public ICommand NavigateManagerCommand { get; }
    public ICommand NavigateGeneratorCommand { get; }
    public ICommand NavigateAccountCommand { get; }

    public Visibility V1 { get; set; }
    public Visibility V2 { get; set; }
    public Visibility V3 { get; set; }
    public bool IsEmpty { get; set; }

    public NavigationBarViewModel(NavigationStore navigator, IS iss)
    {
        Mystructure structure = new();
        Login lgi = new();



        NavigateManagerCommand = new RelayCommand(() =>
        {
            navigator.CurrentViewModel = new SourcesManagerViewModel(navigator, /*structure, lgi,*/ this, iss);
        });
        NavigateGeneratorCommand = new RelayCommand(() =>
        {
            navigator.CurrentViewModel = new GenerateViewModel(structure, new System.Collections.ObjectModel.ObservableCollection<ButtonViewModel>(), navigator, this, lgi, iss);
        });
        NavigateAccountCommand = new RelayCommand(() =>
        {
            navigator.CurrentViewModel = new AccountViewModel(this);
        });
    }

    public void Collapsed()
    {
        V1 = Visibility.Collapsed;
        V2 = Visibility.Collapsed;
        V3 = Visibility.Collapsed;
    }

    public void Visible()
    {
        V1 = Visibility.Visible;
        V2 = Visibility.Visible;
        V3 = Visibility.Visible;
    }
}
