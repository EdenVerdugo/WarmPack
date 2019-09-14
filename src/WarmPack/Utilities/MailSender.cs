using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace WarmPack.Utilities
{
    public class MailSender
    {
        private SmtpClient _client = null;

        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public MailSender(string smtpServer, int port, bool enableSSL, string userName, string password)
        {
            SmtpServer = smtpServer;
            Port = port;
            EnableSSL = enableSSL;
            UserName = userName;
            Password = password;

            _client = new SmtpClient();
            _client.Host = SmtpServer;
            _client.Port = Port;
            _client.EnableSsl = EnableSSL;
            _client.UseDefaultCredentials = false;
            _client.Credentials = new NetworkCredential(UserName, Password);
        }

        public void Send(string fromEmail, string toEmail, string subject, string content, bool isBodyHtml)
        {
            using (var msg = new MailMessage(fromEmail, toEmail, subject, content))
            {
                msg.IsBodyHtml = isBodyHtml;

                _client.Send(msg);
            }
        }

        public void SendWithImages(string fromEmail, string toEmail, string subject, string content, Image[] images, MailSenderAttachmentList files = null)
        {
            using (var msg = new MailMessage(fromEmail, toEmail, subject, content))
            {
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;

                msg.BodyEncoding = Encoding.UTF8;

                var i = 0;
                foreach (var img in images)
                {
                    var ms = new System.IO.MemoryStream();
                    img.Save(ms, ImageFormat.Jpeg);

                    ms.Position = 0;

                    var att = new Attachment(ms, $"{ Guid.NewGuid() }.jpg", "image/jpg");
                    att.ContentId = $"img_{i}";

                    msg.Attachments.Add(att);

                    i++;
                }

                if (files != null)
                    foreach (var file in files)
                    {
                        var ms = new System.IO.MemoryStream(file.FileBuffer);
                        var att = new Attachment(ms, file.Name, string.IsNullOrEmpty(file.MimeType) ? "application/octet-stream" : file.MimeType);

                        msg.Attachments.Add(att);

                        i++;
                    }

                _client.Send(msg);
            }
        }

        public void SendWithFiles(string fromEmail, string toEmail, string subject, string content, System.IO.FileStream[] files, string mimeType = "")
        {
            using (var msg = new MailMessage(fromEmail, toEmail, subject, content))
            {
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;

                msg.BodyEncoding = Encoding.UTF8;

                var i = 0;
                foreach (var file in files)
                {
                    file.Position = 0;

                    var att = new Attachment(file, file.Name, string.IsNullOrEmpty(mimeType) ? "application/octet-stream" : mimeType);
                    //att.ContentId = $"img_{i}";

                    msg.Attachments.Add(att);

                    i++;
                }

                _client.Send(msg);
            }
        }


        public void SendWithFiles(string fromEmail, string toEmail, string subject, string content, params MailSenderAttachment[] files)
        {
            using (var msg = new MailMessage(fromEmail, toEmail, subject, content))
            {
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;

                msg.BodyEncoding = Encoding.UTF8;

                var i = 0;
                foreach (var file in files)
                {
                    var ms = new System.IO.MemoryStream(file.FileBuffer);
                    var att = new Attachment(ms, file.Name, string.IsNullOrEmpty(file.MimeType) ? "application/octet-stream" : file.MimeType);

                    msg.Attachments.Add(att);

                    i++;
                }

                _client.Send(msg);
            }
        }

        public void SendWithFiles(string fromEmail, string toEmail, string subject, string content, MailSenderAttachmentList files)
        {
            using (var msg = new MailMessage(fromEmail, toEmail, subject, content))
            {
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;

                msg.BodyEncoding = Encoding.UTF8;

                var i = 0;
                foreach (var file in files)
                {
                    var ms = new System.IO.MemoryStream(file.FileBuffer);
                    var att = new Attachment(ms, file.Name, string.IsNullOrEmpty(file.MimeType) ? "application/octet-stream" : file.MimeType);

                    msg.Attachments.Add(att);

                    i++;
                }

                _client.Send(msg);
            }
        }
    }
}
