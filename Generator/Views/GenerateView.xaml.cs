using System.Windows.Controls;

namespace Generator.Views
{
    /// <summary>
    /// Interaction logic for GenerateView.xaml
    /// </summary>
    public partial class GenerateView : UserControl
    {
        public GenerateView()
        {
            InitializeComponent();
            Save.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (output.Text == "")
                Save.Visibility = System.Windows.Visibility.Collapsed;
            else
                Save.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
