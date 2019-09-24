using System.Windows;
using System.Windows.Input;

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
