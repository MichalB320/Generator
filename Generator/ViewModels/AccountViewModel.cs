using CommunityToolkit.Mvvm.ComponentModel;

namespace Generator.ViewModels;

internal class AccountViewModel : ObservableObject
{
    public NavigationBarViewModel NavigationBarViewModel { get; }



    public AccountViewModel(NavigationBarViewModel navigationBarViewModel)
    {
        NavigationBarViewModel = navigationBarViewModel;
        navigationBarViewModel.Visible();
    }
}
