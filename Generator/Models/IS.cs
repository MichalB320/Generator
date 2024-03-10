using ClassLibrary;
using Generator.ViewModels;
using System.Collections.ObjectModel;
using System.DirectoryServices;

namespace Generator.Models;

public class IS
{
    private readonly Login _login;
    private readonly Manager _manager;
    private readonly Generator _generator;
    private Mystructure _structure;

    public ObservableCollection<ButtonViewModel> DynamicButtons { get; set; }
    public string Info { get; set; }

    public IS()
    {
        _structure = new Mystructure();
        _login = new Login();
        _manager = new Manager(ref _structure);
        _generator = new Generator("", ref _structure, new System.Collections.ObjectModel.ObservableCollection<ViewModels.ButtonViewModel>());

        Info = "logged out";
        //DynamicButtons = _manager.DynamicButtons;
        DynamicButtons = new ObservableCollection<ButtonViewModel>();
    }

    public async Task<string> WriteLDAP(int index) => await _manager.WriteLDAP(index);

    public void WriteCSV(int index)
    {
        _manager.WriteCSV(index);
    }

    public void AddCSV(CSVData csvData) => _manager.AddCSV(csvData);

    public void AddSearchResulCollection(SearchResultCollection ldapData) => _manager.AddSearchResultCollection(ldapData);

    public string Login(string ldapPath, string userName, string password) => _login.LogIn(ldapPath, userName, password);

    public Login GetLogin() => _login;

    public Manager GetManager() => _manager;

    public Generator GetGenerator() => _generator;

    public ref Mystructure GetStructure() => ref _structure;
}
