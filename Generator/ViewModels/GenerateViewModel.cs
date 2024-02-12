using ClassLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using Generator.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Generator.ViewModels;

internal class GenerateViewModel : ObservableObject
{
    private Mystructure _structure;
    public ICommand GenerateComman { get; set; }
    public ICommand PreviousCommand { get; }

    private string _output;
    public string Output { get => _output; set { _output = value; OnPropertyChanged(nameof(Output)); } }

    private string _input;
    public string Input { get => _input; set { _input = value; OnPropertyChanged(nameof(Input)); } }

    private string _csvKey;
    public string CsvKey { get => _csvKey; set { _csvKey = value; OnPropertyChanged(nameof(CsvKey)); } }

    private string _ldapKey;
    public string LdapKey { get => _ldapKey; set { _ldapKey = value; OnPropertyChanged(nameof(LdapKey)); } }

    public NavigationBarViewModel NavigationBarViewModel { get; }

    private List<string> _zdroje;
    private ObservableCollection<ButtonViewModel> _buttons;

    private Generator.Models.Generator _gen;


    public GenerateViewModel(Mystructure structure, ObservableCollection<ButtonViewModel> buttons, NavigationStore navigation, NavigationBarViewModel navigationBarViewModel, Login lgi)
    {

        _zdroje = new();
        _buttons = buttons;
        NavigationBarViewModel = navigationBarViewModel;

        GenerateComman = new RelayCommand(OnClickGenerate);
        PreviousCommand = new RelayCommand(() =>
        {
            navigation.CurrentViewModel = new SourcesManagerViewModel(navigation, structure, lgi, navigationBarViewModel);
        });

        _structure = structure;
        _csvKey = "osCislo";
        _ldapKey = "uidNumber";

        //CSVData csv = structure.GetItem<CSVData>(0);

        //_output = $"{csv.Count}";


        //_input = "useradd -c \"$displayName$\" -d /home/$uid$ -u $uidNumber$ -m -g $gidNumber$ -s /bin/bash $uid$\nchmod 701 /home/$uid$";
        _input = "useradd -c \"$ukazka2.meno$\" -d /home/$ukazka4.meno$ -u $ukazka2.priezvisko$ -m -g $ukazka4.priezvisko$ -s /bin/bash $ukazka2.skupina$\nchmod 701 /home/$ukazka4.skupina$";

        _gen = new(_input, _structure, _buttons);
        _output = "";
    }

    private void OnClickGenerate()
    {
        Generator.Models.Generator gen = new(Input, _structure, _buttons);

        gen.JoinOn(CsvKey, LdapKey);
        gen.FindStrings();
        gen.FindSourcesAndVariables();
        gen.PrepareVariable();
        //gen.JoinOn("osCislo", "uidNumber");
        string output = gen.Generate();
        Output = output;
    }
}
