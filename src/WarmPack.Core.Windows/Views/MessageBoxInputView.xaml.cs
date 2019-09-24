using System.Windows;
using System.Windows.Input;

namespace WarmPack.Windows.Views
{
    /// <summary>
    /// Interaction logic for MessageBoxInputView.xaml
    /// </summary>
    public partial class MessageBoxInputView : Window
    {
        public MessageBoxInputView()
        {
            InitializeComponent();
        }

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
