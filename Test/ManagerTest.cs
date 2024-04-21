using GeneratorApp.Models;
using System.DirectoryServices;

namespace Test;

public class ManagerTest
{
    [Fact]
    public async Task WriteLDAP_Test()
    {
        Mystructure structure = new Mystructure();
        var manager = new Manager(ref structure);
        var lgi = new Login();

        lgi.LogIn("LDAP://pegasus.fri.uniza.sk", "meno", "heslo"); // prepisat meno a heslo
        SearchResultCollection results = lgi.Search("(cn=)"); //treba dopisat cn nejakej osoby
        manager.AddSearchResultCollection(results);
        var result = await manager.WriteLDAP(0);

        Assert.Equal("cnOsoby\n", result);//treba dopisa cn nejakej osoby
    }

    [Fact]
    public void WriteCSV_Test()
    {
        Mystructure structure = new Mystructure();
        var manager = new Manager(ref structure);

        var csvRows = new List<List<string>>
            {
                new List<string> { "Name", "Age" },
                new List<string> { "John", "30" },
                new List<string> { "Jane", "25" }
            };

        CSVData csvData = new CSVData(csvRows);
        manager.AddCSV(csvData);

        string expectedOutput = "Name;Age;\nJohn;30;\nJane;25;\n";
        string result = manager.WriteCSV(0);

        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public void AddSearchResultCollection_Test()
    {
        Mystructure structure = new Mystructure();
        var manager = new Manager(ref structure);
        var lgi = new Login();

        lgi.LogIn("LDAP://pegasus.fri.uniza.sk", "meno", "heslo"); // prepisat meno a heslo
        SearchResultCollection results = lgi.Search("(cn=)");  //treba dopisat cn nejakej osoby
        manager.AddSearchResultCollection(results);

        Assert.True(manager.Structure.Contains(results));
    }

    [Fact]
    public void AddCSV_Test()
    {
        Mystructure structure = new Mystructure();
        var manager = new Manager(ref structure);
        var csvRows = new List<List<string>>
            {
                new List<string> { "Name", "Age" },
                new List<string> { "John", "30" },
                new List<string> { "Jane", "25" }
            };
        CSVData csvData = new CSVData(csvRows);
        manager.AddCSV(csvData);

        Assert.True(manager.Structure.Contains(csvData));
    }

    [Fact]
    public void Clear_Test()
    {
        Mystructure structure = new Mystructure();
        var manager = new Manager(ref structure);

        var csvRows = new List<List<string>>
            {
                new List<string> { "Name", "Age" },
                new List<string> { "John", "30" },
                new List<string> { "Jane", "25" }
            };
        CSVData csvData = new CSVData(csvRows);
        manager.AddCSV(csvData);

        var lgi = new Login();

        lgi.LogIn("LDAP://pegasus.fri.uniza.sk", "meno", "heslo"); // prepisat meno a heslo
        SearchResultCollection results = lgi.Search("(cn=)"); //treba dopisat cn nejakej osoby
        manager.AddSearchResultCollection(results);

        Assert.True(manager.Structure.Contains(csvData));
        Assert.True(manager.Structure.Contains(results));
        manager.Clear();
        Assert.Equal(0, manager.Structure.Count);
    }
}
