using ClassLibrary;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using Generator.Stores;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Generator.ViewModels;

public class SourcesManagerViewModel : ObservableObject
{
    public ICommand LdapCommand { get; }
    public ICommand CsvCommand { get; }
    public ICommand ClickCommand { get; set; }
    public ICommand NextCommand { get; set; }
    public ICommand PreviousCommand { get; }

    private string _output;
    public string Output { get => _output; set { _output = value; OnPropertyChanged(nameof(Output)); } }

    private int _count;
    public int Count { get => _count; set { _count = value; OnPropertyChanged(nameof(Count)); } }

    private ObservableCollection<ButtonViewModel> _dynamicButtons;
    public ObservableCollection<ButtonViewModel> DynamicButtons
    {
        get => _dynamicButtons;
        set
        {
            if (_dynamicButtons != value)
            {
                _dynamicButtons = value;
                OnPropertyChanged(nameof(DynamicButtons));
            }
        }
    }

    private double _scrollValue;
    public double ScrollValue
    {
        get { return _scrollValue; }
        set
        {
            if (_scrollValue != value)
            {
                _scrollValue = value;
                OnPropertyChanged(nameof(ScrollValue));
            }
        }
    }

    private double _scrollMax;
    public double ScrollMax
    {
        get { return _scrollMax; }
        set
        {
            if (_scrollMax != value)
            {
                _scrollMax = value;
                OnPropertyChanged(nameof(ScrollMax));
            }
        }
    }

    public NavigationBarViewModel NavigationBarViewModel { get; }

    private Login _lgi;
    private Manager _manager;

    public SourcesManagerViewModel(/*Manager manager, */NavigationStore navigation, Mystructure structure, Login login, NavigationBarViewModel navigationBarViewModel)
    {
        var manager = new Manager(ref structure);
        var list = manager.Structure;

        _lgi = login;
        _manager = manager;

        _count = 0;
        _output = "";

        _dynamicButtons = new ObservableCollection<ButtonViewModel>();

        LdapCommand = new RelayCommand(OnClickLdapBtn);
        CsvCommand = new RelayCommand(OnClickCsvBtn);
        ClickCommand = new RelayCommand<ButtonViewModel>((parameter) =>
        {
            if (parameter != null)
            {
                var index = parameter.Index;

                if (parameter.Type == typeof(CSVData))
                    OnClickButton(parameter.Index);
                else if (parameter.Type == typeof(SearchResultCollection))
                    OnClickBtn(parameter.Index);
            }
        });
        NextCommand = new RelayCommand(() => navigation.CurrentViewModel = new GenerateViewModel(manager.Structure, _dynamicButtons, navigation, navigationBarViewModel, login));
        PreviousCommand = new RelayCommand(() => navigation.CurrentViewModel = new LoginViewModel(navigation, structure, navigationBarViewModel));
        NavigationBarViewModel = navigationBarViewModel;

        Update();
    }

    private void Update()
    {
        _count = _manager.Structure.Count;
    }

    private void OnClickCsvBtn()
    {
        var ofd = new OpenFileDialog();
        ofd.Filter = "CSV súbory (*.csv) | *.csv";
        ofd.ShowDialog();

        if (!ofd.FileName.Equals(""))
        {
            var csvFileName = new FileInfo(ofd.FileName);
            CSVData csvFile = new(csvFileName);
            csvFile.Fill();

            _manager.AddCSV(csvFile);
            Count++;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(csvFileName.Name);
            AddButtonToStack(fileNameWithoutExtension, typeof(CSVData));
        }
    }

    private void OnClickLdapBtn()
    {
        if (_lgi.ExistsSearcher())
        {
            ConditionViewModel conVM = new(_lgi);

            ConditionWindow condWin = new()
            {
                DataContext = conVM,
            };
            conVM.CloseWindowRequested += condWin.CloseWindowHandler;
            condWin.ShowDialog();

            var result = conVM.GetResult();
            _manager.AddSearchResultCollection(result);
            Count++;
            string name = conVM.Filter;
            AddButtonToStack(name, typeof(SearchResultCollection));
        }
        else
        {
            MessageBox.Show("You have to loggin!.");
        }
    }

    private void OnClickBtn(int index) => Output = _manager.WriteLDAP(index);

    private void OnClickButton(int index) => Output = _manager.WriteCSV(index);

    private void AddButtonToStack(string content, Type type) => DynamicButtons.Add(new ButtonViewModel(content, ClickCommand, _dynamicButtons.Count, type) /*{ Content = content, Command = ClickCommand, Index = _dynamicButtons.Count, Type = type }*/);
}

public class ButtonViewModel
{
    public string Content { get; set; }
    public ICommand Command { get; set; }
    public int Index { get; set; }
    public Type Type { get; set; }

    public ButtonViewModel(string content, ICommand command, int index, Type type)
    {
        Content = content;
        Command = command;
        Index = index;
        Type = type;
    }
}