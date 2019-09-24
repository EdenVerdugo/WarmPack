using System.Windows;
using System.Windows.Input;
using WarmPack.Windows.ViewModels;

namespace WarmPack.Windows.Views
{
    /// <summary>
    /// Interaction logic for MessageBoxDatabaseConnectionStringView.xaml
    /// </summary>
    public partial class MessageBoxDatabaseConnectionStringView : Window
    {
        public MessageBoxDatabaseConnectionStringView()
        {
            InitializeComponent();
        }

        private void PasswordInputText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (this.DataContext != null)
                {
                    var vm = ((MessageDatabaseConnectionStringViewModel)this.DataContext);
                    vm.Password = PasswordInputText.Password;
                    vm.OkCommand.Execute(this);
                }                    
            }
            
        }

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void PasswordInputText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                var vm = ((MessageDatabaseConnectionStringViewModel)this.DataContext);
                vm.Password = PasswordInputText.Password;                
            }
        }
    }
}
