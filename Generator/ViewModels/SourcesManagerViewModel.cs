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
    private readonly Login _lgi;
    private readonly Manager _manager;

    public ICommand LdapCommand { get; }
    public ICommand CsvCommand { get; }
    public ICommand ClickCommand { get; }
    //public ICommand NextCommand { get; }
    //public ICommand PreviousCommand { get; }
    public ICommand DeleteCommand { get; }

    private string _output;
    public string Output { get => _output;
        set 
        { 
            _output = value; 
            OnPropertyChanged(nameof(Output)); 
        } 
    }

    public ObservableCollection<ButtonViewModel> DynamicButtons
    {
        get => _manager.DynamicButtons;
        set
        {
            if (_manager.DynamicButtons != value)
            {
                _manager.DynamicButtons = value;
                OnPropertyChanged(nameof(DynamicButtons));
            }
        }
    }
    private int _count;
    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            OnPropertyChanged(nameof(Count));
        }
    }

    public NavigationBarViewModel NavigationBarViewModel { get; }

    public SourcesManagerViewModel(NavigationStore navigation, NavigationBarViewModel navigationBarViewModel, IS iss)
    {
        _lgi = iss.GetLogin();
        _manager = iss.GetManager();
        _output = "";
        _count = 0;

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
        //NextCommand = new RelayCommand(() => navigation.CurrentViewModel = new GenerateViewModel(_manager.Structure, DynamicButtons, navigation, navigationBarViewModel, iss.GetLogin()/*login*/, iss));
        //PreviousCommand = new RelayCommand(() => navigation.CurrentViewModel = new LoginViewModel(/*navigation,*/ navigationBarViewModel, iss));
        DeleteCommand = new RelayCommand<ButtonViewModel>(async (parameter) =>
        {
            int index = -1;
            for (int i = 0; i < _manager.Structure.Count; i++)
            {
                if (_manager.Structure.GetTypeOf(i) == typeof(CSVData))
                {
                    string str = _manager.Structure.GetItem<CSVData>(i).String();
                    if (str == Output)
                        index = i;
                }
                else
                {
                    //string str = _manager.Structure.GetItem<SearchResultCollection>(i);

                    string str = await _manager.WriteLDAP(i);
                    if (str == Output)
                        index = i;
                }
            }


            OnDeleteClick(index);

            if (parameter != null)
            {
                
            }
        });
        NavigationBarViewModel = navigationBarViewModel;
        //navigationBarViewModel.Visible();
        //Update();
    }

    //private void Update()
    //{
    //    _count = _manager.DynamicButtons.Count;
    //    DynamicButtons = _manager.DynamicButtons;
    //}

    private void OnDeleteClick(int index)
    {
        DynamicButtons.RemoveAt(index);
        _manager.Structure.RemoveAt(index);
        Count--;

        int i = 0;
        foreach(var button in DynamicButtons)
        { 
            button.Index = i;
            i++;
        }
        Output = "";
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

            try
            {
                var result = conVM.GetResult();
                var filter = conVM.Filter;
                if (!string.IsNullOrEmpty(filter) && result.Count > 0)
                {
                    _manager.AddSearchResultCollection(result);
                    Count++;
                    string name = conVM.Filter;
                    AddButtonToStack(name, typeof(SearchResultCollection));
                }
                else
                {
                    MessageBox.Show("nothing found", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        else
        {
            MessageBox.Show("You have to loggin!.", "stop", MessageBoxButton.OK, MessageBoxImage.Stop);
        }
    }

    private async void OnClickBtn(int index)
    {
        string output = await _manager.WriteLDAP(index);
        this.Output = output;
    }

    private void OnClickButton(int index) => this.Output = _manager.WriteCSV(index);

    private void AddButtonToStack(string content, Type type) => DynamicButtons.Add(new ButtonViewModel(content, ClickCommand, DynamicButtons.Count, type) /*{ Content = content, Command = ClickCommand, Index = _dynamicButtons.Count, Type = type }*/);
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