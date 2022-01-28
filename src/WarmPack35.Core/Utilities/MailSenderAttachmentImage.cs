using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace WarmPack.Core.Utilities
{
    public class MailSenderAttachmentImage
    {
        public Image Image { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }

        public MailSenderAttachmentImage(string source)
        {
            var file = new FileInfo(source);

            Image = Image.FromFile(source);
            Name = file.Name;


            
        }
    }
}
