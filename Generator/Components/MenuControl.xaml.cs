using System.Windows;
using System.Windows.Controls;

namespace GeneratorApp.Components;

/// <summary>
/// Interaction logic for MenuControl.xaml
/// </summary>
public partial class MenuControl : UserControl
{
    public MenuControl()
    {
        InitializeComponent();
        //SwitchLanguage("en");
    }

    //private void SwitchLanguage(string lang)
    //{
    //    ResourceDictionary dictionary = new ResourceDictionary();

    //    if (lang == "en")
    //        dictionary.Source = new Uri("\\Language\\StringResources-en.xaml", UriKind.Relative);
    //    else if (lang == "sk")
    //        dictionary.Source = new Uri("\\Language\\StringResources-sk.xaml", UriKind.Relative);

    //    Application.Current.Resources.MergedDictionaries.Add(dictionary);
    //}

    //private void OnSlovakClick(object sender, RoutedEventArgs e) => SwitchLanguage("sk");

    //private void OnEnglishClick(object sender, RoutedEventArgs e) => SwitchLanguage("en");
}
