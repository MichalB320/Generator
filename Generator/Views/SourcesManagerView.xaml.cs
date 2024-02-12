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
        }

        private void OnLdapBtnEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ldapBtn.Height = 60;
            ldapBtn.Width = 60;
        }

        private void OnLdapBtnLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ldapBtn.Height = 50;
            ldapBtn.Width = 50;
        }

        private void OnCsvBtnEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            csvBtn.Height = 60;
            csvBtn.Width = 60;
        }

        private void OnCsvBtnLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            csvBtn.Height = 50;
            csvBtn.Width = 50;
        }

        private void OnNextBtnEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            NextPage.Height = 60;
            NextPage.Width = 60;
            NextPage.Margin = new System.Windows.Thickness(0);
        }

        private void OnNextBtnLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            NextPage.Height = 50;
            NextPage.Width = 50;
            NextPage.Margin = new System.Windows.Thickness(5);
        }

        private void OnPreviousBtnEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            PreviousPage.Height = 60;
            PreviousPage.Width = 60;
            PreviousPage.Margin = new System.Windows.Thickness(0);
        }

        private void OnPreviousBtnLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            PreviousPage.Height = 50;
            PreviousPage.Width = 50;
            PreviousPage.Margin = new System.Windows.Thickness(5);
        }
    }
}
