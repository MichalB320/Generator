using ClassLibrary;
using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using Generator.Stores;
using System.Windows;
using System.Windows.Input;

namespace Generator.ViewModels;

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
    private string _lang;

    public NavigationBarViewModel(NavigationStore navigator, IS iss)
    {
        IS isk = iss;
        Mystructure structure = new();
        Login lgi = new();

        LoginViewModel loginVM = new(this, isk); ;
        SourcesManagerViewModel scManagerVM = new(this, isk);
        GenerateViewModel GenerateVM = new(structure, new System.Collections.ObjectModel.ObservableCollection<ButtonViewModel>(), navigator, this, lgi, isk);

        _lang = "en";

        NavigateLoginCommand = new RelayCommand(() =>
        {
            navigator.CurrentViewModel = loginVM;
            if (_lang == "en")
                SwitchLanguage("en");
            else if (_lang == "sk")
                SwitchLanguage("sk");
        });
        NavigateManagerCommand = new RelayCommand(() =>
        {
            navigator.CurrentViewModel = scManagerVM;
            if (_lang == "en")
                SwitchLanguage("en");
            else if (_lang == "sk")
                SwitchLanguage("sk");
        });
        NavigateGeneratorCommand = new RelayCommand(() =>
        {
            var novy = isk;
            navigator.CurrentViewModel = GenerateVM;
            if (_lang == "en")
                SwitchLanguage("en");
            else if (_lang == "sk")
                SwitchLanguage("sk");
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
        ResourceDictionary dictionary = new ResourceDictionary();

        if (lang == "en")
        {
            dictionary.Source = new Uri("\\Language\\StringResources-en.xaml", UriKind.Relative);
            _lang = "en";
        }
        else if (lang == "sk")
        {
            dictionary.Source = new Uri("\\Language\\StringResources-sk.xaml", UriKind.Relative);
            _lang = "sk";
        }

        Application.Current.Resources.MergedDictionaries.Add(dictionary);
    }
}
