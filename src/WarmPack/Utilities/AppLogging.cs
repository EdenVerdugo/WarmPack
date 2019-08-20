using System;

namespace WarmPack.Utilities
{
    public class AppLogging
    {
        public static void Start()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            System.Windows.Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        private static void SaveLog(Exception x)
        {
            string msg = string.Format(
            "\r\n => Modulo: {0} \r\n => Clase: {1} \r\n => Metodo: {2} \r\n => Exception: {3} \r\n",
            x.TargetSite.Module,
            x.TargetSite.ReflectedType.FullName,
            x.TargetSite.Name,
            x.Message
            );

            Log.Write(msg);
        }

        private static void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            SaveLog(e.Exception as Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            SaveLog(e.ExceptionObject as Exception);
        }
    }
}
