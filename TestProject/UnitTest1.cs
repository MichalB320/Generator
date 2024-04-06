

using Generator.Models;
using System.Runtime.CompilerServices;

namespace TestProject;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
         Login lgi = new Login();

        Generator.GeneratorApp.Models.Login lg = new();
    }
}