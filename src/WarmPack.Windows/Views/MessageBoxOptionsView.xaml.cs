using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
