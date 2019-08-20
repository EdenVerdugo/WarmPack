using System.IO;
using System.Linq;
using WarmPack.Extensions;

namespace WarmPack.Utilities
{
    public class MailSenderAttachment
    {
        public MailSenderAttachment()
        {

        }

        public MailSenderAttachment(string path, string mimeType = "")
        {
            Name = Path.GetFileName(path);
            FileBuffer = File.ReadAllBytes(path);
            MimeType = mimeType;
        }

        public MailSenderAttachment(string name, byte[] fileBuffer, string mimeType = "")
        {
            Name = name;
            FileBuffer = fileBuffer;
            MimeType = mimeType;
        }

        public MailSenderAttachment(string name, Stream stream, string mimeType = "")
        {
            Name = name;
            FileBuffer = stream.ToArray();
            MimeType = mimeType;
        }

        public string Name { get; set; }
        public byte[] FileBuffer { get; set; }
        public string MimeType { get; set; }
    }
}
