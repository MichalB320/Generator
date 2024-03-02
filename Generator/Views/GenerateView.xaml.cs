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
        }

        //private void OnPreviousBtnEnter(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    PreviousPage.Height = 60;
        //    PreviousPage.Width = 60;
        //    PreviousPage.Margin = new System.Windows.Thickness(0);
        //}

        //private void OnPreviousBtnLeave(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    PreviousPage.Height = 50;
        //    PreviousPage.Width = 50;
        //    PreviousPage.Margin = new System.Windows.Thickness(5);
        //}

        private void OnSaveBtnEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Save.Height = 60;
            Save.Width = 60;
            Save.Margin = new System.Windows.Thickness(0);
        }

        private void OnSaveBtnLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Save.Height = 50;
            Save.Width = 50;
            Save.Margin = new System.Windows.Thickness(5);
        }
    }
}
