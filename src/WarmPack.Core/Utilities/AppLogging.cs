﻿using System;

namespace WarmPack.Utilities
{
    public class AppLogging
    {
        public static void Start()
        {
            if (AppDomain.CurrentDomain != null)
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            }                

            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            }
        }

        private static string GetTextException(Exception ex)
        {
            if (ex == null)
                return "";

            return $"Exception: {ex.Message}\r\nStackTrace: {ex.StackTrace}" + GetTextException(ex.InnerException);
        }

        private static void SaveLog(Exception x)
        {
            string msg = string.Format($"Modulo: {x.TargetSite.Module}\r\nClase: {x.TargetSite.Module}\r\n Pila de Excepciones: \r\n{ GetTextException(x) }");
                   
            /*
            string msg = string.Format(
            "\r\n => Modulo: {0} \r\n => Clase: {1} \r\n => Metodo: {2} \r\n => Exception: {3} \r\n",
            x.TargetSite.Module,
            x.TargetSite.ReflectedType.FullName,
            x.TargetSite.Name,
            x.Message
            );
            */
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