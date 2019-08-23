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
    public static class AppConfigurationExtensions
    {
        public static Castable TryParameter(this AppConfiguration app, string name, bool decrypt, bool creationModal, TextBoxExInputType typeInput = TextBoxExInputType.Text, string message = null, string comment = null)
        {            
            return app.TryParameter(name, decrypt, () =>
            {
                var parameter = Message.ShowInput(message == null ? $"Por favor introduzca un valor para el parametro \"{ name }\" :" : message, MessageStyle.Question, typeInput);                                

                return parameter == string.Empty ? null : new Castable( parameter);
            },
            comment);
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
