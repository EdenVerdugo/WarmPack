using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Web.Rest
{
    public class FormDataContent
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public HttpContent Content { get; set; }

        public FormDataContent()
        {

        }

        public FormDataContent(string name, HttpContent content)
        {
            Name = name;
            Content = content;
        }

        public FormDataContent(string name, string fileName, HttpContent content)
        {
            Name = name;
            FileName = fileName;
            Content = content;
        }
    }
}
