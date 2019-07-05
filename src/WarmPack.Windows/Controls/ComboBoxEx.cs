using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WarmPack.Windows.Controls
{
    public class ComboBoxEx : ComboBox
    {
        public ComboBoxEx()
        {

        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                if (CommandOnKeyEnter != null)
                {
                    var cmdParameter = new CommandParameterOnKeyDown();
                    cmdParameter.CommandParameter = CommandParameterOnKeyEnter;
                    cmdParameter.Key = e.Key;

                    if (CommandOnKeyEnter.CanExecute(cmdParameter))
                    {
                        CommandOnKeyEnter.Execute(cmdParameter);

                        if (cmdParameter.Handled)
                            return;
                    }
                }


                if (NextControlOnKeyTabEnter != null)
                {
                    NextControlOnKeyTabEnter.Focus();
                    e.Handled = true;
                }
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (NextControlOnKeyTabEnter != null)
            {
                NextControlOnKeyTabEnter.Focus();
            }
        }

        public System.Windows.Controls.Control NextControlOnKeyTabEnter
        {
            get { return (System.Windows.Controls.Control)GetValue(NextControlOnKeyTabEnterProperty); }
            set { SetValue(NextControlOnKeyTabEnterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NextControlOnKeyEnter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NextControlOnKeyTabEnterProperty =
            DependencyProperty.Register("NextControlOnKeyTabEnter", typeof(System.Windows.Controls.Control), typeof(ComboBoxEx), new FrameworkPropertyMetadata(null));



        public ICommand CommandOnKeyEnter
        {
            get { return (ICommand)GetValue(CommandOnKeyEnterProperty); }
            set { SetValue(CommandOnKeyEnterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandOnKeyEnter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandOnKeyEnterProperty =
            DependencyProperty.Register("CommandOnKeyEnter", typeof(ICommand), typeof(ComboBoxEx), new FrameworkPropertyMetadata(null));



        public object CommandParameterOnKeyEnter
        {
            get { return (object)GetValue(CommandParameterOnKeyEnterProperty); }
            set { SetValue(CommandParameterOnKeyEnterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameterOnKeyEnter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterOnKeyEnterProperty =
            DependencyProperty.Register("CommandParameterOnKeyEnter", typeof(object), typeof(ComboBoxEx), new FrameworkPropertyMetadata(null));
    }
}
