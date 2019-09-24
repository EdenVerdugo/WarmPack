using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WarmPack.Extensions;

namespace WarmPack.Windows.Controls
{
    public enum TextBoxExInputType
    {
        Text,
        OnlyCharacters,
        Integer,
        Decimal
    }

    public class TextBoxEx : System.Windows.Controls.TextBox
    {        
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
                        

            if (e.Key == Key.Enter || (CommandOnKeyEnterIncludesTab && e.Key == Key.Tab))
            {
                if (CommandOnKeyEnter != null)
                {
                    e.Handled = true;

                    var cmdParameter = new CommandParameterOnKeyDown();
                    cmdParameter.CommandParameter = CommandParameterOnKeyEnter;
                    cmdParameter.Key = e.Key;

                    if (CommandOnKeyEnter.CanExecute(cmdParameter))
                    {
                        CommandOnKeyEnter.Execute(cmdParameter);

                        if (cmdParameter.Handled)
                        {
                            return;
                        }

                    }
                }


                if (NextControlOnKeyTabEnter != null)
                {
                    NextControlOnKeyTabEnter.Focus();
                    e.Handled = true;
                }
            }
            else if (InputType == TextBoxExInputType.OnlyCharacters)
            {
                if (e.Key == Key.Space)
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            this.Text = this.Text.Replace(",", "").Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol, "");
            this.SelectionStart = this.Text.Length;
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            if (SelectOnFocus)
                this.Select(0, this.Text.Length);
        }

        protected override void OnGotMouseCapture(MouseEventArgs e)
        {
            base.OnGotMouseCapture(e);

            if (SelectOnFocus)
                this.Select(0, this.Text.Length);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (!this.IsKeyboardFocusWithin)
            {
                e.Handled = true;
                this.Focus();
            }

        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);


        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (string.IsNullOrEmpty(StringFormat))
                return;

            if (InputType == TextBoxExInputType.Integer)
            {
                this.Text = Convert.ToInt32(this.Text).ToString(StringFormat);
            }
            else if (InputType == TextBoxExInputType.Decimal)
            {
                this.Text = Convert.ToDecimal(this.Text).ToString(StringFormat);
            }
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            if (InputType == TextBoxExInputType.Integer)
            {
                e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
            }
            else if (InputType == TextBoxExInputType.Decimal)
            {
                var textValue = ((TextBoxEx)e.Source).Text;
                textValue = textValue + e.Text;

                e.Handled = !textValue.Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol, "").IsNumeric();
            }
            else if (InputType == TextBoxExInputType.OnlyCharacters)
            {
                e.Handled = Regex.IsMatch(e.Text, "[^A-Za-z]+");
            }
        }

        //private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        public TextBoxExInputType InputType
        {
            get { return (TextBoxExInputType)GetValue(InputTypeProperty); }
            set { SetValue(InputTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputTypeProperty =
            DependencyProperty.Register("InputType", typeof(TextBoxExInputType), typeof(TextBoxEx), new FrameworkPropertyMetadata(TextBoxExInputType.Text));




        public string StringFormat
        {
            get { return (string)GetValue(StringFormatProperty); }
            set { SetValue(StringFormatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StringFormat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register("StringFormat", typeof(string), typeof(TextBoxEx), new FrameworkPropertyMetadata(""));




        public bool SelectOnFocus
        {
            get { return (bool)GetValue(SelectOnFocusProperty); }
            set { SetValue(SelectOnFocusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectOnFocus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectOnFocusProperty =
            DependencyProperty.Register("SelectOnFocus", typeof(bool), typeof(TextBoxEx), new FrameworkPropertyMetadata(false));





        public System.Windows.Controls.Control NextControlOnKeyTabEnter
        {
            get { return (System.Windows.Controls.Control)GetValue(NextControlOnKeyTabEnterProperty); }
            set { SetValue(NextControlOnKeyTabEnterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NextControlOnKeyEnter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NextControlOnKeyTabEnterProperty =
            DependencyProperty.Register("NextControlOnKeyTabEnter", typeof(System.Windows.Controls.Control), typeof(TextBoxEx), new FrameworkPropertyMetadata(null));



        public ICommand CommandOnKeyEnter
        {
            get { return (ICommand)GetValue(CommandOnKeyEnterProperty); }
            set { SetValue(CommandOnKeyEnterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandOnKeyEnter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandOnKeyEnterProperty =
            DependencyProperty.Register("CommandOnKeyEnter", typeof(ICommand), typeof(TextBoxEx), new FrameworkPropertyMetadata(null));



        public object CommandParameterOnKeyEnter
        {
            get { return (object)GetValue(CommandParameterOnKeyEnterProperty); }
            set { SetValue(CommandParameterOnKeyEnterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameterOnKeyEnter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterOnKeyEnterProperty =
            DependencyProperty.Register("CommandParameterOnKeyEnter", typeof(object), typeof(TextBoxEx), new FrameworkPropertyMetadata(null));


        public bool CommandOnKeyEnterIncludesTab
        {
            get { return (bool)GetValue(CommandOnKeyEnterIncludesTabProperty); }
            set { SetValue(CommandOnKeyEnterIncludesTabProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandOnKeyEnterIncludesTab.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandOnKeyEnterIncludesTabProperty =
            DependencyProperty.Register("CommandOnKeyEnterIncludesTab", typeof(bool), typeof(TextBoxEx), new FrameworkPropertyMetadata(false));




    }
}
