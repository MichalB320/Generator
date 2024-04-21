using CommunityToolkit.Mvvm.ComponentModel;

namespace GeneratorApp.Stores;

public class NavigationStore
{
    private ObservableObject _currentViewModel;
    public ObservableObject CurrentViewModel
    {
        get { return _currentViewModel; }
        set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }

    public event Action CurrentViewModelChanged;
}
