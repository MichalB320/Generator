using System.Windows.Controls;

namespace Generator.Views
{
    /// <summary>
    /// Interaction logic for SourcesManagerView.xaml
    /// </summary>
    public partial class SourcesManagerView : UserControl
    {
        public SourcesManagerView()
        {
            InitializeComponent();
            Delete.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (ukazka.Text == "")
                Delete.Visibility = System.Windows.Visibility.Collapsed;
            else
                Delete.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
