using System.Windows;
using System.Windows.Input;

namespace WarmPack.Windows.Views
{
    /// <summary>
    /// Interaction logic for MessageBoxOptionsView.xaml
    /// </summary>
    public partial class MessageBoxOptionsView : Window
    {
        public MessageBoxOptionsView()
        {
            InitializeComponent();
        }

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
