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
