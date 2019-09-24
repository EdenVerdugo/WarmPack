using System.Linq;

namespace WarmPack.Windows.ViewModels
{
    internal class MessageWithOptionsViewModel : MessageViewModel
    {

        private IMessageWithOption _MessageOptionSelected;
        public IMessageWithOption MessageOptionSelected
        {
            get
            {
                if (MessageWithOptionControlStyle == MessageWithOptionControlStyle.RadioButtonsList && MessageWithOptionSelectionMode == MessageWithOptionSelectionMode.Single)
                {
                    return MessageOptionsDataSource.Cast<IMessageWithOption>().FirstOrDefault(item => item.IsChecked);
                }

                return _MessageOptionSelected;
            }
            set
            {
                _MessageOptionSelected = value;
                OnPropertyChanged("MessageOptionSelected");
            }
        }


        private System.Collections.IList _MessageOptionsDataSource;
        public System.Collections.IList MessageOptionsDataSource
        {
            get
            {
                return _MessageOptionsDataSource;
            }
            set
            {
                _MessageOptionsDataSource = value;
                OnPropertyChanged("MessageOptionsDataSource");
            }
        }



        private System.Collections.IList _MessageOptionsDataSourceMultiple;
        public System.Collections.IList MessageOptionsDataSourceMultiple
        {
            get
            {
                return _MessageOptionsDataSourceMultiple;
            }
            set
            {
                _MessageOptionsDataSourceMultiple = value;
                OnPropertyChanged("MessageOptionsDataSourceMultiple");
            }
        }

        private MessageWithOptionControlStyle _MessageWithOptionControlStyle;
        public MessageWithOptionControlStyle MessageWithOptionControlStyle
        {
            get
            {
                return _MessageWithOptionControlStyle;
            }
            set
            {
                _MessageWithOptionControlStyle = value;
                OnPropertyChanged("MessageWithOptionControlStyle");
            }
        }


        private MessageWithOptionSelectionMode _MessageWithOptionSelectionMode;
        public MessageWithOptionSelectionMode MessageWithOptionSelectionMode
        {
            get
            {
                return _MessageWithOptionSelectionMode;
            }
            set
            {
                _MessageWithOptionSelectionMode = value;
                OnPropertyChanged("MessageWithOptionSelectionMode");
            }
        }

        public MessageWithOptionsViewModel()
        {
            MessageWithOptionSelectionMode = MessageWithOptionSelectionMode.Single;
        }
    }
}
