using System.IO;
using System.Text;
using System.Xml;

namespace WarmPack.Extensions
{
    public static class XmlDocumentExtensions
    {
        public static string Indent(this XmlDocument doc)
        {
            string result = "";
            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            /*
            Encoding utf8noBOM = new UTF8Encoding(false);
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace,
                Encoding = utf8noBOM
            };
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {                
                doc.Save(writer);
            }
            */
            Encoding utf8noBOM = new UTF8Encoding(false);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = utf8noBOM;
            using (MemoryStream output = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(output, settings))
                {
                    doc.Save(writer);
                }
                result = Encoding.Default.GetString(output.ToArray());
            }

            return result;
        }
    }
}
