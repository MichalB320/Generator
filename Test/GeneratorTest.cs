using GeneratorApp.Models;
using GeneratorApp.ViewModels;

namespace Test;

public class GeneratorTest
{

    [Fact]
    public async Task FindStrings_Test()
    {
        Mystructure structure = new Mystructure();
        var buttons = new System.Collections.ObjectModel.ObservableCollection<ButtonViewModel>();
        var generator = new GeneratorApp.Models.Generator("test$sub$string", structure, buttons);

        await generator.FindStrings('$');

        Assert.Contains("sub", generator.Get_strings());
        Assert.Single(generator.Get_strings());
    }

    [Fact]
    public async Task FindSourcesAndVariables_FindsDelimiterInStrings_AddsSourcesAndVariablesToLists()
    {
        Mystructure structure = new Mystructure();
        var buttons = new System.Collections.ObjectModel.ObservableCollection<ButtonViewModel>();
        var generator = new GeneratorApp.Models.Generator("useradd -u $zdroj.meno$ -m $zdroj.priezvisko$ + $zdroj2.meno$ a $zdroj2.priezvisko$", structure, buttons);
        await generator.FindStrings('$');

        await generator.FindSourcesAndVariables('.');

        Assert.Contains("zdroj", generator.Get_sources());
        Assert.Contains("zdroj2", generator.Get_sources());
        Assert.Contains("meno", generator.Get_variables());
        Assert.Contains("priezvisko", generator.Get_variables());
        Assert.Equal(4, generator.Get_sources().Count);
        Assert.Equal(4, generator.Get_variables().Count);
    }


}
