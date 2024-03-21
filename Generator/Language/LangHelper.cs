using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Generator.Language;

internal class LangHelper
{
    private static ResourceManager _rm;

    static LangHelper()
    {
        _rm = new ResourceManager("", Assembly.GetExecutingAssembly());
    }

    public static string? GetString(string name)
    {
        return _rm.GetString(name);
    }

    public static void ChangeLanguage(string language)
    {
        var cultureInfo = new CultureInfo(language);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
       
    }
}
