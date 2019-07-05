using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WarmPack.Windows.Controls
{
    public static class WindowEx
    {
        private static void OnPreviewKeyDow(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                var cmd = (ICommand)((FrameworkElement)sender)?.GetValue(CommandOnKeyEscProperty);

                var param = new CommandParameterOnKeyDown()
                {
                    CommandParameter = ((FrameworkElement)sender).GetValue(CommandParameterOnKeyEscProperty),
                    Key = e.Key
                };

                cmd?.Execute(param);
                e.Handled = true;
            }
            else if (e.Key == Key.F1)
            {
                var cmd = (ICommand)((FrameworkElement)sender)?.GetValue(CommandOnKeyF1Property);

                var param = new CommandParameterOnKeyDown()
                {
                    CommandParameter = ((FrameworkElement)sender).GetValue(CommandParameterOnKeyF1Property),
                    Key = e.Key
                };

                cmd?.Execute(param);
                e.Handled = true;
            }
            else if (e.Key == Key.F2)
            {
                var cmd = (ICommand)((FrameworkElement)sender)?.GetValue(CommandOnKeyF2Property);

                var param = new CommandParameterOnKeyDown()
                {
                    CommandParameter = ((FrameworkElement)sender).GetValue(CommandParameterOnKeyF2Property),
                    Key = e.Key
                };

                cmd?.Execute(param);
                e.Handled = true;
            }
            else if (e.Key == Key.F3)
            {
                var cmd = (ICommand)((FrameworkElement)sender)?.GetValue(CommandOnKeyF3Property);

                var param = new CommandParameterOnKeyDown()
                {
                    CommandParameter = ((FrameworkElement)sender).GetValue(CommandParameterOnKeyF3Property),
                    Key = e.Key
                };

                cmd?.Execute(param);
                e.Handled = true;
            }
            else if (e.Key == Key.F4)
            {
                var cmd = (ICommand)((FrameworkElement)sender)?.GetValue(CommandOnKeyF4Property);

                var param = new CommandParameterOnKeyDown()
                {
                    CommandParameter = ((FrameworkElement)sender).GetValue(CommandParameterOnKeyF4Property),
                    Key = e.Key
                };

                cmd?.Execute(param);
                e.Handled = true;
            }
            else if (e.Key == Key.F5)
            {
                var cmd = (ICommand)((FrameworkElement)sender)?.GetValue(CommandOnKeyF5Property);

                var param = new CommandParameterOnKeyDown()
                {
                    CommandParameter = ((FrameworkElement)sender).GetValue(CommandParameterOnKeyF5Property),
                    Key = e.Key
                };

                cmd?.Execute(param);
                e.Handled = true;
            }
            else if (e.Key == Key.F6)
            {
                var cmd = (ICommand)((FrameworkElement)sender)?.GetValue(CommandOnKeyF6Property);

                var param = new CommandParameterOnKeyDown()
                {
                    CommandParameter = ((FrameworkElement)sender).GetValue(CommandParameterOnKeyF6Property),
                    Key = e.Key
                };

                cmd?.Execute(param);
                e.Handled = true;
            }
            else if (e.Key == Key.F7)
            {
                var cmd = (ICommand)((FrameworkElement)sender)?.GetValue(CommandOnKeyF7Property);

                var param = new CommandParameterOnKeyDown()
                {
                    CommandParameter = ((FrameworkElement)sender).GetValue(CommandParameterOnKeyF7Property),
                    Key = e.Key
                };

                cmd?.Execute(param);
                e.Handled = true;
            }
            else if (e.Key == Key.F8)
            {
                var cmd = (ICommand)((FrameworkElement)sender)?.GetValue(CommandOnKeyF8Property);

                var param = new CommandParameterOnKeyDown()
                {
                    CommandParameter = ((FrameworkElement)sender).GetValue(CommandParameterOnKeyF8Property),
                    Key = e.Key
                };

                cmd?.Execute(param);
                e.Handled = true;
            }
            else if (e.Key == Key.F9)
            {
                var cmd = (ICommand)((FrameworkElement)sender)?.GetValue(CommandOnKeyF9Property);

                var param = new CommandParameterOnKeyDown()
                {
                    CommandParameter = ((FrameworkElement)sender).GetValue(CommandParameterOnKeyF9Property),
                    Key = e.Key
                };

                cmd?.Execute(param);
                e.Handled = true;
            }
        }

        private static void OnChangeCommandOnKeyDown(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)d;

            if (e.OldValue == null)
            {
                fe.PreviewKeyDown += OnPreviewKeyDow;
            }

        }

        public static Object GetCommandParameterOnKeyEsc(DependencyObject obj)
        {
            return (Object)obj.GetValue(CommandParameterOnKeyEscProperty);
        }

        public static void SetCommandParameterOnKeyEsc(DependencyObject obj, Object value)
        {
            obj.SetValue(CommandParameterOnKeyEscProperty, value);
        }

        public static readonly DependencyProperty CommandParameterOnKeyEscProperty =
            DependencyProperty.RegisterAttached("CommandParameterOnKeyEsc", typeof(Object), typeof(WindowEx), new PropertyMetadata(null));




        public static Object GetCommandParameterOnKeyF1(DependencyObject obj)
        {
            return (Object)obj.GetValue(CommandParameterOnKeyF1Property);
        }

        public static void SetCommandParameterOnKeyF1(DependencyObject obj, Object value)
        {
            obj.SetValue(CommandParameterOnKeyF1Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandParameterOnKeyF1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterOnKeyF1Property =
            DependencyProperty.RegisterAttached("CommandParameterOnKeyF1", typeof(Object), typeof(WindowEx), new PropertyMetadata(null));



        public static Object GetCommandParameterOnKeyF2(DependencyObject obj)
        {
            return (Object)obj.GetValue(CommandParameterOnKeyF2Property);
        }

        public static void SetCommandParameterOnKeyF2(DependencyObject obj, Object value)
        {
            obj.SetValue(CommandParameterOnKeyF2Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandParameterOnKeyF2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterOnKeyF2Property =
            DependencyProperty.RegisterAttached("CommandParameterOnKeyF2", typeof(Object), typeof(WindowEx), new PropertyMetadata(null));




        public static object GetCommandParameterOnKeyF3(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterOnKeyF3Property);
        }

        public static void SetCommandParameterOnKeyF3(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterOnKeyF3Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandParameterOnKeyF3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterOnKeyF3Property =
            DependencyProperty.RegisterAttached("CommandParameterOnKeyF3", typeof(object), typeof(WindowEx), new PropertyMetadata(null));




        public static object GetCommandParameterOnKeyF4(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterOnKeyF4Property);
        }

        public static void SetCommandParameterOnKeyF4(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterOnKeyF4Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandParameterOnKeyF4.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterOnKeyF4Property =
            DependencyProperty.RegisterAttached("CommandParameterOnKeyF4", typeof(object), typeof(WindowEx), new PropertyMetadata(null));




        public static object GetCommandParameterOnKeyF5(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterOnKeyF5Property);
        }

        public static void SetCommandParameterOnKeyF5(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterOnKeyF5Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandParameterOnKeyF5.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterOnKeyF5Property =
            DependencyProperty.RegisterAttached("CommandParameterOnKeyF5", typeof(object), typeof(WindowEx), new PropertyMetadata(null));




        public static object GetCommandParameterOnKeyF6(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterOnKeyF6Property);
        }

        public static void SetCommandParameterOnKeyF6(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterOnKeyF6Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandParameterOnKeyF6.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterOnKeyF6Property =
            DependencyProperty.RegisterAttached("CommandParameterOnKeyF6", typeof(object), typeof(WindowEx), new PropertyMetadata(null));




        public static object GetCommandParameterOnKeyF7(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterOnKeyF7Property);
        }

        public static void SetCommandParameterOnKeyF7(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterOnKeyF7Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandParameterOnKeyF7.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterOnKeyF7Property =
            DependencyProperty.RegisterAttached("CommandParameterOnKeyF7", typeof(object), typeof(WindowEx), new PropertyMetadata(null));




        public static object GetCommandParameterOnKeyF8(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterOnKeyF8Property);
        }

        public static void SetCommandParameterOnKeyF8(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterOnKeyF8Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandParameterOnKeyF8.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterOnKeyF8Property =
            DependencyProperty.RegisterAttached("CommandParameterOnKeyF8", typeof(object), typeof(WindowEx), new PropertyMetadata(null));




        public static object GetCommandParameterOnKeyF9(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterOnKeyF9Property);
        }

        public static void SetCommandParameterOnKeyF9(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterOnKeyF9Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandParameterOnKeyF9.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterOnKeyF9Property =
            DependencyProperty.RegisterAttached("CommandParameterOnKeyF9", typeof(object), typeof(WindowEx), new PropertyMetadata(null));




        public static ICommand GetCommandOnKeyEsc(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (ICommand)element.GetValue(CommandOnKeyEscProperty);
        }

        public static void SetCommandOnKeyEsc(DependencyObject element, ICommand value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(CommandOnKeyEscProperty, value);
        }

        public static readonly DependencyProperty CommandOnKeyEscProperty = DependencyProperty.RegisterAttached("CommandOnKeyEsc", typeof(ICommand), typeof(WindowEx), new FrameworkPropertyMetadata(OnChangeCommandOnKeyDown));


        public static ICommand GetCommandOnKeyF1(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandOnKeyF1Property);
        }

        public static void SetCommandOnKeyF1(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandOnKeyF1Property, value);
        }


        public static readonly DependencyProperty CommandOnKeyF1Property = DependencyProperty.RegisterAttached("CommandOnKeyF1", typeof(ICommand), typeof(WindowEx), new FrameworkPropertyMetadata(OnChangeCommandOnKeyDown));

        public static ICommand GetCommandOnKeyF2(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandOnKeyF2Property);
        }

        public static void SetCommandOnKeyF2(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandOnKeyF2Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandOnKeyF2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandOnKeyF2Property =
            DependencyProperty.RegisterAttached("CommandOnKeyF2", typeof(ICommand), typeof(WindowEx), new FrameworkPropertyMetadata(OnChangeCommandOnKeyDown));



        public static ICommand GetCommandOnKeyF3(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandOnKeyF3Property);
        }

        public static void SetCommandOnKeyF3(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandOnKeyF3Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandOnKeyF3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandOnKeyF3Property =
            DependencyProperty.RegisterAttached("CommandOnKeyF3", typeof(ICommand), typeof(WindowEx), new FrameworkPropertyMetadata(OnChangeCommandOnKeyDown));



        public static ICommand GetCommandOnKeyF4(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandOnKeyF4Property);
        }

        public static void SetCommandOnKeyF4(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandOnKeyF4Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandOnKeyF4.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandOnKeyF4Property =
            DependencyProperty.RegisterAttached("CommandOnKeyF4", typeof(ICommand), typeof(WindowEx), new FrameworkPropertyMetadata(OnChangeCommandOnKeyDown));



        public static ICommand GetCommandOnKeyF5(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandOnKeyF5Property);
        }

        public static void SetCommandOnKeyF5(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandOnKeyF5Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandOnKeyF5.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandOnKeyF5Property =
            DependencyProperty.RegisterAttached("CommandOnKeyF5", typeof(ICommand), typeof(WindowEx), new FrameworkPropertyMetadata(OnChangeCommandOnKeyDown));




        public static ICommand GetCommandOnKeyF6(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandOnKeyF6Property);
        }

        public static void SetCommandOnKeyF6(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandOnKeyF6Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandOnKeyF6.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandOnKeyF6Property =
            DependencyProperty.RegisterAttached("CommandOnKeyF6", typeof(ICommand), typeof(WindowEx), new FrameworkPropertyMetadata(OnChangeCommandOnKeyDown));




        public static ICommand GetCommandOnKeyF7(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandOnKeyF7Property);
        }

        public static void SetCommandOnKeyF7(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandOnKeyF7Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandOnKeyF7.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandOnKeyF7Property =
            DependencyProperty.RegisterAttached("CommandOnKeyF7", typeof(ICommand), typeof(WindowEx), new FrameworkPropertyMetadata(OnChangeCommandOnKeyDown));




        public static ICommand GetCommandOnKeyF8(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandOnKeyF8Property);
        }

        public static void SetCommandOnKeyF8(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandOnKeyF8Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandOnKeyF8.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandOnKeyF8Property =
            DependencyProperty.RegisterAttached("CommandOnKeyF8", typeof(ICommand), typeof(WindowEx), new FrameworkPropertyMetadata(OnChangeCommandOnKeyDown));




        public static ICommand GetCommandOnKeyF9(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandOnKeyF9Property);
        }

        public static void SetCommandOnKeyF9(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandOnKeyF9Property, value);
        }

        // Using a DependencyProperty as the backing store for CommandOnKeyF9.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandOnKeyF9Property =
            DependencyProperty.RegisterAttached("CommandOnKeyF9", typeof(ICommand), typeof(WindowEx), new FrameworkPropertyMetadata(OnChangeCommandOnKeyDown));


        public static ICommand GetEventOnLoadCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(EventOnLoadCommandProperty);
        }

        public static void SetEventOnLoadCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(EventOnLoadCommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for EventOnLoadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EventOnLoadCommandProperty =
            DependencyProperty.RegisterAttached("EventOnLoadCommand", typeof(ICommand), typeof(WindowEx), new FrameworkPropertyMetadata(CommandOnLoad));



        private static void CommandOnLoad(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)d;

            if (e.OldValue == null)
            {
                var cmd = (ICommand)(fe)?.GetValue(EventOnLoadCommandProperty);

                cmd?.Execute(null);
            }
        }
    }
}
