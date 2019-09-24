using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WarmPack.Windows.Controls
{
    public static class ControlEx
    {
        public static Control GetNextControlOnKeyTabEnter(DependencyObject obj)
        {
            return (Control)obj.GetValue(NextControlOnKeyTabEnterProperty);
        }

        public static void SetNextControlOnKeyTabEnter(DependencyObject obj, Control value)
        {
            obj.SetValue(NextControlOnKeyTabEnterProperty, value);
        }

        public static readonly DependencyProperty NextControlOnKeyTabEnterProperty =
            DependencyProperty.RegisterAttached("NextControlOnKeyTabEnter", typeof(Control), typeof(ControlEx), new FrameworkPropertyMetadata(CommandOnKeyTabEnterPropertyChanged));

        private static void NextControlOnKeyTabEnterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)d;

            if (e.OldValue == null)
            {
                fe.PreviewKeyDown += OnCommandKeyTabEnterPreviewKeyDow;
            }
        }

        //private static void OnPreviewKeyDow(object sender, KeyEventArgs e)
        //{
        //    if(e.Key == Key.Enter || e.Key == Key.Tab)
        //    {
        //        var p = (Control)((Control)e.Source).GetValue(NextControlOnKeyTabEnterProperty);
        //        p?.Focus();

        //        //e.Handled = true;
        //    }
        //}



        public static ICommand GetCommandOnKeyTabEnter(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandOnKeyTabEnterProperty);
        }

        public static void SetCommandOnKeyTabEnter(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandOnKeyTabEnterProperty, value);
        }

        // Using a DependencyProperty as the backing store for CommandOnKeyTabEnter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandOnKeyTabEnterProperty =
            DependencyProperty.RegisterAttached("CommandOnKeyTabEnter", typeof(ICommand), typeof(ControlEx), new FrameworkPropertyMetadata(CommandOnKeyTabEnterPropertyChanged));

        private static void CommandOnKeyTabEnterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)d;

            if (e.OldValue == null)
            {
                fe.PreviewKeyDown += OnCommandKeyTabEnterPreviewKeyDow;
            }
        }

        private static void OnCommandKeyTabEnterPreviewKeyDow(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                var p = (FrameworkElement)((FrameworkElement)e.Source).GetValue(NextControlOnKeyTabEnterProperty);
                p?.Focus();


                var cmd = (ICommand)((FrameworkElement)sender)?.GetValue(CommandOnKeyTabEnterProperty);

                var param = new CommandParameterOnKeyDown()
                {
                    CommandParameter = ((FrameworkElement)sender).GetValue(CommandParameterOnKeyTabEnterProperty),
                    Key = e.Key
                };

                cmd?.Execute(param);
                e.Handled = true;
            }
        }

        public static Object GetCommandParameterOnKeyTabEnter(DependencyObject obj)
        {
            return (Object)obj.GetValue(CommandParameterOnKeyTabEnterProperty);
        }

        public static void SetCommandParameterOnKeyTabEnter(DependencyObject obj, Object value)
        {
            obj.SetValue(CommandParameterOnKeyTabEnterProperty, value);
        }

        // Using a DependencyProperty as the backing store for CommandParameterOnKeyTabEnter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterOnKeyTabEnterProperty =
            DependencyProperty.RegisterAttached("CommandParameterOnKeyTabEnter", typeof(Object), typeof(ControlEx), new FrameworkPropertyMetadata(null));


    }
}
