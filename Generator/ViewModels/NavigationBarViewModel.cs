//using ClassLibrary;
using CommunityToolkit.Mvvm.Input;
using GeneratorApp.Models;
using GeneratorApp.Stores;
using System.Windows;
using System.Windows.Input;

namespace GeneratorApp.ViewModels;

public class NavigationBarViewModel : ViewModelBase
{
    public ICommand NavigateLoginCommand { get; }
    public ICommand NavigateManagerCommand { get; }
    public ICommand NavigateGeneratorCommand { get; }

    public ICommand SlovakCommand { get; }
    public ICommand EnglishCommand { get; }

    public LoginViewModel LoginViewModel { get; }
    public SourcesManagerViewModel SourcesManagerViewModel { get; }
    public GenerateViewModel GenerateViewModel { get; }
    private ResourceDictionary _resourceDictionary;

    public NavigationBarViewModel(NavigationStore navigator, IS iss)
    {
        IS isk = iss;
        Mystructure structure = new();

        LoginViewModel loginVM = new(this, isk);
        SourcesManagerViewModel scManagerVM = new(this, isk);
        GenerateViewModel GenerateVM = new(this, isk);

        _resourceDictionary = new ResourceDictionary();

        NavigateLoginCommand = new RelayCommand(() => navigator.CurrentViewModel = loginVM);
        NavigateManagerCommand = new RelayCommand(() => navigator.CurrentViewModel = scManagerVM);
        NavigateGeneratorCommand = new RelayCommand(() =>
        {
            var novy = isk;
            navigator.CurrentViewModel = GenerateVM;
        });
        SlovakCommand = new RelayCommand(() => SwitchLanguage("sk"));
        EnglishCommand = new RelayCommand(() => SwitchLanguage("en"));

        navigator.CurrentViewModel = loginVM;
        LoginViewModel = loginVM;
        SourcesManagerViewModel = scManagerVM;
        GenerateViewModel = GenerateVM;
    }

    private void SwitchLanguage(string lang)
    {
        if (lang == "en")
            _resourceDictionary.Source = new Uri("\\Language\\StringResources-en.xaml", UriKind.Relative);
        else if (lang == "sk")
            _resourceDictionary.Source = new Uri("\\Language\\StringResources-sk.xaml", UriKind.Relative);

        Application.Current.Resources.MergedDictionaries.Add(_resourceDictionary);
    }
}
