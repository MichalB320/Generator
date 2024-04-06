using System.IO;
using System.Text;

namespace Generator.Models;

public class CSVData
{
    private FileInfo _csvFile;
    private List<List<string>> _rowsCSV;
    public int Count { get => _rowsCSV.Count; }

    public CSVData(FileInfo fileInfo)
    {
        _rowsCSV = new();
        _csvFile = fileInfo;
    }

    public CSVData()
    {
        _rowsCSV = new();
    }

    //public CSVData(string content)
    //{
    //    _rowsCSV = new();
    //    var rows = content.Split('\n');
    //    //var items = new List<string>();


    //    foreach (var row in rows)
    //    {
    //        List<string> itemsL = new();
    //        var items = row.Split(';');
    //        foreach (var item in items)
    //            itemsL.Add(item);

    //        _rowsCSV.Add(itemsL);
    //    }
    //}

    public CSVData(List<List<string>> rows)
    {
        _rowsCSV = rows;
    }

    public void Fill()
    {
        using (StreamReader sr = new(_csvFile.FullName))
        {
            string? line;

            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();

                if (line != null)
                {
                    string[] parts = line.Split(';');

                    List<string> row = new();
                    foreach (string part in parts)
                        row.Add(part);

                    _rowsCSV.Add(row);
                }
            }
        }
    }

    public void Clear()
    {

    }

    public void AddRow(List<string> row) => _rowsCSV.Add(row);

    public void AddRows(List<List<string>> rows) => _rowsCSV = rows;

    public void RemoveRow(int index) => _rowsCSV.RemoveAt(index);

    public List<List<string>> GetRows() => _rowsCSV;

    public List<string> GetRow(int index)
    {
        if (Count > index)
            return _rowsCSV[index];
        else
            return null;
    }

    public List<string> GetRow(string keyName, string key)
    {
        int indexCsvStlpca = _rowsCSV[0].IndexOf(keyName);

        foreach (List<string> row in _rowsCSV)
        {
            if (row[indexCsvStlpca] == key)
            {
                return row;
            }
        }

        return null;
    }

    public string String()
    {
        StringBuilder sb = new();

        foreach (List<string> row in _rowsCSV)
        {
            foreach (string item in row)
            {
                sb.Append($"{item};");
            }
            sb.Append("\n");
        }

        return sb.ToString();
    }

    public CSVData DeepCopy()
    {
        var copiedRows = new List<List<string>>();

        foreach (var row in _rowsCSV)
        {
            var copiedRow = new List<string>(row);
            copiedRows.Add(copiedRow);
        }

        return new CSVData(copiedRows);
    }
}
