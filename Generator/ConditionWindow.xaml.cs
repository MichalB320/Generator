using System.Windows;

namespace Generator
{
    /// <summary>
    /// Interaction logic for ConditionWindow.xaml
    /// </summary>
    public partial class ConditionWindow : Window
    {
        public ConditionWindow()
        {
            InitializeComponent();
        }

        public void CloseWindowHandler()
        {
            Close();
        }
    }
}
