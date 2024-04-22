//using ClassLibrary;
using CommunityToolkit.Mvvm.Input;
using Generator;
using GeneratorApp.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GeneratorApp.ViewModels;

public class SourcesManagerViewModel : ViewModelBase
{
    private readonly Mystructure _mystructure = new Mystructure();
    private readonly IS _is;

    public ICommand LdapCommand { get; }
    public ICommand CsvCommand { get; }
    public ICommand EVCommand { get; }
    public ICommand ClickCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand DeleteAllCommand { get; }

    private string _output = string.Empty;
    public string Output { get => _output; set { _output = value; OnPropertyChanged(nameof(Output)); } }

    public ObservableCollection<ButtonViewModel> DynamicButtons
    {
        get => _is.DynamicButtons;
        set
        {
            if (_is.DynamicButtons != value)
            {
                _is.DynamicButtons = value;
                OnPropertyChanged(nameof(DynamicButtons));
            }
        }
    }
    private int _count = 0;
    public int Count { get => _count; set { _count = value; OnPropertyChanged(nameof(Count)); } }

    public NavigationBarViewModel NavigationBarViewModel { get; }

    public SourcesManagerViewModel(NavigationBarViewModel navigationBarViewModel, IS iss)
    {
        NavigationBarViewModel = navigationBarViewModel;
        _is = iss;

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
        DeleteCommand = new RelayCommand<ButtonViewModel>(async (parameter) =>
        {
            int index = -1;
            for (int i = 0; i < _is.GetStructure().Count; i++)
            {
                if (_is.GetStructure().GetTypeOf(i) == typeof(CSVData))
                {
                    string str = _mystructure.GetItem<CSVData>(i).String();
                    if (str == Output)
                        index = i;
                }
                else
                {
                    string str = await _is.WriteLDAP(i);
                    if (str == Output)
                        index = i;
                }
            }

            OnDeleteClick(index);
        });
        EVCommand = new RelayCommand(OnClickEVBtn);
        DeleteAllCommand = new RelayCommand(OnClickDeleteAll);
    }

    private void OnClickDeleteAll()
    {
        DynamicButtons.Clear();
        _is.ClearDataStack();
        _mystructure.Clear();
        Count = 0;
        Output = string.Empty;
    }

    private void OnDeleteClick(int index)
    {
        DynamicButtons.RemoveAt(index);
        _is.GetManager().Structure.RemoveAt(index); //_manager.Structure.RemoveAt(index);
        _mystructure.RemoveAt(index);
        Count--;

        int i = 0;
        foreach (var button in DynamicButtons)
        {
            button.Index = i;
            i++;
        }
        Output = "";
    }

    private void OnClickCsvBtn()
    {
        try
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "CSV súbory (*.csv) | *.csv";
            ofd.ShowDialog();

            if (!ofd.FileName.Equals(""))
            {
                var csvFileName = new FileInfo(ofd.FileName);
                CSVData csvFile = new(csvFileName);
                CSVData csv = new(csvFileName);
                csvFile.Fill();
                csv.Fill();

                _is.AddCSV(csvFile);
                Count++;
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(csvFileName.Name);
                AddButtonToStack(fileNameWithoutExtension, typeof(CSVData));
                //---------------
                _mystructure.Add(csv);
            }
        }
        catch (IOException e)
        {
            MessageBox.Show(Application.Current.FindResource("fileException") as string);
        }
    }
    private void OnClickEVBtn()
    {
        UrlViewModel urlVM = new();

        UrlWindow urlWin = new()
        {
            DataContext = urlVM,
        };
        urlVM.CloseWindowRequested += urlWin.CloseWindowHandler;
        urlWin.ShowDialog();
        string name = urlVM.Alias;//"predmety";
        if (name.Equals(""))
        {
            MessageBox.Show(Application.Current.FindResource("emptyAlias") as string);
        }
        else
        {
            //urlVM.GetCSVFormat();
            CSVData csv = new(urlVM.GetCSVFormat());

            _is.AddCSV(csv);
            Count++;

            AddButtonToStack(name, typeof(CSVData));
            //------
            _mystructure.Add(csv);
        }
    }

    private void OnClickLdapBtn()
    {
        if (_is.GetLogin().ExistsSearcher())
        {
            ConditionViewModel conVM = new(_is.GetLogin());

            ConditionWindow condWin = new()
            {
                DataContext = conVM,
            };
            conVM.CloseWindowRequested += condWin.CloseWindowHandler;
            condWin.ShowDialog();

            try
            {
                SearchResultCollection result = conVM.GetResult();
                string filter = conVM.Filter;
                if (!string.IsNullOrEmpty(filter) && result.Count > 0)
                {
                    _is.AddSearchResulCollection(result);
                    _mystructure.Add(result);
                    Count++;
                    string name = conVM.Alias; //conVM.Filter;
                    AddButtonToStack(name, typeof(SearchResultCollection));
                }
                else
                {
                    MessageBox.Show(Application.Current.FindResource("nothingFound") as string, Application.Current.FindResource("warning") as string, MessageBoxButton.OK, MessageBoxImage.Warning);
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
            MessageBox.Show(Application.Current.FindResource("loginWarning") as string, "stop", MessageBoxButton.OK, MessageBoxImage.Stop);
        }
    }

    private async void OnClickBtn(int index)
    {
        string output = await _is.WriteLDAP(index); //_manager.WriteLDAP(index);
        Output = output;
    }

    private void OnClickButton(int index) //=> this.Output = _manager.WriteCSV(index);
    {
        CSVData csv = _mystructure.GetItem<CSVData>(index);

        StringBuilder sb = new();
        foreach (var row in csv.GetRows())
        {
            foreach (var coll in row)
                sb.Append($"{coll};");

            sb.Append("\n");
        }

        Output = sb.ToString();
    }

    private void AddButtonToStack(string content, Type type) => DynamicButtons.Add(new ButtonViewModel(content, ClickCommand, DynamicButtons.Count, type));
}