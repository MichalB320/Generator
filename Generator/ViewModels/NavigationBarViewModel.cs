using ClassLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using Generator.Stores;
using System.Windows.Input;

namespace Generator.ViewModels;

public class NavigationBarViewModel : ObservableObject
{
    public ICommand NavigateManagerCommand { get; }
    public ICommand NavigateGeneratorCommand { get; }

    public NavigationBarViewModel(NavigationStore navigator)
    {
        Mystructure structure = new();
        Login lgi = new();

        NavigateManagerCommand = new RelayCommand(() =>
        {
            navigator.CurrentViewModel = new SourcesManagerViewModel(navigator, structure, lgi, this);
        });
        NavigateGeneratorCommand = new RelayCommand(() =>
        {
            navigator.CurrentViewModel = new GenerateViewModel(structure, new System.Collections.ObjectModel.ObservableCollection<ButtonViewModel>(), navigator, this, lgi);
        });

    }
}
