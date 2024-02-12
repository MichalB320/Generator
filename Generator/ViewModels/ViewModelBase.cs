using System.ComponentModel;

namespace Generator.ViewModels;

public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string? propertyName)
    {
        PropertyChanged?.Invoke(this.PropertyChanged, new PropertyChangedEventArgs(propertyName));
    }
}
