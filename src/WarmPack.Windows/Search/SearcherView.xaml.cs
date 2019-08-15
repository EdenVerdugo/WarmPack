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

namespace WarmPack.Windows.Search
{
    /// <summary>
    /// Interaction logic for SearcherView.xaml
    /// </summary>
    public partial class SearcherView : Window
    {
        public SearcherView()
        {
            InitializeComponent();
            Keyboard.Focus(SearchText);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(SearchText);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void SearchItemsDataGridView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SearchItemsDataGridView.SelectedItem != null)
                this.Close();
        }
    }
}
