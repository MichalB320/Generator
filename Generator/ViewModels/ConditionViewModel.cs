using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Generator.Models;
using System.DirectoryServices;
using System.Windows;
using System.Windows.Input;

namespace Generator.ViewModels;

public class ConditionViewModel : ObservableObject
{
    private Login _lgi;
    private SearchResultCollection? _result;

    private string _filter;
    public string Filter
    {
        get => _filter;
        set
        {
            _filter = value;
            OnPropertyChanged(nameof(Filter));
        }
    }
    public string Info { get; private set; }
    public string Alias { get; set; }
    public ICommand SearchCommand { get; set; }

    public delegate void CloseWindowEventHandler();
    public event CloseWindowEventHandler? CloseWindowRequested;

    public ConditionViewModel(Login lgi)
    {
        _lgi = lgi;
        _filter = "";
        SearchCommand = new RelayCommand(Search);
        Info = "OK filter";
        Alias = "";
    }

    private void Search()
    {
        try
        {
            var filter = Filter;
            _result = _lgi.Search(filter);
            CloseWindowRequested?.Invoke();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public SearchResultCollection GetResult()
    {
        return _result;
    }
}
