using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WarmPack.Windows.ViewModels
{
    internal class MessageViewModel : ONotifyPropertyChanged<MessageViewModel>
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
                OnPropertyChanged("MessageResult");
            }
        }

        private bool _CanShowYesButton;
        public bool CanShowYesButton
        {
            get
            {
                return _CanShowYesButton;
            }
            set
            {
                _CanShowYesButton = value;
                OnPropertyChanged("CanShowYesButton");
            }
        }


        private bool _CanShowNoButton;
        public bool CanShowNoButton
        {
            get
            {
                return _CanShowNoButton;
            }
            set
            {
                _CanShowNoButton = value;
                OnPropertyChanged("CanShowNoButton");
            }
        }


        private bool _CanShowOKButton;
        public bool CanShowOKButton
        {
            get
            {
                return _CanShowOKButton;
            }
            set
            {
                _CanShowOKButton = value;
                OnPropertyChanged("CanShowOKButton");
            }
        }


        private bool _CanShowCancelButton;
        public bool CanShowCancelButton
        {
            get
            {
                return _CanShowCancelButton;
            }
            set
            {
                _CanShowCancelButton = value;
                OnPropertyChanged("CanShowCancelButton");
            }
        }

        private string _ToolbarColor1;
        public string ToolbarColor1
        {
            get
            {
                return _ToolbarColor1;
            }
            set
            {
                _ToolbarColor1 = value;
                OnPropertyChanged("ToolbarColor1");
            }
        }


        private string _ToolbarColor2;
        public string ToolbarColor2
        {
            get
            {
                return _ToolbarColor2;
            }
            set
            {
                _ToolbarColor2 = value;
                OnPropertyChanged("ToolbarColor2");
            }
        }


        private string _Caption;
        public string Caption
        {
            get
            {
                return _Caption;
            }
            set
            {
                _Caption = value;
                OnPropertyChanged("Caption");
            }
        }


        private string _MessageBoxText;
        public string MessageBoxText
        {
            get
            {
                return _MessageBoxText;
            }
            set
            {
                _MessageBoxText = value;
                OnPropertyChanged("MessageBoxText");
            }
        }


        private MessageStyle _MessageStyle;
        public MessageStyle MessageStyle
        {
            get
            {
                return _MessageStyle;
            }
            set
            {
                _MessageStyle = value;
                OnPropertyChanged("MessageStyle");
            }
        }



        private MessageOptions _MessageOptions;
        public MessageOptions MessageOptions
        {
            get
            {
                return _MessageOptions;
            }
            set
            {
                _MessageOptions = value;
                OnPropertyChanged("MessageOptions");
            }
        }

        private MessageButton _MessageButton;
        public MessageButton MessageButton
        {
            get
            {
                return _MessageButton;
            }
            set
            {
                _MessageButton = value;
                OnPropertyChanged("MessageButton");

                switch (_MessageButton)
                {
                    case MessageButton.YesNo:
                        CanShowYesButton = true;
                        CanShowNoButton = true;
                        break;
                    case MessageButton.YesNoCancel:
                        CanShowYesButton = true;
                        CanShowNoButton = true;
                        CanShowCancelButton = true;
                        break;
                    case MessageButton.OK:
                        CanShowOKButton = true;
                        break;
                    case MessageButton.OKCancel:
                        CanShowOKButton = true;
                        CanShowCancelButton = true;
                        break;
                }
            }
        }

        public MessageViewModel()
        {
            CanShowYesButton = false;
            CanShowNoButton = false;
            CanShowCancelButton = false;
            CanShowOKButton = false;
        }

        public void ApplyMessageStyle()
        {
            switch (MessageStyle)
            {
                case MessageStyle.Info:
                    ToolbarColor1 = "#0479f7";
                    ToolbarColor2 = "#007aff";
                    break;
                case MessageStyle.Warning:
                    ToolbarColor1 = "#ffc926";
                    ToolbarColor2 = "#ffc61c";
                    break;
                case MessageStyle.Error:
                    ToolbarColor1 = "#f92a40";
                    ToolbarColor2 = "#f72239";
                    break;
                case MessageStyle.Primary:
                    ToolbarColor1 = "#0079ff";
                    ToolbarColor2 = "#026fe8";
                    break;
                case MessageStyle.Secondary:
                    //secundario #d0d3d8
                    ToolbarColor1 = "#d0d3d8";
                    ToolbarColor2 = "#8e9196";
                    break;
                case MessageStyle.Success:
                    //secundario #34db5a
                    ToolbarColor1 = "#34db5a";
                    ToolbarColor2 = "#27aa45";
                    break;
                case MessageStyle.Question:
                    ToolbarColor1 = "#81aff9";
                    ToolbarColor2 = "#719add";
                    break;

            }
        }


        private System.Windows.Input.ICommand _OKCommand;
        public virtual System.Windows.Input.ICommand OKCommand
        {
            get
            {
                if (_OKCommand == null)
                {
                    _OKCommand = new RelayCommand<System.Windows.Window>(p => OK(p));
                }

                return _OKCommand;
            }
        }

        private void OK(System.Windows.Window o)
        {
            if (!CanShowOKButton)
                return;

            MessageResult = MessageResult.OK;
            o.Close();
        }


        private System.Windows.Input.ICommand _YesCommand;
        public virtual System.Windows.Input.ICommand YesCommand
        {
            get
            {
                if (_YesCommand == null)
                {
                    _YesCommand = new RelayCommand<System.Windows.Window>(p => Yes(p));
                }

                return _YesCommand;
            }
        }

        private void Yes(System.Windows.Window o)
        {
            if (!CanShowYesButton)
                return;

            MessageResult = MessageResult.Yes;
            o.Close();
        }


        private System.Windows.Input.ICommand _NoCommand;
        public virtual System.Windows.Input.ICommand NoCommand
        {
            get
            {
                if (_NoCommand == null)
                {
                    _NoCommand = new RelayCommand<System.Windows.Window>(p => No(p));
                }

                return _NoCommand;
            }
        }

        private void No(System.Windows.Window o)
        {
            if (!CanShowNoButton)
                return;

            MessageResult = MessageResult.No;
            o.Close();
        }


        private System.Windows.Input.ICommand _CancelCommand;
        public virtual System.Windows.Input.ICommand CancelCommand
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
            if (!CanShowCancelButton)
                return;

            MessageResult = MessageResult.Cancel;
            o.Close();
        }


        private System.Windows.Input.ICommand _KeyOKCommand;
        public virtual System.Windows.Input.ICommand KeyOKCommand
        {
            get
            {
                if (_KeyOKCommand == null)
                {
                    _KeyOKCommand = new RelayCommand<System.Windows.Window>(p => KeyOK(p));
                }

                return _KeyOKCommand;
            }
        }

        private void KeyOK(Window p)
        {
            if (MessageOptions == MessageOptions.ButtonsAsButtons)
                return;

            OK(p);
        }

        private System.Windows.Input.ICommand _KeyNoCommand;
        public virtual System.Windows.Input.ICommand KeyNoCommand
        {
            get
            {
                if (_KeyNoCommand == null)
                {
                    _KeyNoCommand = new RelayCommand<System.Windows.Window>(p => KeyNo(p));
                }

                return _KeyNoCommand;
            }
        }

        private void KeyNo(Window p)
        {
            if (MessageOptions == MessageOptions.ButtonsAsButtons)
                return;

            No(p);
        }

        private System.Windows.Input.ICommand _KeyYesCommand;
        public virtual System.Windows.Input.ICommand KeyYesCommand
        {
            get
            {
                if (_KeyYesCommand == null)
                {
                    _KeyYesCommand = new RelayCommand<System.Windows.Window>(p => KeyYes(p));
                }

                return _KeyYesCommand;
            }
        }

        private void KeyYes(Window p)
        {
            if (MessageOptions == MessageOptions.ButtonsAsButtons)
                return;

            Yes(p);
        }

        private System.Windows.Input.ICommand _KeyCancelCommand;
        public virtual System.Windows.Input.ICommand KeyCancelCommand
        {
            get
            {
                if (_KeyCancelCommand == null)
                {
                    _KeyCancelCommand = new RelayCommand<System.Windows.Window>(p => KeyCancel(p));
                }

                return _KeyCancelCommand;
            }
        }

        private void KeyCancel(Window p)
        {
            if (MessageOptions == MessageOptions.ButtonsAsButtons)
                return;

            Cancel(p);
        }


        private System.Windows.Input.ICommand _PlaySoundCommand;
        public virtual System.Windows.Input.ICommand PlaySoundCommand
        {
            get
            {
                if (_PlaySoundCommand == null)
                {
                    _PlaySoundCommand = new RelayCommand<object>(p => PlaySound(p));
                }

                return _PlaySoundCommand;
            }
        }

        private void PlaySound(object o)
        {
            switch (MessageStyle)
            {
                case MessageStyle.Info:
                    System.Media.SystemSounds.Beep.Play();
                    break;
                case MessageStyle.Warning:
                    System.Media.SystemSounds.Asterisk.Play();
                    break;
                case MessageStyle.Error:
                    System.Media.SystemSounds.Hand.Play();
                    break;
                case MessageStyle.Primary:
                    System.Media.SystemSounds.Beep.Play();
                    break;
                case MessageStyle.Secondary:
                    System.Media.SystemSounds.Beep.Play();
                    break;
                case MessageStyle.Success:
                    System.Media.SystemSounds.Beep.Play();
                    //Softwee.Task.Run(() =>
                    //{
                    //    var soundPlayer = new System.Media.SoundPlayer(@"C:\Windows\Media\Ring10.wav");
                    //    soundPlayer.LoadTimeout = 2000;                        
                    //    soundPlayer.Play();
                    //});                   


                    break;
                case MessageStyle.Question:
                    System.Media.SystemSounds.Question.Play();
                    break;

            }
        }

    }
}
