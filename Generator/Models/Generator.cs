using Generator.ViewModels;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Text;
using System.Windows;

namespace Generator.Models;

public class Generator
{
    private string[][] _pole;
    private string _input;
    private List<string> _strings;
    private List<string> _variables;
    private List<string> _sources;

    private Mystructure _stlpce;

    private Mystructure _structure;
    private ObservableCollection<ButtonViewModel> _buttons;

    private List<CSVData> _csvS;

    private Dictionary<string, int> _csvSIndexes;

    public Generator(string input, Mystructure structure, ObservableCollection<ButtonViewModel> buttons)
    {
        _input = input;
        _strings = new();
        _variables = new();
        _sources = new();
        _structure = structure;
        //_structure = new(); 
        //for (int i = 0; i < structure.Count; i++)
        //{
        //    if (structure.GetTypeOf(i) == typeof(CSVData))
        //    {
        //        _structure.Add<CSVData>(structure.GetItem<CSVData>(i));
        //    }
        //    else
        //    {
        //        _structure.Add<SearchResultCollection>(structure.GetItem<SearchResultCollection>(i));
        //    }

        //}
        _buttons = buttons;

        _stlpce = new();
        _csvS = new();
        _csvSIndexes = new();
    }

    public void setValues(string input, ObservableCollection<ButtonViewModel> buttons)
    {
        _input = input;
        _buttons = buttons;
        _strings.Clear();
        _variables.Clear();
        _sources.Clear();
        _csvS.Clear();
        _csvSIndexes.Clear();
    }

    public string Generate(char specialChar)
    {
        string input = _input;

        StringBuilder sb = new();
        try
        {
            for (int j = 0; j < _pole[0].Length; j++)
            {
                string text = input;
                for (int i = 0; i < _strings.Count; i++)
                    text = text.Replace(_strings[i], _pole[i][j]);

                text = text.Replace(Convert.ToString(specialChar), "");
                sb.Append($"{text}\n");
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
        return sb.ToString();
    }

    public void PrepareVariable()
    {
        try
        {
            Mystructure stlpce = new();

            _pole = new string[][]
            {
            new string[] { "michal", "maria", "jozko" },
            new string[] { "513", "514", "515"},
            new string[] { "559550", "559024", "559011"}
            };

            List<string> foundMatch = new List<string>();

            foreach (ButtonViewModel btnVM in _buttons)
            {
                if (_sources.Contains(btnVM.Content))
                {
                    foundMatch.Add(btnVM.Content);
                }
            }

            int indexSource = 0;
            foreach (string source in _sources)
            {
                ButtonViewModel btn = _buttons.FirstOrDefault(BtnVM => BtnVM.Content == _sources[indexSource]);

                int index = _buttons.IndexOf(btn);

                if (_buttons[index].Type == typeof(CSVData))
                {
                    //CSVData csv = _structure.GetItem<CSVData>(index);
                    int csvSIndex = _csvSIndexes[_buttons[index].Content];
                    CSVData csv = _csvS[csvSIndex]; // pada because of _csvS[index];
                    stlpce.Add(csv);
                }
                if (_buttons[index].Type == typeof(SearchResultCollection))
                {
                    //SearchResultCollection ldap = _structure.GetItem<SearchResultCollection>(index);
                    int csvSIndex = _csvSIndexes[_buttons[index].Content];
                    CSVData csv = _csvS[csvSIndex]; // pada because of _csvS[index];
                    //stlpce.Add(ldap);

                    stlpce.Add(csv);
                }
                indexSource++;
            }


            _stlpce = stlpce;

            Join();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public async Task FindStrings(char specialChar)
    {
        await Task.Run(() =>
        {
            int startI = _input.IndexOf(specialChar);
            int count = _input.Count(c => c == specialChar);

            if (count % 2 != 0 || startI == -1)
                throw new Exception(Application.Current.FindResource("specialChar") as string);

            for (int i = 0; i < count / 2; i++)
            {
                int startIndex = startI;
                int endIndex = _input.IndexOf(specialChar, startIndex + 1);
                startI = _input.IndexOf(specialChar, endIndex + 1);
                string subStr = _input.Substring(startIndex + 1, endIndex - 1 - startIndex);
                _strings.Add(subStr);
            }
        });
    }

    public async Task FindSourcesAndVariables(char delimiter)
    {
        await Task.Run(() =>
        {
            foreach (string str in _strings)
            {
                if (str.Contains(delimiter))
                {
                    string[] parts = str.Split(delimiter);
                    _sources.Add(parts[0]);
                    _variables.Add(parts[1]);
                }
                else
                    throw new Exception(Application.Current.FindResource("deliminerChar") as string);
            }
        });
    }

    public void Join()
    {
        List<string> join = new();
        string[][] matrix = new string[_stlpce.Count][];
        for (int i = 0; i < _stlpce.Count; i++)
        {
            matrix[i] = new string[_csvS[0].Count]; // 24
        }

        for (int i = 0; i < _stlpce.Count; i++)
        {
            for (int j = 0; j < _csvS[0].Count; j++) // 24
            {
                string v1 = "";
                StringBuilder sb = new StringBuilder();

                Type typ = _stlpce.GetTypeOf(i);
                if (typ != typeof(CSVData))
                {
                    SearchResultCollection results = _stlpce.GetItem<SearchResultCollection>(i);
                    //foreach (SearchResult result in results)
                    //{
                    //    DirectoryEntry resultEntry = result.GetDirectoryEntry();
                    //    string propName = _variables[i];
                    //    v1 = (string)resultEntry.InvokeGet(propName).ToString();
                    //}

                    SearchResult result = results[j];
                    DirectoryEntry resultEntry = result.GetDirectoryEntry();
                    string propName = _variables[i];

                    PropertyCollection properties = resultEntry.Properties;

                    if (properties.Contains(propName))
                    {
                        v1 = (string)resultEntry.InvokeGet(propName).ToString();
                    }
                    else
                    {
                        v1 = "";
                    }

                    //v1 = (string)resultEntry.InvokeGet(propName).ToString();

                    //v1 = "LDAP";
                }
                if (typ == typeof(CSVData))
                {
                    int indexCsvStlpca = _stlpce.GetItem<CSVData>(i).GetRow(0).IndexOf(_variables[i]);
                    v1 = _stlpce.GetItem<CSVData>(i).GetRow(j)[indexCsvStlpca];//[stlpecCSV]
                }

                matrix[i][j] = v1;
            }
        }
        _pole = matrix;
    }

    public static bool ObsahujeStringy(List<string> list, ButtonViewModel button)
    {
        foreach (var item in list)
        {
            if (button.Content == item)
            {
                return true;
            }
        }
        return false;
    }

    public async Task JoinOn(string csvKey, string ldapKey, IProgress<int> proggres)
    {
        List<SearchResultCollection> ldapS = new();
        List<CSVData> csvS = new();

        int value = 50;
        for (int i = 0; i < _structure.Count; i++)
        {
            proggres.Report(value);
            if (_structure.GetTypeOf(i) == typeof(CSVData) && ObsahujeStringy(_sources, _buttons[i]))
            {
                CSVData subor = _structure.GetItem<CSVData>(i).DeepCopy();
                csvS.Add(subor);
                _csvSIndexes.Add(_buttons[i].Content, csvS.IndexOf(subor));
            }
            else if (_structure.GetTypeOf(i) != typeof(CSVData) && ObsahujeStringy(_sources, _buttons[i]))
            {
                SearchResultCollection ldap = _structure.GetItem<SearchResultCollection>(i);

                CSVData csv = new();

                List<string> propertyNames = new();
                SearchResult result = ldap[0];
                DirectoryEntry resultEntry = result.GetDirectoryEntry();
                await Task.Run(() =>
                {
                    foreach (string propertyName in resultEntry.Properties.PropertyNames)
                        propertyNames.Add(propertyName);

                    proggres.Report(60);
                });
                csv.AddRow(propertyNames);

                foreach (SearchResult vysledok in ldap)
                {
                    List<string> values = new();
                    DirectoryEntry vyslednyVstup = vysledok.GetDirectoryEntry();
                    await Task.Run(() =>
                    {
                        foreach (string propertyName in propertyNames)
                        {
                            values.Add(vyslednyVstup.InvokeGet(propertyName) + "");
                        }
                        proggres.Report(80);
                    });
                    csv.AddRow(values);
                }

                csvS.Add(csv);
                _csvSIndexes.Add(_buttons[i].Content, csvS.IndexOf(csv));
            }
            value += 10;
        }

        string[][] kluce = new string[csvS.Count][];

        // 1 krok a 2 krok
        int index = 0;
        foreach (CSVData csv in csvS)
        {
            kluce[index] = new string[csv.Count];
            for (int i = 1; i < csv.GetRows().Count; i++)
            {
                int indexCsvStlpca;
                if (csv.GetRow(0).Contains(csvKey))
                    indexCsvStlpca = csv.GetRow(0).IndexOf(csvKey);
                else
                    indexCsvStlpca = csv.GetRow(0).IndexOf(ldapKey);

                string kluc;

                if (indexCsvStlpca > csv.GetRow(i).Count || indexCsvStlpca < 0)
                    kluc = "";
                else
                    kluc = csv.GetRow(i)[indexCsvStlpca];

                kluce[index][i - 1] = kluc;
            }
            index++;
        }

        // krok 3
        if (csvS.Count > 1)
        {
            List<string> spolocnePole = NajdiSpolocnePrvky(kluce);

            // krok 4 a krok 5
            foreach (var csv in csvS)
            {
                for (int i = csv.Count - 1; i > 0; i--)
                {
                    int indexCsvStlpca;
                    if (csv.GetRow(0).Contains(csvKey))
                        indexCsvStlpca = csv.GetRow(0).IndexOf(csvKey);
                    else
                        indexCsvStlpca = csv.GetRow(0).IndexOf(ldapKey);

                    if (indexCsvStlpca > csv.GetRow(i).Count || indexCsvStlpca < 0)
                        csv.RemoveRow(i);
                    else if (!spolocnePole.Contains(csv.GetRow(i)[indexCsvStlpca]))
                        csv.RemoveRow(i);
                }
            }


            // krok 6 a krok 7
            List<CSVData> csvS2 = new();
            foreach (CSVData csv in csvS)
            {
                List<List<string>> polePoli = csv.GetRows();

                int indexCsvStlpca = csv.GetRow(0).IndexOf(ldapKey);

                if (csv.GetRow(0).Contains(ldapKey))
                    indexCsvStlpca = csv.GetRow(0).IndexOf(ldapKey);
                else
                    indexCsvStlpca = csv.GetRow(0).IndexOf(csvKey);
                try
                {
                    polePoli = polePoli.Skip(1).OrderBy(list => int.Parse(list[indexCsvStlpca])).ToList();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                polePoli.Insert(0, csv.GetRow(0));
                CSVData newCsv = new(polePoli);
                csvS2.Add(newCsv);
            }

            //_csvS = csvS;
            _csvS = csvS2;
        }
        else
        {
            _csvS = csvS;
        }
    }

    static List<string> NajdiSpolocnePrvky(string[][] pole)
    {
        List<string> spolocnePole = new List<string>();

        if (pole.Length > 0)
        {
            foreach (string prvek in pole[0])
            {
                if (VsetkyPoleObsahuju(prvek, pole))
                {
                    spolocnePole.Add(prvek);
                }
            }
        }

        return spolocnePole;
    }

    static bool VsetkyPoleObsahuju(string prvek, string[][] pole)
    {
        foreach (string[] jednoPole in pole)
        {
            if (!Obsahuje(prvek, jednoPole))
            {
                return false;
            }
        }
        return true;
    }

    static bool Obsahuje(string prvek, string[] jednoPole)
    {
        return Array.IndexOf(jednoPole, prvek) != -1;
    }

    internal bool SourcesExists()
    {
        return true;
    }

    internal void CheckVariables()
    {

    }

    internal void Save()
    {

    }
}
