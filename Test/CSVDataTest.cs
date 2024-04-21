using GeneratorApp.Models;

namespace Test;

public class CSVDataTest : IDisposable
{
    private readonly string _testFilePath = "test.csv";
    private CSVData _csvData;

    public CSVDataTest()
    {
        File.WriteAllText(_testFilePath, "Name;Age\nJohn;30\nJane;25");

        _csvData = new CSVData(new FileInfo(_testFilePath));
        _csvData.Fill();
    }

    [Fact]
    public void AddRow_test()
    {
        var newRow = new List<string> { "Jack", "40" };
        _csvData.AddRow(newRow);
        Assert.Equal(4, _csvData.Count);
    }

    [Fact]
    public void RemoveRow_test()
    {
        _csvData.RemoveRow(0);
        Assert.Equal(2, _csvData.Count);
    }

    [Fact]
    public void GetRows_test()
    {
        var expectedRows = new List<List<string>>
        {
            new List<string> { "Name", "Age" },
            new List<string> { "John", "30" },
            new List<string> { "Jane", "25" }
        };

        var actualRows = _csvData.GetRows();

        Assert.Equal(expectedRows, actualRows);

        for (int i = 0; i < expectedRows.Count; i++)
        {
            Assert.Equal(expectedRows[i], actualRows[i]);
        }
    }

    [Fact]
    public void GetRow_test()
    {
        var expectedRow = new List<string>
        {
            "John", "30"
        };
        var actualRow = _csvData.GetRow(1);
        Assert.Equal(expectedRow, actualRow);
    }

    [Fact]
    public void String_test()
    {
        string expectedCsvString = "Name;Age;\nJohn;30;\nJane;25;\n";
        string actualCsvString = _csvData.String();
        Assert.Equal(expectedCsvString, actualCsvString);
    }

    [Fact]
    public void DeepCopy_Test()
    {
        var copy = _csvData.DeepCopy();
        Assert.NotSame(_csvData, copy);
        Assert.Equal(_csvData.Count, copy.Count);
    }

    public void Dispose()
    {
        File.Delete(_testFilePath);
    }
}
