using ClassLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using Generator.Stores;
using System.Windows.Input;

namespace Generator.ViewModels;

public class NavigationBarViewModel : ObservableObject
{
    public ICommand NavigateLoginCommand { get; }
    public ICommand NavigateManagerCommand { get; }
    public ICommand NavigateGeneratorCommand { get; }
    public ICommand NavigateAccountCommand { get; }

    public NavigationBarViewModel(NavigationStore navigator, IS iss)
    {
        IS isk = iss;
        Mystructure structure = new();
        Login lgi = new();

        LoginViewModel loginVM = new(this, isk);
        SourcesManagerViewModel scManagerVM = new(/*navigator,*/ this, isk);
        GenerateViewModel GenerateVM = new(structure, new System.Collections.ObjectModel.ObservableCollection<ButtonViewModel>(), navigator, this, lgi, isk);

        NavigateLoginCommand = new RelayCommand(() =>
        {
            navigator.CurrentViewModel = loginVM;
        });
        NavigateManagerCommand = new RelayCommand(() =>
        {
            navigator.CurrentViewModel = scManagerVM;//new SourcesManagerViewModel(navigator, this, iss);
        });
        NavigateGeneratorCommand = new RelayCommand(() =>
        {
            var novy = isk;
            navigator.CurrentViewModel = GenerateVM; //new GenerateViewModel(structure, new System.Collections.ObjectModel.ObservableCollection<ButtonViewModel>(), navigator, this, lgi, iss);
        });
        NavigateAccountCommand = new RelayCommand(() =>
        {
            navigator.CurrentViewModel = new AccountViewModel(this);
        });

        navigator.CurrentViewModel = loginVM;
    }
}
