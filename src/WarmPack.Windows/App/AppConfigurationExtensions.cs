using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.App;
using WarmPack.Classes;
using WarmPack.Data;
using WarmPack.Threading;
using WarmPack.Windows.Controls;

namespace WarmPack.Windows.App
{       
    public class AppConfigurationConexionOptions
    {
        internal AppConfigurationConexionOptions _instance;
        internal AppConfiguration _app;

        internal string _name;
        internal bool _decrypt;               
        internal string _comment;
        internal Func<ConnectionString, bool> _action;
        internal bool _creationModal;

        public AppConfigurationConexionOptions(AppConfigurationConexionOptions options)
        {
            _instance = options;
            _app = _instance._app;
        }

        public AppConfigurationConexionOptions(AppConfiguration app, string name, bool decrypt)
        {
            _app = app;
            _name = name;
            _decrypt = decrypt;
        }

        public AppConfigurationConexionOptionsRequest RequestIfNotFound()
        {
            _creationModal = true;                       

            return new AppConfigurationConexionOptionsRequest(this);
        }

        public AppConfigurationConexionOptions Comment(string comment)
        {
            _comment = comment;
            return this;
        }        

        public string Value()
        {
            return _app.TryConnectionString(_name, _decrypt, _creationModal, _action, _comment);
        }
    }

    public class AppConfigurationConexionOptionsRequest : AppConfigurationConexionOptions
    {
        public AppConfigurationConexionOptionsRequest(AppConfigurationConexionOptions options) : base(options)
        {
            _instance = options;
        }

        public AppConfigurationConexionOptions Test(Func<ConnectionString, bool> action)
        {
            _instance._action = action;

            return _instance;
        }
    }

    public class AppConfigurationParameterOptions
    {
        internal AppConfigurationParameterOptions _instance;
        internal AppConfiguration _app;

        internal string _name;
        internal bool _decrypt;
        internal string _message;
        internal TextBoxExInputType _typeInput;
        internal string _comment;
        internal Action<Castable> _action;
        internal bool _creationModal;
        internal object _defaultValue;

        public AppConfigurationParameterOptions(AppConfigurationParameterOptions options)
        {            
            _instance = options;
            _app = _instance._app;
        }

        public AppConfigurationParameterOptions(AppConfiguration app, string name, bool decrypt)
        {
            _app = app;
            _name = name;
            _decrypt = decrypt;
        }

        public AppConfigurationParameterOptionsRequest RequestIfNotFound(string message = null, TextBoxExInputType typeInput = TextBoxExInputType.Text)
        {
            _creationModal = true;
            _message = message;
            _typeInput = typeInput;

            return new AppConfigurationParameterOptionsRequest(this);
        }        

        public AppConfigurationParameterOptions DefaultValue(object defaultValue)
        {
            _defaultValue = defaultValue;

            return this;
        }

        public AppConfigurationParameterOptions Comment(string comment)
        {
            _comment = comment;
            return this;
        }
        
        public Castable Value()
        {
            if (_creationModal)
            {
                return _app.TryParameter(_name, _decrypt, _creationModal, _typeInput, _message, _action, _comment);
            }

            else
            {
                var param = _app.TryParameter(_name, _decrypt, () => new Castable(_defaultValue), _comment);

                if (param != null && _action != null)
                {
                    _action(param);
                }

                return param;
            }                
        }
    }

    public class AppConfigurationParameterOptionsRequest : AppConfigurationParameterOptions 
    {
        public AppConfigurationParameterOptionsRequest(AppConfigurationParameterOptions options) : base(options)
        {
            _instance = options;            
        }

        public AppConfigurationParameterOptions AfterRequest(Action<Castable> action)
        {
            _instance._action = action;

            return _instance;
        }
    }

    public static class AppConfigurationExtensions
    {
        public static AppConfigurationParameterOptions TryParameter(this AppConfiguration app, string name, bool decrypt)
        {
            return new AppConfigurationParameterOptions(app, name, decrypt);
        }

        public static Castable TryParameter(this AppConfiguration app, string name, bool decrypt, bool creationModal, TextBoxExInputType typeInput = TextBoxExInputType.Text, string message = null, string comment = null)
        {
            return TryParameter(app, name, decrypt, creationModal, typeInput, message, null, comment);
        }

        public static Castable TryParameter(this AppConfiguration app, string name, bool decrypt, bool creationModal, TextBoxExInputType typeInput = TextBoxExInputType.Text, string message = null, Action<Castable> afterCreationModal = null, string comment = null)
        {         
            var param = app.TryParameter(name, decrypt, () =>
            {
                var parameter = Message.ShowInput(message == null ? $"Por favor introduzca un valor para el parametro \"{ name }\" :" : message, MessageStyle.Question, typeInput);

                return parameter == string.Empty ? null : new Castable(parameter);
            },
            comment);


            if(param != null && afterCreationModal != null)
            {
                afterCreationModal(param);
            }

            return param;
        }

        public static AppConfigurationConexionOptions TryConnectionString(this AppConfiguration app, string name, bool decrypt)
        {
            return new AppConfigurationConexionOptions(app, name, decrypt);
        }

        public static string TryConnectionString(this AppConfiguration app, string name, bool decrypt, bool creationModal, string message = null, string comment = null)
        {
            return app.TryConnectionString(name, decrypt, () =>
            {
                var connectionString = Message.ShowInput(message == null ? $"Por favor introduzca un valor para la cadena de conexion \"{ name }\" :" : message, MessageStyle.Question);

                return connectionString == string.Empty ? null : connectionString;
            }, 
            comment);
        }

        /*
        private static void ShowConnectionForm(Func<ConnectionString, bool> testFunc)
        {
            var connectionString = Message.ShowDatabaseConnection();
            bool testSuccess = false;

            if(connectionString == null)
            {
                ShowConnectionForm(testFunc);
            }

            Splash.RunTask(() =>
            {
                testSuccess = testFunc(connectionString);
            }, "Estableciendo conexion con el servidor ...")
            .Completed(() =>
            {
                if (!testSuccess)
                {
                    Message.Show("No se pudo establecer la conexión con el servidor, intente de nuevo...", "", MessageButton.OK, MessageStyle.Error);
                    ShowConnectionForm(testFunc);
                }
            });            
        }
        */
        
        public static string TryConnectionString(this AppConfiguration app, string name, bool decrypt, bool creationModal, Func<ConnectionString, bool> testFunc, string comment = null)
        {
            return app.TryConnectionString(name, decrypt, () =>
            {
                var connectionString = Message.ShowDatabaseConnection();

                bool success = false;

                if(connectionString != null)
                {
                    Splash.Show();
                    success = testFunc(connectionString);
                    Splash.Hide();
                }                

                if(success)
                    Message.Show("Se ha establecido la conexión con el servidor correctamente", "", MessageButton.OK, MessageStyle.Success);
                else
                    Message.Show("No se pudo establecer la conexión con el servidor", "", MessageButton.OK, MessageStyle.Error);

                return success ? connectionString.ToMsqlConnectionString() : null;
            },
            comment);
        }
    }
}
