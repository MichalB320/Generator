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
            EditBtn.Visibility = System.Windows.Visibility.Collapsed;
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

        //private void OnNextBtnEnter(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    NextPage.Height = 60;
        //    NextPage.Width = 60;
        //    NextPage.Margin = new System.Windows.Thickness(0);
        //}

        //private void OnNextBtnLeave(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    NextPage.Height = 50;
        //    NextPage.Width = 50;
        //    NextPage.Margin = new System.Windows.Thickness(5);
        //}

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

        private void OnDeleteBtnEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Delete.Height = 60;
            Delete.Width = 60;
            Delete.Margin = new System.Windows.Thickness(0);
        }

        private void OnDeleteBtnLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Delete.Height = 50;
            Delete.Width = 50;
            Delete.Margin = new System.Windows.Thickness(5);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (ukazka.Text == "")
            {
                Delete.Visibility = System.Windows.Visibility.Collapsed;
                EditBtn.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                Delete.Visibility = System.Windows.Visibility.Visible;
                EditBtn.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void OnEVBtnEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            evBtn.Height = 60;
            evBtn.Width = 60;
        }

        private void OnEVBtnLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            evBtn.Height = 50;
            evBtn.Width = 50;
        }

        private void OnEditBtnEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            EditBtn.Height = 60;
            EditBtn.Width = 60;
            EditBtn.Margin = new System.Windows.Thickness(0);
        }

        private void OnEditBtnLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            EditBtn.Height = 50;
            EditBtn.Width = 50;
            EditBtn.Margin = new System.Windows.Thickness(5);
        }
    }
}
