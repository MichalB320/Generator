namespace GeneratorApp.ViewModels;

public class MenuViewModel : ViewModelBase
{
    public LoginViewModel LoginVM { get; }
    public SourcesManagerViewModel SrcManagerVM { get; }
    public GenerateViewModel GenerateVM { get; }

    public MenuViewModel(LoginViewModel loginVM, GenerateViewModel generateVM)
    {
        LoginVM = loginVM;
        GenerateVM = generateVM;
    }
}
