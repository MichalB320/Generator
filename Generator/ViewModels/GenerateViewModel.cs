using ClassLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using Generator.Stores;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace Generator.ViewModels;

internal class GenerateViewModel : ObservableObject
{
    private Mystructure _structure;
    public ICommand GenerateComman { get; set; }
    //public ICommand PreviousCommand { get; }
    public ICommand SaveCommand { get; }

    private string _output;
    public string Output { get => _output; set { _output = value; OnPropertyChanged(nameof(Output)); } }

    private string _input;
    public string Input { get => _input; set { _input = value; OnPropertyChanged(nameof(Input)); } }

    private string _csvKey;
    public string CsvKey { get => _csvKey; set { _csvKey = value; OnPropertyChanged(nameof(CsvKey)); } }

    private string _ldapKey;
    public string LdapKey { get => _ldapKey; set { _ldapKey = value; OnPropertyChanged(nameof(LdapKey)); } }

    private int _progresBar;
    public int ProgresBar { get => _progresBar; set { _progresBar = value; OnPropertyChanged(nameof(ProgresBar)); } }

    public NavigationBarViewModel NavigationBarViewModel { get; }

    private List<string> _zdroje;
    private ObservableCollection<ButtonViewModel> _buttons;

    private Generator.Models.Generator _gen;
    private IS _is;

    public GenerateViewModel(Mystructure structure, ObservableCollection<ButtonViewModel> buttons, NavigationStore navigation, NavigationBarViewModel navigationBarViewModel, Login lgi, IS iss)
    {
        _is = iss;
        _zdroje = new();
        _buttons = buttons;
        NavigationBarViewModel = navigationBarViewModel;

        GenerateComman = new RelayCommand(OnClickGenerate);
        //PreviousCommand = new RelayCommand(() =>
        //{
        //    navigation.CurrentViewModel = new SourcesManagerViewModel(navigation, /*structure, lgi,*/ navigationBarViewModel, iss);
        //});
        SaveCommand = new RelayCommand(OnClickSave);

        _structure = structure;
        _csvKey = "osCislo";
        _ldapKey = "uidNumber";

        //CSVData csv = structure.GetItem<CSVData>(0);

        //_output = $"{csv.Count}";


        //_input = "useradd -c \"$displayName$\" -d /home/$uid$ -u $uidNumber$ -m -g $gidNumber$ -s /bin/bash $uid$\nchmod 701 /home/$uid$";
        _input = "useradd -c \"$ukazka2.meno$\" -d /home/$ukazka4.meno$ -u $ukazka2.priezvisko$ -m -g $ukazka4.priezvisko$ -s /bin/bash $ukazka2.skupina$\nchmod 701 /home/$ukazka4.skupina$";

        _gen = new(_input, ref _structure, _buttons);
        _output = "";
        //navigationBarViewModel.Visible();
    }

    private void OnClickSave()
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "CSV file (*.csv) | *.csv|SQL file (*.sql) | *.sql| All files (*.*) | *.*";
        sfd.CheckFileExists = false;
        sfd.CheckPathExists = true;
        sfd.ShowDialog();
        
        using (StreamWriter sw = new StreamWriter(sfd.FileName))
        {
            sw.Write(Output);
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
        Generator.Models.Generator gen = new(Input, ref _is.GetStructure(), _is.GetManager().DynamicButtons);

        ProgresBar = 0;


        await gen.FindStrings();
        ProgresBar = 10;
        await gen.FindSourcesAndVariables();
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
}
