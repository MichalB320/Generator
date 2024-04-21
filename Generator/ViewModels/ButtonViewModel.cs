using System.Windows.Input;

namespace GeneratorApp.ViewModels;

public class ButtonViewModel
{
    public string Content { get; set; }
    public ICommand Command { get; set; }
    public int Index { get; set; }
    public Type Type { get; set; }

    public ButtonViewModel(string content, ICommand command, int index, Type type)
    {
        Content = content;
        Command = command;
        Index = index;
        Type = type;
    }
}
