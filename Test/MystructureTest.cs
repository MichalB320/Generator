using GeneratorApp.Models;

namespace Test;

public class MystructureTest
{

    [Fact]
    public void Count_Test()
    {
        var myStructure = new Mystructure();
        int count = myStructure.Count;

        Assert.Equal(0, count);
    }

    [Fact]
    public void Add_Test()
    {
        var myStructure = new Mystructure();
        var item = "test";

        myStructure.Add(item);

        Assert.Equal(1, myStructure.Count);
        Assert.True(myStructure.Contains(item));
    }

    [Fact]
    public void RemoveAt_Test()
    {
        var myStructure = new Mystructure();
        myStructure.Add("item1");
        myStructure.Add("item2");
        myStructure.Add("item3");

        myStructure.RemoveAt(1);

        Assert.Equal(2, myStructure.Count);
        Assert.Equal("item1", myStructure[0]);
        Assert.Equal("item3", myStructure[1]);
    }

    [Fact]
    public void GetTypeOf_test()
    {
        var myStructure = new Mystructure();
        myStructure.Add(new CSVData());
        myStructure.Add("test");

        var type1 = myStructure.GetTypeOf(0);
        var type2 = myStructure.GetTypeOf(1);

        Assert.Equal(typeof(CSVData), type1);
        Assert.Equal(typeof(string), type2);
    }

    [Fact]
    public void GetItem_test()
    {
        var myStructure = new Mystructure();
        myStructure.Add("test");
        myStructure.Add(10);

        string stringItem = myStructure.GetItem<string>(0);
        int intItem = myStructure.GetItem<int>(1);

        Assert.Equal("test", stringItem);
        Assert.Equal(10, intItem);
    }

    [Fact]
    public void Clear_Test()
    {
        var myStructure = new Mystructure();
        myStructure.Add("test");
        myStructure.Add(10);

        myStructure.Clear();

        Assert.Equal(0, myStructure.Count);
    }

    [Fact]
    public void Contains_Test()
    {
        var myStructure = new Mystructure();
        myStructure.Add("test");
        myStructure.Add(10);

        Assert.True(myStructure.Contains("test"));
        Assert.True(myStructure.Contains(10));

        Assert.False(myStructure.Contains("example"));
        Assert.False(myStructure.Contains(20));
    }
}
