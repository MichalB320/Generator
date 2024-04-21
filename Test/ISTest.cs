using GeneratorApp.Models;
using System.DirectoryServices;

namespace Test;

public class ISTest
{
    [Fact]
    public async Task WriteLDAP_ShouldReturnCorrectResult()
    {
        var sut = new IS();
        sut.Login("LDAP://pegasus.fri.uniza.sk", "bezo1", "milujemStatistiku");
        var ldapData = sut.GetLogin().Search("(cn=Michal*)");
        sut.AddSearchResulCollection(ldapData);

        var expected = "Michal Adamík\nMichal Babík\nMichal Babjak\nMichal Bajzík\nMichal Baláž\nMichal Balik\nMichal Belianský\nMichal Bežo\nMichal Bitarovský\nMichal Bízik\nMichal Bobot\nMichal Bohucky\nMichal Boltižiar\nMichal Čadecký\nMichal Čečko\nMichal Čekan\nMichal Červenec\nMichal Chabada\nMichal Chorvát\nMichal Chovanec\nMichal Crkoň\nMichal Ďuračík\nMichal Ďuratný\nMichal Durbák\nMichal Durdík\nMichal Durik\nMichal Ďurík\nMichal Džubek\nMichal Fašanok\nMichal Fiľo\nMichal Franc\nMichal Fraňo\nMichal Habalčík\nMichal Helbich\nMichal Hicz\nMichal Hlaváč\nMichal Hodoň\nMichal Hodoň UVP\nMichal Holčiak\nMichal Hraška\nMichal Hrčka\nMichal Jakabovič\nMichal Janírek\nMichal Janovec\nMichal Jastraban\nMichal Joštiak\nMichal Kais\nMichal Kaukič\nMichal Knapčok\nMichal Knotek\nMichal Kochláň\nMichal Kochláň\nMichal Kocifaj\nMichal Koháni\nMichal Kollár\nMichal Kováč\nMichal Kováčik\nMichal Krč\nMichal Krupa\nMichal Kubánek\nMichal Kubaščík\nMichal Kudla\nMichal Kurtulík\nMichal Kvak\nMichal Kvet\nMichal Kysela\nMichal Lajš\nMichal Lavko\nMichal Lekýr\nMichal Lichner\nMichal Litvin\nMichal Macko\nMichal Madera\nMichal Magner\nMichal Majerčík\nMichal Maruna\nMichal Masár\nMichal Matia\nMichal Matištík\nMichal Mičáň\nMichal Mikulík\nMichal Minka\nMichal Mlynár\nMichal Mokryš\nMichal Molitoris\nMichal Mrena\nMichal Mulík\nMichal Murín\nMichal Murín\nMichal Ondáš\nMichal Orčo\nMichal Páleš\nMichal Pavlák\nMichal Pavlov\nMichal Pažitný\nMichal Pelach\nMichal Petrán\nMichal Petrik\nMichal Pilarčík\nMichal Plichta\nMichal Poprac\nMichal Práznovský\nMichal Rajtek\nMichal Remenec\nMichal Remiš\nMichal Rovňan\nMichal Rybárik\nMichal Rykalsky\nMichal Salaj\nMichal Šarlák\nMichal Sihelský\nMichal Simonik\nMichal Sirota\nMichal Sládeček\nMichal Slávik\nMichal Smatlak\nMichal Smolka\nMichal Smutný\nMichal Sojčák\nMichal Šotkovský\nMichal Šovčík\nMichal Srnec\nMichal Štaffen\nMichal Stanek\nMichal Šterbák\nMichal Stupka\nMichal Šubert\nMichal Tkačín\nMichal Trnka\nMichal Tulák\nMichal Tvrdý\nMichal Uhliarik\nMichal Urbančok\nMichal Urbánek\nMichal Urbanovič\nMichal Varga\nMichal Varga_auto\nMichal Varmus\nMichal Vartiak\nMichal Vlach\nMichal Vrábel\nMichal Vrsansky\nMichal Zábovský\nMichal Zabovsky_auto\nMichal Žarnay\nMichal Zifčák\nMichal Zigo\nMichal Zúbek\n";

        var result = await sut.WriteLDAP(0);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ClearDataStack_test()
    {
        var sut = new IS();
        sut.AddCSV(new CSVData());

        sut.ClearDataStack();

        Assert.Equal(0, sut.GetManager().Structure.Count);
    }

    [Fact]
    public void AddCSV_Test()
    {
        var sut = new IS();
        var csvData = new CSVData();
        sut.AddCSV(csvData);
        Assert.Equal(csvData, sut.GetStructure().GetItem<CSVData>(0));
    }

    [Fact]
    public void AddSearchResultCollection_ShouldAddSearchResultCollection()
    {
        var sut = new IS();
        sut.Login("LDAP://pegasus.fri.uniza.sk", "bezo1", "milujemStatistiku");
        var ldapData = sut.GetLogin().Search("(cn=Michal*)");
        sut.AddSearchResulCollection(ldapData);

        var actual = sut.GetStructure().GetItem<SearchResultCollection>(0);
        //Assert.Equal(ldapData, actual);
        if (ldapData == actual)
        {
            Assert.True(true);
        }
        else
        {
            Assert.True(false);
        }
    }
}
