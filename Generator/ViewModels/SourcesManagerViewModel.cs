using ClassLibrary;
using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using GeneratorApp.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Generator.ViewModels;

public class SourcesManagerViewModel : ViewModelBase
{
    private readonly Mystructure _mystructure;
    private readonly IS _is;

    public ICommand LdapCommand { get; }
    public ICommand CsvCommand { get; }
    public ICommand EVCommand { get; }
    public ICommand ClickCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand ShowDataGridCommand { get; }
    public ICommand ShowTextBoxCommand { get; }

    public ICommand DeleteAllCommand { get; }

    private bool _isReadOnly = true;
    public bool IsReadOnly { get => _isReadOnly; set { _isReadOnly = value; OnPropertyChanged(nameof(IsReadOnly)); } }
    private Visibility _textBoxVisibility;
    public Visibility TextBoxVisibility { get => _textBoxVisibility; set { _textBoxVisibility = value; OnPropertyChanged(nameof(TextBoxVisibility)); } }
    private Visibility _dataGridVisibility;
    public Visibility DataGridIsVisibility { get => _dataGridVisibility; set { _dataGridVisibility = value; OnPropertyChanged(nameof(DataGridIsVisibility)); } }
    public ObservableCollection<CSVData> Data { get; set; } = new ObservableCollection<CSVData>();
    public ObservableCollection<RowData> DataRows { get; set; } 

    private string _output;
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
    private int _count;
    public int Count { get => _count; set { _count = value; OnPropertyChanged(nameof(Count)); } }

    public NavigationBarViewModel NavigationBarViewModel { get; }

    public SourcesManagerViewModel(NavigationBarViewModel navigationBarViewModel, IS iss)
    {
        _is = iss;
        _output = "";
        _count = 0;

        DataRows = new ObservableCollection<RowData>
        {
            new RowData(new List<string>{"A1", "B1", "C1"}),
            new RowData(new List<string>{"A2", "B2", "C2"}),
            new RowData(new List<string>{"A3", "B3", "C3"}),
        };

        _mystructure = new Mystructure();

        //CSVData da = new();
        //var nieco = da.GetRow(0)[1];

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

            if (parameter != null)
            {

            }
        });
        EVCommand = new RelayCommand(OnClickEVBtn);
        EditCommand = new RelayCommand(OnClickEditBtn);
        DeleteAllCommand = new RelayCommand(OnClickDeleteAll);
        ShowDataGridCommand = new RelayCommand(OnClickDataGrid);
        ShowTextBoxCommand = new RelayCommand(OnClickTextBox);
        NavigationBarViewModel = navigationBarViewModel;
        TextBoxVisibility = Visibility.Visible;
        DataGridIsVisibility = Visibility.Collapsed;
    }

    private void OnClickTextBox()
    {
        //MessageBox.Show("textBox");
        TextBoxVisibility = Visibility.Visible;
        DataGridIsVisibility = Visibility.Collapsed;
    }

    private void OnClickDataGrid()
    {
        //MessageBox.Show("DataGrid");
        TextBoxVisibility = Visibility.Collapsed;
        DataGridIsVisibility = Visibility.Visible;
    }

    private void OnClickDeleteAll()
    {
        DynamicButtons.Clear();
        _is.ClearDataStack();
        _mystructure.Clear();
        Count = 0;
        Output = string.Empty;
    }

    private void OnClickEditBtn()
    {
        if (IsReadOnly)
        {
            IsReadOnly = false;
        }
        else
            IsReadOnly = true;
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
            _mystructure.Add<CSVData>(csv);
        }
    }

    private void OnClickEVBtn()
    {
        UrlViewModel urlVM = new();

        UrlWindow urlWin = new()
        {
            DataContext = urlVM,
        };

        urlWin.ShowDialog();

        urlVM.GetCSVFormat();
        //string vars = urlVM.WebData;
        //string vars = urlVM.People.ToString();
        //Output = vars;
        CSVData csv = new(urlVM.csv);
        _is.AddCSV(csv);
        Count++;
        string name = urlVM.Alias;//"predmety";
        AddButtonToStack(name, typeof(CSVData));
        //------
        _mystructure.Add<CSVData>(csv);
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
                    _mystructure.Add<SearchResultCollection>(result);
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
        
        Data.Add(_mystructure.GetItem<CSVData>(index));
    }

    private void AddButtonToStack(string content, Type type) => DynamicButtons.Add(new ButtonViewModel(content, ClickCommand, DynamicButtons.Count, type));
}