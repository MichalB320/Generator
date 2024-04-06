using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using GeneratorApp;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Generator.ViewModels;

public class GenerateViewModel : ViewModelBase
{
    public ICommand GenerateComman { get; set; }
    public ICommand SaveCommand { get; }
    public ICommand DelimiterCommand { get; }
    public ICommand KeyCommand { get; }

    private string _output = string.Empty;
    public string Output { get => _output; set { _output = value; OnPropertyChanged(nameof(Output)); } }

    private string _input;
    public string Input { get => _input; set { _input = value; OnPropertyChanged(nameof(Input)); } }

    private string _csvKey;
    public string CsvKey { get => _csvKey; set { _csvKey = value; OnPropertyChanged(nameof(CsvKey)); } }

    private string _ldapKey;
    public string LdapKey { get => _ldapKey; set { _ldapKey = value; OnPropertyChanged(nameof(LdapKey)); } }

    private char _delimiter = '.';
    public char Delimiter { get => _delimiter; set { _delimiter = value; OnPropertyChanged(nameof(Delimiter)); } }

    private int _progresBar;
    public int ProgresBar { get => _progresBar; set { _progresBar = value; OnPropertyChanged(nameof(ProgresBar)); } }

    public NavigationBarViewModel NavigationBarViewModel { get; }

    private IS _is;

    public GenerateViewModel(NavigationBarViewModel navigationBarViewModel, IS iss)
    {
        _is = iss;
        NavigationBarViewModel = navigationBarViewModel;

        GenerateComman = new RelayCommand(OnClickGenerate);
        SaveCommand = new RelayCommand(OnClickSave);
        DelimiterCommand = new RelayCommand(onClickDelimiter);
        KeyCommand = new RelayCommand(OnClickKey);

        _csvKey = "osCislo";
        _ldapKey = "uidNumber";

        //_input = "useradd -c \"$displayName$\" -d /home/$uid$ -u $uidNumber$ -m -g $gidNumber$ -s /bin/bash $uid$\nchmod 701 /home/$uid$";
        _input = "useradd -c \"$ukazka2.meno$\" -d /home/$ukazka4.meno$ -u $ukazka2.priezvisko$ -m -g $ukazka4.priezvisko$ -s /bin/bash $ukazka2.skupina$\nchmod 701 /home/$ukazka4.skupina$";
    }

    private void OnClickKey()
    {
        DelimiterWindow deliWin = new()
        {
            DataContext = this,
        };
        deliWin.ShowDialog();
    }

    private void onClickDelimiter()
    {
        //DelimiterWindow delWin = new()
        //{
        //    DataContext = this,
        //};
        //delWin.ShowDialog();
        //string inputValue = Microsoft.VisualBasic.Interaction.InputBox("Zadejte hodnotu:", "Vstupní pole", "");
        try
        {
            Delimiter = Convert.ToChar(Microsoft.VisualBasic.Interaction.InputBox(Application.Current.FindResource("characterEnter") as string, "Enter character", $"{Delimiter}"));
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private void OnClickSave()
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "CSV file (*.csv) | *.csv|SQL file (*.sql) | *.sql| All files (*.*) | *.*";
        sfd.CheckFileExists = false;
        sfd.CheckPathExists = true;
        sfd.ShowDialog();

        if (sfd.FileName != "")
        {
            using (StreamWriter sw = new StreamWriter(sfd.FileName))
            {
                sw.Write(Output);
            }
        }
        else
        {
            MessageBox.Show("FileName is empty!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private async void OnClickGenerate()
    {
        var progress = new Progress<int>(value =>
        {
            ProgresBar = value;
        });

        //await Task.Run(() => Gen(progress));

        //Generator.Models.Generator gen = new(Input, _structure, _buttons);
        //Generator.Models.Generator gen = new(Input, _is.GetStructure(), _is.DynamicButtons);

        try
        {
            Generator.Models.Generator gen = _is.GetGenerator();
            gen.setValues(Input, _is.DynamicButtons);

            ProgresBar = 0;


            await gen.FindStrings();
            ProgresBar = 10;
            await gen.FindSourcesAndVariables(Delimiter);
            ProgresBar = 20;

            if (gen.SourcesExists())
            {
                await gen.JoinOn(CsvKey, LdapKey, progress);
                ProgresBar = 60;
                gen.PrepareVariable();
                ProgresBar = 80;

                //gen.JoinOn("osCislo", "uidNumber");
                string output = gen.Generate();
                ProgresBar = 95;
                Output = output;
                ProgresBar = 100;
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
}
