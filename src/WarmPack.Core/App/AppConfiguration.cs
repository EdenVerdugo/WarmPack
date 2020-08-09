using System;
using System.IO;
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

        private string GetDefaultPath()
        {
            return string.Format(@"C:\Configuration\{0}.xml", Globals.ApplicationName == null ? "asp_config" : Globals.ApplicationName);
        }

        /// <summary>
        /// Crea un archivo de configuración para la aplicación.
        /// </summary>
        public AppConfiguration()
        {
            var path = GetDefaultPath();

            this.Initialice(path, new Encrypter(DEFAULT_USER_KEY));
        }
        /// <summary>
        /// Crea un archivo de configuración para la aplicación.
        /// </summary>
        /// <param name="path">Ruta donde se creará el archivo de configuración.</param>
        public AppConfiguration(string path)
        {
            this.Initialice(path, new Encrypter(DEFAULT_USER_KEY));
        }
        /// <summary>
        /// Crea un archivo de configuración para la aplicación.
        /// </summary>
        /// <param name="path">Ruta donde se creará el archivo de configuración.</param>
        /// <param name="userKey">Semilla de encriptación para el archivo de configuración.</param>
        public AppConfiguration(string path, Encrypter encrypter)
        {
            this.Initialice(path, encrypter);
        }

        public AppConfiguration(Encrypter encrypter)
        {
            var path = GetDefaultPath();

            this.Initialice(path, encrypter);
        }

        private void Initialice(string path, Encrypter encrypter)
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

            _encrypter = encrypter;

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
        [Obsolete("ParameterSetValue is deprecated, please use Update instead")]
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

        public string ConnectionString(string name, bool decrypt)
        {
            return ConnectionString(name, decrypt, false, "");
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
                        parametro.SetAttribute(CONF_ConnectionString_Value, decrypt ? _encrypter.Encrypt(DEFAULT_CONNECTIONSTRING) : DEFAULT_CONNECTIONSTRING);
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

        public void Create(AppConfigurationType type, string name, string value, bool encrypt, string comment = null)
        {
            if (type == AppConfigurationType.Parameter)
            {
                XmlElement parametro = _xml.CreateElement(CONF_Parameter);

                parametro.SetAttribute(CONF_Parameter_Name, name);
                parametro.SetAttribute(CONF_Parameter_Value, encrypt ? _encrypter.Encrypt(value) : value);
                
                _xml.DocumentElement.GetElementsByTagName(CONF_Parameters)[0].AppendChild(parametro);

                if(comment != null)
                {
                    _xml.DocumentElement.GetElementsByTagName(CONF_Parameters)[0].InsertBefore(_xml.CreateComment(comment), parametro);                    
                }                

                _xml.Save(this.Path);
            }
            else
            {
                XmlElement parametro = _xml.CreateElement(CONF_ConnectionString);

                parametro.SetAttribute(CONF_ConnectionString_Name, name);
                parametro.SetAttribute(CONF_ConnectionString_Value, encrypt ? _encrypter.Encrypt(value) : value);

                _xml.DocumentElement.GetElementsByTagName(CONF_Connections)[0].AppendChild(parametro);

                if (comment != null)
                {                    
                    _xml.DocumentElement.GetElementsByTagName(CONF_Connections)[0].InsertBefore(_xml.CreateComment(comment), parametro);
                }

                _xml.Save(Path);
            }
        }

        public void Update(AppConfigurationType type, string name, string value, bool encrypt, string comment = null)
        {
            bool finded = false;

            if (type == AppConfigurationType.Parameter)
            {                
                var nodes = _xml.DocumentElement.GetElementsByTagName(CONF_Parameters)[0].SelectNodes(CONF_Parameter);

                foreach (XmlNode nod in nodes)
                {
                    if (nod.Attributes[CONF_Parameter_Name].Value == name)
                    {
                        nod.Attributes[CONF_Parameter_Value].Value = value;

                        finded = true;

                        if (comment != null)
                        {
                            XmlComment prevComment = (XmlComment)nod.PreviousSibling;

                            if(prevComment == null)
                            {
                                _xml.InsertBefore(_xml.CreateComment(comment), nod);
                            }                                
                            else
                            {
                                prevComment.Value = comment;                                
                            }
                        }

                        break;
                    }
                }

                if (!finded)
                {
                    throw new Exception($"the parameter {name} was not found");
                }
                                
                _xml.Save(this.Path);
            }
            else
            {                
                var nodes = _xml.DocumentElement.GetElementsByTagName(CONF_Connections)[0].SelectNodes(CONF_ConnectionString);

                foreach (XmlNode nod in nodes)
                {
                    if (nod.Attributes[CONF_ConnectionString_Name].Value == name)
                    {
                        nod.Attributes[CONF_ConnectionString_Value].Value = value;

                        finded = true;

                        if (comment != null)
                        {
                            _xml.InsertBefore(_xml.CreateComment(comment), nod);
                        }

                        break;
                    }
                }

                if (!finded)
                {
                    throw new Exception($"the parameter {name} was not found");
                }

                _xml.Save(this.Path);
            }
        }


        public Castable TryParameter(String name, bool decrypt, Func<Castable> onError, string comment = null)
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
                var value = onError();

                if (value != null)
                    Create(AppConfigurationType.Parameter, name, value.ToString(), decrypt, comment);
                else
                    value = new Castable("");

                return value;
            }

            return new Castable(result);
        }

        public T TryParameter<T>(String name, bool decrypt, Func<T> onError, string comment = null) where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            string result = null;

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
                var value = onError();
                
                Create(AppConfigurationType.Parameter, name, value.ToString(), decrypt, comment);

                return value;
            }

            return (T)Convert.ChangeType(result, typeof(T));
        }

        public string TryConnectionString(string name, bool decrypt, Func<string> onError, string comment = null)
        {
            string result = null;

            if (_xml.DocumentElement.GetElementsByTagName(CONF_Connections).Count == 0)
            {
                var con = _xml.CreateElement(CONF_Connections);
                _xml.DocumentElement.AppendChild(con);
            }

            var nodes = _xml.DocumentElement.GetElementsByTagName(CONF_Connections)[0].SelectNodes(CONF_ConnectionString);
            bool finded = false;
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
                var value = onError();

                if (value != null)
                    Create(AppConfigurationType.ConexionString, name, value, decrypt, comment);

                return value;
            }

            if (decrypt)
            {
                result = _encrypter.Decrypt(result);
            }

            return result;
        }        
    }

    public enum AppConfigurationType
    {
        Parameter,
        ConexionString
    }
}
