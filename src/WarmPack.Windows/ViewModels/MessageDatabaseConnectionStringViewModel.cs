using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.DataModel;

namespace WarmPack.Windows.ViewModels
{
    public class MessageDatabaseConnectionStringViewModel : ONotifyPropertyChanged<MessageDatabaseConnectionStringViewModel>
    {        
        private MessageResult _MessageResult;
        public MessageResult MessageResult
        {
            get
            {
                return _MessageResult;
            }
            set
            {
                _MessageResult = value;
                OnPropertyChanged(p => p.MessageResult);
            }
        }

        private string _DataSource;
        public string DataSource
        {
            get
            {
                return _DataSource;
            }
            set
            {
                _DataSource = value;
                OnPropertyChanged(p => p.DataSource);
            }
        }


        private string _Database;
        public string Database
        {
            get
            {
                return _Database;
            }
            set
            {
                _Database = value;
                OnPropertyChanged(p => p.Database);
            }
        }


        private string _UserId;
        public string UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                _UserId = value;
                OnPropertyChanged(p => p.UserId);
            }
        }


        private string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                OnPropertyChanged(p => p.Password);
            }
        }


        private System.Windows.Input.ICommand _OkCommand;
        public System.Windows.Input.ICommand OkCommand
        {
            get
            {
                if (_OkCommand == null)
                {
                    _OkCommand = new RelayCommand<System.Windows.Window>(p => Ok(p));
                }

                return _OkCommand;
            }
        }

        private void Ok(System.Windows.Window o)
        {
            MessageResult = MessageResult.OK;

            o.Close();
        }


        private System.Windows.Input.ICommand _CancelCommand;
        public System.Windows.Input.ICommand CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                {
                    _CancelCommand = new RelayCommand<System.Windows.Window>(p => Cancel(p));
                }

                return _CancelCommand;
            }
        }

        private void Cancel(System.Windows.Window o)
        {
            MessageResult = MessageResult.Cancel;
            //acciones
            o.Close();
        }


    }
}
