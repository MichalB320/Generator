using System.Security.Cryptography;

namespace ClassLibrary;

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
}
