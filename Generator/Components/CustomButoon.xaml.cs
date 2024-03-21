using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Generator.Components
{
    /// <summary>
    /// Interaction logic for CustomButoon.xaml
    /// </summary>
    public partial class CustomButoon : UserControl
    {
        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("ColorProperty", typeof(Brush), typeof(CustomButoon), new PropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CustomButoon), new PropertyMetadata(null));

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(string), typeof(CustomButoon), new PropertyMetadata(string.Empty));

        public CustomButoon()
        {
            InitializeComponent();
        }

        private void OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Btn.Height = 60;
            Btn.Width = 60;
            Btn.Margin = new System.Windows.Thickness(0);
        }

        private void OnMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Btn.Height = 50;
            Btn.Width = 50;
            Btn.Margin = new System.Windows.Thickness(5);
        }
    }
}
