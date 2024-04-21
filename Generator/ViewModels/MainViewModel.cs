using CommunityToolkit.Mvvm.ComponentModel;
using GeneratorApp.Stores;

namespace GeneratorApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private NavigationStore _navigationStore;
    public ObservableObject CurrentViewModel => _navigationStore.CurrentViewModel;

    public MainViewModel(NavigationStore navigation)
    {
        //CurrentViewModel = new SourcesManagerViewModel(manager);
        _navigationStore = navigation;

        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }


}
