using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Windows.Controls;
using WarmPack.Windows.ViewModels;

namespace WarmPack.Windows
{
    public static class Message
    {
        public static MessageResult Show(string messageBoxText)
        {
            return Show(null, messageBoxText, string.Empty, MessageButton.OK, MessageStyle.Primary, MessageOptions.ButtonsAsButtons);
        }

        public static MessageResult Show(string messageBoxText, string caption)
        {
            return Show(null, messageBoxText, caption, MessageButton.OK, MessageStyle.Primary, MessageOptions.ButtonsAsButtons);
        }

        public static MessageResult Show(System.Windows.Window owner, string messageBoxText)
        {
            return Show(owner, messageBoxText, string.Empty, MessageButton.OK, MessageStyle.Primary, MessageOptions.ButtonsAsButtons);
        }

        public static MessageResult Show(string messageBoxText, string caption, MessageButton button)
        {
            return Show(null, messageBoxText, caption, button, MessageStyle.Primary, MessageOptions.ButtonsAsButtons);
        }

        public static MessageResult Show(System.Windows.Window owner, string messageBoxText, string caption)
        {
            return Show(owner, messageBoxText, caption, MessageButton.OK, MessageStyle.Primary, MessageOptions.ButtonsAsButtons);
        }

        public static MessageResult Show(string messageBoxText, string caption, MessageButton button, MessageStyle style)
        {
            return Show(null, messageBoxText, caption, button, style, MessageOptions.ButtonsAsButtons);
        }

        public static MessageResult Show(string messageBoxText, string caption, MessageButton button, MessageStyle style, MessageOptions options)
        {
            return Show(null, messageBoxText, caption, button, style, options);
        }

        public static MessageResult Show(System.Windows.Window owner, string messageBoxText, string caption, MessageButton button, MessageStyle style)
        {
            return Show(null, messageBoxText, caption, button, style, MessageOptions.ButtonsAsButtons);
        }

        public static MessageResult Show(System.Windows.Window owner, string messageBoxText, string caption, MessageButton button, MessageStyle style, MessageOptions options)
        {
            var vm = new MessageViewModel();
            var view = new Views.MessageBoxView();
            view.DataContext = vm;

            vm.Caption = caption;
            vm.MessageBoxText = messageBoxText;
            vm.MessageStyle = style;
            vm.MessageButton = button;
            vm.MessageOptions = options;
            vm.ApplyMessageStyle();

            view.ShowDialog();

            return vm.MessageResult;
        }

        public static string ShowInput(System.Windows.Window owner, string messageBoxText, string caption, MessageStyle style, TextBoxExInputType inputType)
        {
            var vm = new MessageInputViewModel();
            var view = new Views.MessageBoxInputView();
            view.DataContext = vm;

            vm.Caption = caption;
            vm.MessageBoxText = messageBoxText;
            vm.MessageStyle = style;
            vm.MessageButton = MessageButton.OKCancel;
            vm.MessageInputType = inputType;
            vm.ApplyMessageStyle();

            view.ShowDialog();

            return vm.MessageResult == MessageResult.OK ? vm.MessageInputText : "";
        }

        public static string ShowInput(string messageBoxText)
        {
            return ShowInput(null, messageBoxText, string.Empty, MessageStyle.Primary, TextBoxExInputType.Text);
        }

        public static string ShowInput(string messageBoxText, TextBoxExInputType inputType)
        {
            return ShowInput(null, messageBoxText, string.Empty, MessageStyle.Primary, inputType);
        }

        public static string ShowInput(string messageBoxText, MessageStyle style)
        {
            return ShowInput(null, messageBoxText, string.Empty, style, TextBoxExInputType.Text);
        }

        public static string ShowInput(string messageBoxText, MessageStyle style, TextBoxExInputType inputType)
        {
            return ShowInput(null, messageBoxText, string.Empty, style, inputType);
        }

        public static string ShowInput(string messageBoxText, string caption, MessageStyle style)
        {
            return ShowInput(null, messageBoxText, caption, style, TextBoxExInputType.Text);
        }

        public static string ShowInput(string messageBoxText, string caption, MessageStyle style, TextBoxExInputType inputType)
        {
            return ShowInput(null, messageBoxText, caption, style, inputType);
        }

        public static T ShowWithOptions<T>(System.Windows.Window owner, string messageBoxText, string caption, MessageStyle style, System.Collections.IList dataSource, MessageWithOptionControlStyle controlsAs, MessageWithOptionSelectionMode selectionMode) where T : IMessageWithOption
        {
            var vm = new MessageWithOptionsViewModel();
            var view = new Views.MessageBoxOptionsView();
            view.DataContext = vm;

            vm.Caption = caption;
            vm.MessageBoxText = messageBoxText;
            vm.MessageStyle = style;
            vm.MessageButton = MessageButton.OKCancel;

            if (selectionMode == MessageWithOptionSelectionMode.Single)
                vm.MessageOptionsDataSource = dataSource;
            else
                vm.MessageOptionsDataSourceMultiple = dataSource;

            vm.MessageWithOptionControlStyle = controlsAs;
            vm.MessageWithOptionSelectionMode = selectionMode;
            vm.ApplyMessageStyle();

            view.ShowDialog();

            return vm.MessageResult == MessageResult.OK ? (T)vm.MessageOptionSelected : default(T);
        }

        public static T ShowWithOptions<T>(string messageBoxText, string caption, MessageStyle style, System.Collections.IList dataSource, MessageWithOptionControlStyle controlsAs) where T : IMessageWithOption
        {
            return ShowWithOptions<T>(null, messageBoxText, caption, style, dataSource, controlsAs, MessageWithOptionSelectionMode.Single);
        }

        public static T ShowWithOptions<T>(string messageBoxText, string caption, MessageStyle style, System.Collections.IList dataSource) where T : IMessageWithOption
        {
            return ShowWithOptions<T>(null, messageBoxText, caption, style, dataSource, MessageWithOptionControlStyle.ComboBox, MessageWithOptionSelectionMode.Single);
        }

        public static T ShowWithOptions<T>(string messageBoxText, MessageStyle style, System.Collections.IList dataSource) where T : IMessageWithOption
        {
            return ShowWithOptions<T>(null, messageBoxText, string.Empty, style, dataSource, MessageWithOptionControlStyle.ComboBox, MessageWithOptionSelectionMode.Single);
        }

        public static T ShowWithOptions<T>(string messageBoxText, System.Collections.IList dataSource) where T : IMessageWithOption
        {
            return ShowWithOptions<T>(null, messageBoxText, string.Empty, MessageStyle.Primary, dataSource, MessageWithOptionControlStyle.ComboBox, MessageWithOptionSelectionMode.Single);
        }

        public static T ShowWithOptions<T>(string messageBoxText, System.Collections.IList dataSource, MessageWithOptionControlStyle controlsAs) where T : IMessageWithOption
        {
            return ShowWithOptions<T>(null, messageBoxText, string.Empty, MessageStyle.Primary, dataSource, controlsAs, MessageWithOptionSelectionMode.Single);
        }

        //public static List<T> ShowWithOptions<T>(string messageBoxText, System.Collections.IList dataSource) where T : IMessageWithOption
        //{
        //    return ShowWithOptions<T>(null, messageBoxText, string.Empty, MessageStyle.Primary, dataSource, MessageWithOptionControlStyle.RadioButtonsList);
        //}
    }


    public enum MessageResult
    {
        Cancel,
        No,
        None,
        OK,
        Yes
    }

    public enum MessageButton
    {
        OK,
        OKCancel,
        YesNo,
        YesNoCancel
    }

    public enum MessageImage
    {
        Asterisk,
        Error,
        Exclamation,
        Hand,
        Information,
        None,
        Question,
        Stop,
        Warning
    }

    public enum MessageStyle
    {
        Info,
        Error,
        Warning,
        Primary,
        Question,
        Secondary,
        Success
    }

    public enum MessageOptions
    {
        ButtonsAsButtons,
        ButtonsAsLabels
    }


    public interface IMessageWithOption
    {
        string OptionDescription { get; set; }
        bool IsChecked { get; set; }
    }

    public enum MessageWithOptionControlStyle
    {
        ComboBox,
        RadioButtonsList
    }

    public enum MessageWithOptionSelectionMode
    {
        Single,
        Multiple
    }
}
