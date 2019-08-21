using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.App;
using WarmPack.Classes;
using WarmPack.Windows.Controls;

namespace WarmPack.Windows.App
{
    public static class AppConfigurationExtensions
    {
        private static Castable TryParameter(this AppConfiguration app, string name, bool decrypt, bool creationModal, TextBoxExInputType typeInput = TextBoxExInputType.Text, string message = null)
        {            
            return app.TryParameter(name, decrypt, () =>
            {
                var parameter = Message.ShowInput(message == null ? $"Por favor introduzca un valor para el parametro \"{ name }\" :" : message, MessageStyle.Question, typeInput);

                app.Create(AppConfigurationType.Parameter, name, parameter, decrypt);

                return new Castable(parameter);
            });
        }

        public static string TryConnectionString(this AppConfiguration app, string name, bool decrypt, bool creationModal, string message = null)
        {
            return app.TryConnectionString(name, decrypt, () =>
            {
                var connectionString = Message.ShowInput(message == null ? $"Por favor introduzca un valor para la cadena de conexion \"{ name }\" :" : message, MessageStyle.Question);

                app.Create(AppConfigurationType.ConexionString, name, connectionString, decrypt);

                return app.ConnectionString(name, decrypt);
            });
        }
    }
}
