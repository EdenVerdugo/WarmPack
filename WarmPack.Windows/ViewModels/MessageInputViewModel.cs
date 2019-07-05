using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Windows.Controls;

namespace WarmPack.Windows.ViewModels
{
    internal class MessageInputViewModel : MessageViewModel
    {

        private string _MessageInputText;
        public string MessageInputText
        {
            get
            {
                return _MessageInputText;
            }
            set
            {
                _MessageInputText = value;
                OnPropertyChanged("MessageInputText");
            }
        }


        private TextBoxExInputType _MessageInputType;
        public TextBoxExInputType MessageInputType
        {
            get
            {
                return _MessageInputType;
            }
            set
            {
                _MessageInputType = value;
                OnPropertyChanged("MessageInputType");
            }
        }

        public MessageInputViewModel()
        {
            MessageInputText = string.Empty;
            MessageInputType = TextBoxExInputType.Text;
        }


        private System.Windows.Input.ICommand _KeyOKCommand;
        public override System.Windows.Input.ICommand KeyOKCommand
        {
            get
            {
                if (_KeyOKCommand == null)
                {
                    _KeyOKCommand = new RelayCommand<CommandParameterOnKeyDown>(p => KeyOK(p));
                }

                return _KeyOKCommand;
            }
        }

        private void KeyOK(CommandParameterOnKeyDown o)
        {
            MessageResult = MessageResult.OK;
            ((System.Windows.Window)o.CommandParameter)?.Close();
        }


    }
}
