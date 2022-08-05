using System;
using System.Collections.Generic;

namespace WarmPack.Extensions
{
    public static class ExceptionExtensions
    {
        private static string LogMessage(Exception ex, string extras = "")
        {
            string msg = string.Format(
                "\r\n => Modulo: {0} \r\n => Clase: {1} \r\n => Metodo: {2} \r\n => Exception: {3} \r\n => StackTrace: {4}\r\n => Extras: {5}\r\n",
                ex?.TargetSite?.Module,
                ex?.TargetSite?.ReflectedType?.FullName,
                ex?.TargetSite?.Name,
                ex?.Message,
                ex?.StackTrace,
                extras
                );

            return msg;
        }

        //public static void Log(this Exception exception, params object[] methodParameters)
        //{
        //    var msg = LogMessage(exception);

        //    var pm = exception.TargetSite.GetParameters();

        //    msg += "\r\n---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\r\n";

        //    Utilities.Log.Write(msg);
        //}

        //public static void Log(this Exception ex, Dictionary<string, object> methodParameters)
        //{
        //    var msg = LogMessage(ex);

        //    var pm = ex.TargetSite.GetParameters();

        //    msg += "\r\n---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\r\n";

        //    Utilities.Log.Write(msg);
        //}

        public static void Log(this Exception ex)
        {
            Log(ex, "", "");
        }

        public static void Log(this Exception ex, string saveDirectory)
        {
            Log(ex, saveDirectory, "");
        }

        public static void Log(this Exception ex, string saveDirectory, string extras)
        {
            var msg = LogMessage(ex, extras);

            var pm = ex.TargetSite.GetParameters();

            msg += "\r\n---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\r\n";

            Utilities.Log.Write(msg, saveDirectory);
        }
    }
}
