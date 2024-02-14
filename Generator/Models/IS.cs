using ClassLibrary;

namespace Generator.Models;

public class IS
{
    private Login _login;
    private Manager _manager;
    private Generator _generator;
    private Mystructure _structure;

    public IS(Login login, Manager manager, Generator generator, Mystructure structure)
    {
        _login = login;
        _manager = manager;
        _generator = generator;
        _structure = structure;
    }

    public IS(Mystructure structure)
    {
        _login = new Login("LDAP://pegasus.fri.uniza.sk", "bezo1", "Logged out");
        _manager = new Manager(ref structure);
        _generator = new Generator("", structure, new System.Collections.ObjectModel.ObservableCollection<ViewModels.ButtonViewModel>());

        _structure = structure;
    }

    public Login GetLogin() => _login;

    public Manager GetManager() => _manager;

    public Generator GetGenerator() => _generator;

    public ref Mystructure GetStructure() => ref _structure;
}
