using Generator.Language;
using Generator.Stores;
using Generator.ViewModels;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SwitchLanguage("en");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SwitchLanguage("en");
        }

        private void SwitchLanguage(string lang)
        {
            ResourceDictionary dictionary = new ResourceDictionary();

            if (lang == "en")
            {
                dictionary.Source = new Uri("\\Language\\StringResources-en.xaml", UriKind.Relative);
            }
            else if (lang == "sk")
            {
                dictionary.Source = new Uri("\\Language\\StringResources-sk.xaml", UriKind.Relative);
            }


            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        private void OnSlovakClick(object sender, RoutedEventArgs e)
        {
            SwitchLanguage("sk");
        }
    }
}