using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace Generator.ViewModels;

public class ViewModelBase : ObservableObject
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string? propertyName)
    {
        PropertyChanged?.Invoke(this.PropertyChanged, new PropertyChangedEventArgs(propertyName));
    }
}
