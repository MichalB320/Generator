using System.Globalization;
using System.Reflection;
using System.Resources;

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
