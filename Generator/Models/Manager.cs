//using ClassLibrary;
using System.DirectoryServices;
using System.Text;

namespace Generator.Models;

public class Manager
{
    private Mystructure _struct;
    public Mystructure Structure { get => _struct; }

    //private ObservableCollection<ButtonViewModel> _dynamicButtons;
    //public ObservableCollection<ButtonViewModel> DynamicButtons
    //{
    //    get => _dynamicButtons;
    //    set
    //    {
    //        if (_dynamicButtons != value)
    //        {
    //            _dynamicButtons = value;
    //        }
    //    }
    //}

    public Manager(ref Mystructure structure)
    {
        _struct = structure;
        //_dynamicButtons = new ObservableCollection<ButtonViewModel>();
    }

    public async Task<string> WriteLDAP(int index)
    {
        SearchResultCollection results = _struct.GetItem<SearchResultCollection>(index);

        StringBuilder sb = new();

        //foreach (SearchResult result in results)
        //{
        //    foreach (string propertyName in result.Properties.PropertyNames)
        //        sb.Append($"{propertyName};");
        //}

        //foreach (SearchResult result in results)
        //{
        //    await Task.Run(() =>
        //    {
        //        foreach (string propertyName in result.Properties.PropertyNames)
        //        {
        //            foreach (var propertyValue in result.Properties[propertyName])
        //                sb.Append($"{propertyValue};");
        //        }
        //        sb.Append("\n");
        //    });
        //}

        foreach (SearchResult result in results)
            await Task.Run(() => sb.Append(result.Properties["cn"][0] + "\n"));

        return sb.ToString();
    }

    public string WriteCSV(int index)
    {
        CSVData csv = _struct.GetItem<CSVData>(index);

        StringBuilder sb = new();
        foreach (var row in csv.GetRows())
        {
            foreach (var coll in row)
                sb.Append($"{coll};");

            sb.Append("\n");
        }

        return sb.ToString();
    }

    public void AddCSV(CSVData csvData)
    {
        _struct.Add<CSVData>(csvData);
    }

    public void AddSearchResultCollection(SearchResultCollection ldapData)
    {
        _struct.Add<SearchResultCollection>(ldapData);
    }

    public void Clear() => _struct.Clear();
}
