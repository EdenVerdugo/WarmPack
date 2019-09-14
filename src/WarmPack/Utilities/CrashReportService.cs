using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using WarmPack.App;
using WarmPack.Extensions;
#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472
using System.Threading.Tasks;            
#endif

namespace WarmPack.Utilities
{
    public static class CrashReportService
    {
        private static MailSenderAttachmentList _files;

        public static void Start()
        {
            if (AppDomain.CurrentDomain != null)
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            }





#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
#endif
        }

#if NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471 || NET472
        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            SendMail(e.Exception as Exception);
            Environment.Exit(0);
        }
#endif

        public static void AddMailFileOnException(MailSenderAttachment attachment)
        {
            if(_files == null)
            {
                _files = new MailSenderAttachmentList();
            }

            _files.Add(attachment);
        }

        private static bool IgnoreException(Exception exception)
        {
            bool ignore = false;
            _IgnoreWhenExceptionContainsInnerText.ForEach(item =>
            {
                if (exception.Message.ToLower().Contains(item.ToLower()) && !ignore)
                {
                    ignore = true;
                }
            });

            return ignore;
        }

        private static void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (IgnoreException(e.Exception))
                return;

            SendMail(e.Exception as Exception);

            if (CloseApplicationWhenUnhandledError)
                Environment.Exit(0);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (IgnoreException(e.ExceptionObject as Exception))
                return;

            SendMail(e.ExceptionObject as Exception);

            if (CloseApplicationWhenUnhandledError)
                Environment.Exit(0);
        }

        private static string ExceptionsText(Exception ex)
        {
            var text = "";
            if (ex.InnerException != null)
                text = ExceptionsText(ex.InnerException);

            text += $"<div><strong>{ ex.GetType().Name} : </strong>{ ex.Message }</div><hr/><div>{ ex.StackTrace.Replace("\r\n", "<br/>") }</div><br/><br/>";

            return text;
        }

        public static void IgnoreWhenExceptionContains(params string[] exceptionText)
        {
            exceptionText.ForEach(item =>
            {
                _IgnoreWhenExceptionContainsInnerText.Add(item);
            });
        }

        public static void SendMail(Exception ex, bool showMessage = true)
        {

            var builder = new StringBuilder();
            builder.AppendLine("<html><body style='width:95%'>");

            builder.AppendLine("<h2 style='color:#e2710d'>Pila de Excepciones</h2><br>");

            builder.AppendLine(ExceptionsText(ex));

            builder.AppendLine("<h2 style='color:#e2710d'>Captura de pantalla</h2><br>");
            builder.AppendLine("<img src='cid:img_0'/>");

            builder.AppendLine("<h2 style='color:#e2710d'>Versión</h2><br>");
            builder.AppendLine($"{ Globals.ApplicationVersion }");

            builder.AppendLine("<h2 style='color:#e2710d'>Detalles</h2><br>");
            builder.AppendLine($"{ Globals.ApplicationName } - { Globals.ApplicationPath }");

            builder.AppendLine("<h2 style='color:#e2710d'>Equipo</h2><br>");
            builder.AppendLine($"Nombre del equipo: <strong>{ Environment.MachineName }</strong> <br>");
            builder.AppendLine("Direcciones ip encontradas :<br>");

            builder.AppendLine("<ul>");
            foreach(var ip in Helpers.NetworkHelper.GetLocalIPAdresses())
            {
                builder.AppendLine($"<li>{ip}</li>");
            }
            builder.AppendLine($"<li>{ Helpers.NetworkHelper.GetPublicExtternalIPAddress() }</li>");
            builder.AppendLine("</ul>");


            builder.AppendLine("<h2 style='color:#e2710d'>Archivos anexados al correo</h2><br>");
            builder.AppendLine("<ul>");
            foreach (var file in _files)
            {
                builder.AppendLine($"<li>{file.Name}</li>");
            }
            builder.AppendLine("</ul>");

            builder.AppendLine("</body></html>");


            ScreenCapture screen = new ScreenCapture();
            var img = screen.CaptureScreen();

            var lst = new List<Image>();
            lst.Add(img);

            if (showMessage)
            {
                if (CloseApplicationWhenUnhandledError)
                    MessageBox.Show("Ha ocurrido un error en la aplicación y debe cerrarse. Se han recopilado los datos de este error para ser analizados. Pulse aceptar para cerrar la aplicación.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                    MessageBox.Show("Ha ocurrido un error en la aplicacion. Se han recopilado los datos de este error para ser analizados. Pulse aceptar para continuar.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            //var sender = new MailSender("172.19.1.4", 587, false, "correoautomatico@difarmer.com", "Difarmer01");
            var sender = new MailSender(MailServerConfiguration.SmtpServer, MailServerConfiguration.Port, MailServerConfiguration.EnableSSL, MailServerConfiguration.UserName, MailServerConfiguration.Password);                        

            sender.SendWithImages(MailServerConfiguration.UserName, MailConfiguration.ToEmail, MailConfiguration.Subject, builder.ToString(), lst.ToArray(), _files);

        }

        public static bool CloseApplicationWhenUnhandledError { get; set; } = true;
        private static List<string> _IgnoreWhenExceptionContainsInnerText = new List<string>();

        private static CrashReportMailServerConfiguration _MailServerConfiguration = null;
        public static CrashReportMailServerConfiguration MailServerConfiguration
        {
            get
            {
                if (_MailServerConfiguration == null)
                    _MailServerConfiguration = new CrashReportMailServerConfiguration();

                return _MailServerConfiguration;
            }
            set
            {
                _MailServerConfiguration = value;
            }
        }


        private static CrashReportMailConfiguration _MailConfiguration = null;
        public static CrashReportMailConfiguration MailConfiguration
        {
            get
            {
                if (_MailConfiguration == null)
                    _MailConfiguration = new CrashReportMailConfiguration();

                return _MailConfiguration;
            }
            set
            {
                _MailConfiguration = value;
            }
        }

    }

    public class CrashReportMailServerConfiguration
    {
        public CrashReportMailServerConfiguration()
        {

        }

        public CrashReportMailServerConfiguration(string smtServer, int port, bool enableSSL, string userName, string password)
        {
            SmtpServer = smtServer;
            Port = port;
            EnableSSL = enableSSL;
            UserName = userName;
            Password = password;
        }

        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class CrashReportMailConfiguration
    {
        public CrashReportMailConfiguration()
        {

        }

        public CrashReportMailConfiguration(string toEmail, string subject)
        {
            ToEmail = toEmail;
            Subject = subject; 
        }

        public string ToEmail { get; set; }
        public string Subject { get; set; }
    }
}
