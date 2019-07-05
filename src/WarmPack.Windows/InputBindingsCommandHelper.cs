using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WarmPack.Windows
{
    // clase tomada de https://www.developpez.net/forums/d1098409/dotnet/developpement-windows/windows-presentation-foundation/mvvm-net3-5-inputbinding-binding-command-commandparameter/
    public class InputBindingsCommandHelper : Freezable, ICommand
    {
        public InputBindingsCommandHelper()
        {
            // Blank
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(InputBindingsCommandHelper), new PropertyMetadata(new PropertyChangedCallback(OnCommandChanged)));
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(InputBindingsCommandHelper), new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            if (Command != null)
            {
                if (CommandParameter != null)
                    return Command.CanExecute(CommandParameter);
                else
                    return Command.CanExecute(parameter);
            }
            return false;
        }

        public void Execute(object parameter)
        {
            if (CommandParameter != null)
                Command.Execute(CommandParameter);
            else
                Command.Execute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InputBindingsCommandHelper commandReference = d as InputBindingsCommandHelper;
            ICommand oldCommand = e.OldValue as ICommand;
            ICommand newCommand = e.NewValue as ICommand;

            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= commandReference.CanExecuteChanged;
            }
            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += commandReference.CanExecuteChanged;
            }
        }

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
