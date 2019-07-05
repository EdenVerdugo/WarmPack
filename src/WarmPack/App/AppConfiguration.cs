using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WarmPack.Classes;
using WarmPack.Extensions;
using WarmPack.Utilities;

namespace WarmPack.App
{
    //       Autor: L.I. Eden Verdugo
    //       Fecha: 2018/08/10    
    // Comentarios: Hice esta clase pensando facilitar el uso de los archivos de configuración para las aplicaciones de toda la vida.
    /// <summary>
    /// Se encarga de generar y obtener un archivo de configuración para la aplicación, con sus parametros y cadenas de conexion.
    /// </summary>
    public class AppConfiguration
    {
        private const string CONF = "Configuracion";
        private const string CONF_Parameters = "Parametros";
        private const string CONF_Parameter = "Parametro";
        private const string CONF_Parameter_Name = "nombre";
        private const string CONF_Parameter_Value = "valor";

        private const string CONF_Connections = "Conexiones";
        private const string CONF_ConnectionString = "CadenaConexion";
        private const string CONF_ConnectionString_Name = "nombre";
        private const string CONF_ConnectionString_Value = "valor";


        private const string DEFAULT_USER_KEY = "#C0nF1gUr4T10n!";

        private const string DEFAULT_CONNECTIONSTRING = "Data Source = localhost; Initial catalog = master; User=sa; Password=123";

        private XmlDocument _xml;
        private Encrypter _encrypter;

        public string Path { get; set; }

        /// <summary>
        /// Crea un archivo de configuración para la aplicación.
        /// </summary>
        public AppConfiguration()
        {
            var path = string.Format(@"C:\Configuration\{0}.xml", Globals.ApplicationName == null ? "asp_config" : Globals.ApplicationName);

            this.Initialice(path, DEFAULT_USER_KEY);
        }
        /// <summary>
        /// Crea un archivo de configuración para la aplicación.
        /// </summary>
        /// <param name="path">Ruta donde se creará el archivo de configuración.</param>
        public AppConfiguration(string path)
        {
            this.Initialice(path, DEFAULT_USER_KEY);
        }
        /// <summary>
        /// Crea un archivo de configuración para la aplicación.
        /// </summary>
        /// <param name="path">Ruta donde se creará el archivo de configuración.</param>
        /// <param name="userKey">Semilla de encriptación para el archivo de configuración.</param>
        public AppConfiguration(string path, string userKey)
        {
            this.Initialice(path, userKey);
        }

        private void Initialice(string path, string userKey)
        {
            this.Path = path;

            var fileInfo = new FileInfo(path);

            var directory = new DirectoryInfo(fileInfo.DirectoryName);

            foreach (var dir in directory.GetDirectoryPaths())
            {
                if (!Directory.Exists(dir.FullName))
                    Directory.CreateDirectory(dir.FullName);
            }

            if (System.IO.File.Exists(Path) == false)
            {
                XDocument config = new XDocument(new XElement(CONF));

                config.Element(CONF).Add(new XElement(CONF_Parameters));
                config.Element(CONF).Add(new XElement(CONF_Connections));

                config.Save(Path);
            }

            _encrypter = new Encrypter(userKey);

            _xml = new XmlDocument();
            _xml.Load(Path);
        }

        /// <summary>
        /// Obtiene el valor del parametro.
        /// </summary>
        /// <param name="name">Nombre del parametro.</param>
        /// <returns></returns>
        public Castable Parameter(String name)
        {
            return Parameter(name, false, false, "", false);
        }
        /// <summary>
        /// Obtiene el valor del parametro.
        /// </summary>
        /// <param name="name">Nombre del parametro.</param>
        /// <param name="decrypt">true para desencriptar el parametro</param>
        /// <returns></returns>
        public Castable Parameter(String name, bool decrypt)
        {
            return Parameter(name, decrypt, false, "", false);
        }
        /// <summary>
        /// Obtiene el valor del parametro.
        /// </summary>
        /// <param name="name">Nombre del parametro.</param>
        /// <param name="createIfNotExists">true para crear el archivo si no existe.</param>
        /// <param name="defaultValue">Valor por default para el parametro en caso de que no exista.</param>
        /// <returns></returns>
        public Castable Parameter(String name, bool createIfNotExists, string defaultValue)
        {
            return Parameter(name, false, createIfNotExists, defaultValue, false);
        }
        /// <summary>
        /// Obtiene el valor del parametro.
        /// </summary>
        /// <param name="name">Nombre del parametro.</param>
        /// <param name="createIfNotExists">true para crear el archivo si no existe.</param>
        /// <param name="defaultValue">Valor por default para el parametro en caso de que no exista.</param>
        /// <param name="encrypt">true para encriptar el valor del parametro en caso de que no exista.</param>
        /// <returns></returns>
        public Castable Parameter(String name, bool createIfNotExists, string defaultValue, bool encrypt)
        {
            return Parameter(name, false, createIfNotExists, defaultValue, encrypt);
        }

        private Castable Parameter(String name, bool decrypt, bool createIfNotExists, string defaultValue, bool encrypt)
        {
            String result = null;

            XmlNodeList nodes = _xml.DocumentElement.GetElementsByTagName(CONF_Parameters)[0].SelectNodes(CONF_Parameter);
            Boolean finded = false;
            foreach (XmlNode nod in nodes)
            {
                if (nod.Attributes[CONF_Parameter_Name].Value == name)
                {
                    result = nod.Attributes[CONF_Parameter_Value].Value;
                    result = decrypt ? _encrypter.Decrypt(result) : result;

                    finded = true;
                    break;
                }
            }

            if (!finded)
            {
                if (createIfNotExists)
                {
                    XmlElement parametro = _xml.CreateElement(CONF_Parameter);
                    parametro.SetAttribute(CONF_Parameter_Name, name);
                    parametro.SetAttribute(CONF_Parameter_Value, encrypt ? _encrypter.Encrypt(defaultValue) : defaultValue);

                    _xml.DocumentElement.GetElementsByTagName(CONF_Parameters)[0].AppendChild(parametro);

                    _xml.Save(this.Path);

                    result = defaultValue;
                }
            }

            return new Castable(result);
        }

        /// <summary>
        /// Asigna un valor a un parametro
        /// </summary>
        /// <param name="name">Nombre del parametro.</param>
        /// <param name="value">Valor del parametro.</param>
        public void ParameterSetValue(string name, string value)
        {
            Boolean finded = false;
            XmlNodeList nodes = _xml.DocumentElement.GetElementsByTagName(CONF_Parameters)[0].SelectNodes(CONF_Parameter);

            foreach (XmlNode nod in nodes)
            {
                if (nod.Attributes[CONF_Parameter_Name].Value == name)
                {
                    nod.Attributes[CONF_Parameter_Value].Value = value;
                    finded = true;
                }
            }

            if (finded == false)
            {
                XmlElement parametro = _xml.CreateElement(CONF_Parameter);
                parametro.SetAttribute(CONF_Parameter_Name, name);
                parametro.SetAttribute(CONF_Parameter_Value, value);

                _xml.DocumentElement.GetElementsByTagName(CONF_Parameters)[0].AppendChild(parametro);
            }

            _xml.Save(this.Path);
        }
        /// <summary>
        /// Obtiene la cadena de conexion.
        /// </summary>
        /// <param name="name">Nombre de la cadena de conexión.</param>
        /// <returns></returns>
        public String ConnectionString(String name)
        {
            String result = null;

            XmlNodeList nodes = _xml.DocumentElement.GetElementsByTagName(CONF_Connections)[0].SelectNodes(CONF_ConnectionString);

            foreach (XmlNode nod in nodes)
            {
                if (nod.Attributes[CONF_ConnectionString_Name].Value == name)
                {
                    result = nod.Attributes[CONF_ConnectionString_Value].Value;
                }
            }

            return result;
        }
        /// <summary>
        /// Obtiene la cadena de conexion.
        /// </summary>
        /// <param name="name">Nombre de la cadena de conexión.</param>
        /// <param name="decrypt">true para desencriptar.</param>
        /// <param name="createIfNotExists">true para crear la cadena de conexión si no existe.</param>
        /// <param name="defaultValue">Valor por default para la cadena de conexión en caso de que no exista.</param>
        /// <returns></returns>
        public String ConnectionString(String name, Boolean decrypt, Boolean createIfNotExists, string defaultValue)
        {
            String result = null;

            if (_xml.DocumentElement.GetElementsByTagName(CONF_Connections).Count == 0)
            {
                XmlElement con = _xml.CreateElement(CONF_Connections);
                _xml.DocumentElement.AppendChild(con);
            }

            XmlNodeList nodes = _xml.DocumentElement.GetElementsByTagName(CONF_Connections)[0].SelectNodes(CONF_ConnectionString);
            Boolean finded = false;
            foreach (XmlNode nod in nodes)
            {
                if (nod.Attributes[CONF_ConnectionString_Name].Value == name)
                {
                    result = nod.Attributes[CONF_ConnectionString_Value].Value;
                    finded = true;
                }
            }

            if (!finded)
            {
                if (createIfNotExists)
                {
                    XmlElement parametro = _xml.CreateElement(CONF_ConnectionString);
                    parametro.SetAttribute(CONF_ConnectionString_Name, name);

                    if (defaultValue != string.Empty)
                    {
                        parametro.SetAttribute(CONF_ConnectionString_Value, defaultValue);
                    }
                    else
                    {
                        parametro.SetAttribute(CONF_ConnectionString_Value, _encrypter.Encrypt(DEFAULT_CONNECTIONSTRING));
                    }

                    _xml.DocumentElement.GetElementsByTagName(CONF_Connections)[0].AppendChild(parametro);

                    _xml.Save(Path);

                    result = defaultValue;
                }
            }

            if (decrypt)
            {
                result = _encrypter.Decrypt(result);
            }

            return result;
        }
    }
}
