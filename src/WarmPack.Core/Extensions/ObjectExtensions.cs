using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace WarmPack.Extensions
{
    //       Autor: L.I. Eden Verdugo
    //       Fecha: 2018/08/13    
    // Comentarios: Le hice ligeras modificaciones a la extensión ToXml y corregí algunos errores que verifique que salian al tratar de serializar un objeto anonimo.
    //              * Se agrega la opcion de serializar las propiedades como atributos o elementos del xml.
    public static class ObjectExtensions
    {
        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }

        public static string SerializeToXml(this object obj)
        {
            var type = obj.GetType();

            string xml = "";
            var serializer = new XmlSerializer(type);

            using (StringWriter writer = new Utf8StringWriter())
            {
                serializer.Serialize(writer, obj);
                xml = writer.ToString();
            }

            return xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n", "");
        }
        
        public static List<T> ToList<T>(this object obj)
        {
            return obj as List<T>;
        }

        public static T To<T>(this object obj) where T : IConvertible
        {
            return (T)Convert.ChangeType(obj, typeof(T));
        }

        [Obsolete("Metodo obsoleto dar preferencia al metodo SerializeToXml.")]
        public static string ToXml2(this object obj)
        {
            return ToXml(obj);

        }
        /// <summary>
        /// Obtiene un string que representa un xml de un objeto.
        /// </summary>
        /// <param name="obj">Objeto a serializar a xml</param>
        /// <returns>Regresa un string que representa el contenido de un xml</returns>
        [Obsolete("Metodo obsoleto dar preferencia al metodo SerializeToXml.")]
        public static string ToXml(this object obj)
        {
            return ToXml(obj, string.Empty, ObjectPropertiesAs.Attributes);
        }
        [Obsolete("Metodo obsoleto dar preferencia al metodo SerializeToXml.")]
        public static string ToXml(this object obj, ObjectPropertiesAs propertiesAs)
        {
            return ToXml(obj, string.Empty, propertiesAs);
        }
        [Obsolete("Metodo obsoleto dar preferencia al metodo SerializeToXml.")]
        public static string ToXml(this object obj, string rootName)
        {
            return ToXml(obj, rootName, ObjectPropertiesAs.Attributes);
        }

        //public static string ToXml(this object obj, string rootName, ObjectPropertiesAs propertiesAs)
        //{
        //    return "";
        //}

        [Obsolete("Metodo obsoleto dar preferencia al metodo SerializeToXml.")]
        public static string ToXml(this object obj, string rootName, ObjectPropertiesAs propertiesAs)
        {
            try
            {
                string dateFormat = "yyyyMMdd HH:mm:ss";
                string resultado = string.Empty;

                if (obj != null)
                {
                    var tipo = obj.GetType();

                    // obtener un nombre representable en xml
                    rootName = System.Xml.XmlConvert.EncodeName(rootName != string.Empty ? rootName : tipo.Name);

                    // crear el nodo raiz
                    XElement e = new XElement(rootName);

                    // obtener la lista de propiedades
                    var lstPropiedades = tipo.GetProperties();

                    // verificar si pertenece a un tipo primitivo (int, byte, float, char, etc..) o si es un string, decimal o datetime
                    if (tipo.IsPrimitive || tipo == typeof(string) || tipo == typeof(decimal) || tipo == typeof(DateTime))
                    {
                        if (propertiesAs == ObjectPropertiesAs.Attributes)
                        {
                            // si es un datetime aplicarle el formato para datetime
                            e.SetAttributeValue("value", tipo == typeof(DateTime) ? Convert.ToDateTime(obj).ToString(dateFormat) : obj);
                        }
                        else
                        {
                            var el = new XElement("value");
                            el.SetValue(tipo == typeof(DateTime) ? Convert.ToDateTime(obj).ToString(dateFormat) : obj);

                            e.SetValue(el);
                        }

                    }
                    // verificar si es una lista
                    else if (obj is IList && tipo.IsGenericType && tipo.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
                    {
                        IList lst = (IList)obj;

                        if (lst != null)
                        {
                            // verificar si la lista que se esta leyendo es un arreglo de bytes (por si es una imagen o buffer de algo)
                            if (lst.Count > 0 && lst[0].GetType() == typeof(byte))
                            {
                                if (propertiesAs == ObjectPropertiesAs.Attributes)
                                {
                                    // convertirlo a base64string
                                    e.SetAttributeValue("base64String", Convert.ToBase64String((byte[])lst));
                                }
                                else
                                {
                                    var el = new XElement("base64String");
                                    el.SetValue(Convert.ToBase64String((byte[])lst));
                                    e.SetValue(el);
                                }
                            }
                            else
                            {
                                // convertir cada elemento de la lista a un xml
                                foreach (var item in lst)
                                {
                                    e.Add(XElement.Parse(item.ToXml(string.Empty, propertiesAs)));
                                }
                            }
                        }
                    }
                    // verificar si obj es una instancia de una clase
                    // no usar recursividad porque se cicla de manera infinita con las propiedades "runtime"
                    else
                    {
                        // leer cada propiedad
                        foreach (var propiedad in lstPropiedades)
                        {
                            // obtener el valor de la propiedad
                            object property = propiedad.GetValue(obj, null);

                            // verificar si la propiedad no es una lista                        
                            if (property is IList && propiedad.PropertyType.IsGenericType && propiedad.PropertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
                            {
                                // asignar un type IList
                                var lst = (IList)property;

                                // crear el elemento con el nombre de la propiedad
                                XElement elemento = new XElement(System.Xml.XmlConvert.EncodeName(propiedad.Name));

                                if (lst != null)
                                {
                                    // Verificar si la propiedad no es una lista de bytes
                                    if (lst.Count > 0 && lst[0].GetType() == typeof(byte))
                                    {
                                        if (propertiesAs == ObjectPropertiesAs.Attributes)
                                        {
                                            elemento.SetAttributeValue("base64String", Convert.ToBase64String((byte[])lst));
                                        }
                                        else
                                        {
                                            var el = new XElement("base64String");
                                            el.SetValue(Convert.ToBase64String((byte[])lst));

                                            elemento.SetValue(el);
                                        }
                                    }
                                    else
                                    {
                                        // convertir cada elemento de la lista a un xml
                                        foreach (var item in lst)
                                        {
                                            elemento.Add(XElement.Parse(item.ToXml(string.Empty, propertiesAs)));
                                        }
                                    }
                                }
                                // añadir el elemento
                                e.Add(elemento);
                            }
                            // si la propiedad es un objeto leer sus propiedades
                            else if (propiedad.PropertyType.IsClass && (propiedad.PropertyType != typeof(String) & propiedad.PropertyType != typeof(decimal) & propiedad.PropertyType != typeof(DateTime)))
                            {

                                string valor = property.ToXml();

                                // crear el elemento
                                XElement elemento = XElement.Parse(valor != string.Empty ? valor : new XElement(System.Xml.XmlConvert.EncodeName(propiedad.Name)).ToString());

                                e.Add(elemento);
                            }
                            else
                            {
                                if (propertiesAs == ObjectPropertiesAs.Attributes)
                                {
                                    // verificar si la propiedad es una fecha
                                    if (propiedad.PropertyType == typeof(DateTime))
                                    {
                                        e.SetAttributeValue(propiedad.Name.ToCamelCase(), Convert.ToDateTime(propiedad.GetValue(obj, null)).ToString(dateFormat));
                                    }
                                    else
                                    {
                                        e.SetAttributeValue(propiedad.Name.ToCamelCase(), propiedad.GetValue(obj, null));
                                    }
                                }
                                else
                                {
                                    var el = new XElement(propiedad.Name);

                                    if (propiedad.PropertyType == typeof(DateTime))
                                    {
                                        el.SetValue(Convert.ToDateTime(propiedad.GetValue(obj, null)).ToString(dateFormat));
                                    }
                                    else
                                    {
                                        el.SetValue(propiedad.GetValue(obj, null));
                                    }

                                    e.Add(el);
                                }
                            }
                        }
                    }


                    resultado = e.ToString();
                }

                return resultado;

            }
            catch (Exception ex)
            {
                throw new Exception("No se puede serializar el objeto, vea la excepcion interna para mas información.\n\nExcepcion: " + ex.Message, ex);
            }

        }

        /*
        public static string ToJson(this object obj)
        {
            return ""; //JsonConvert.SerializeObject(obj);
        }

        
        public static string ToJson(this object obj)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                string dateFormat = "yyyyMMdd HH:mm:ss";
                string resultado = string.Empty;

                if (obj != null)
                {
                    var tipo = obj.GetType();

                    // obtener un nombre representable en xml
                    builder.AppendLine("{ ");

                    // obtener la lista de propiedades
                    var lstPropiedades = tipo.GetProperties();

                    // verificar si pertenece a un tipo primitivo (int, byte, float, char, etc..) o si es un string, decimal o datetime
                    if (tipo.IsPrimitive || tipo == typeof(string) || tipo == typeof(decimal) || tipo == typeof(DateTime))
                    {
                        builder.AppendLine($" 'value' : '{ (tipo == typeof(DateTime) ? Convert.ToDateTime(obj).ToString(dateFormat) : obj) }' ");                                                

                    }
                    // verificar si es una lista
                    else if (obj is IList && tipo.IsGenericType && tipo.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
                    {
                        IList lst = (IList)obj;

                        if (lst != null)
                        {
                            // verificar si la lista que se esta leyendo es un arreglo de bytes (por si es una imagen o buffer de algo)
                            if (lst.Count > 0 && lst[0].GetType() == typeof(byte))
                            {
                                builder.AppendLine($" 'base64String' : '{ Convert.ToBase64String((byte[])lst) }' ");
                                                                
                            }
                            else
                            {
                                // convertir cada elemento de la lista a un xml
                                foreach (var item in lst)
                                {
                                    builder.Append(item.ToJson());
                                }
                            }
                        }
                    }
                    // verificar si obj es una instancia de una clase
                    // no usar recursividad porque se cicla de manera infinita con las propiedades "runtime"
                    else
                    {
                        // leer cada propiedad
                        foreach (var propiedad in lstPropiedades)
                        {
                            // obtener el valor de la propiedad
                            object property = propiedad.GetValue(obj, null);

                            // verificar si la propiedad no es una lista                        
                            if (property is IList && propiedad.PropertyType.IsGenericType && propiedad.PropertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)))
                            {
                                // asignar un type IList
                                var lst = (IList)property;

                                // crear el elemento con el nombre de la propiedad
                                //XElement elemento = new XElement(System.Xml.XmlConvert.EncodeName(propiedad.Name));                                

                                if (lst != null)
                                {
                                    // Verificar si la propiedad no es una lista de bytes
                                    if (lst.Count > 0 && lst[0].GetType() == typeof(byte))
                                    {
                                        builder.AppendLine($"  '{ propiedad.Name }' : '{ Convert.ToBase64String((byte[])lst) }'");
                                    }
                                    else
                                    {
                                        // convertir cada elemento de la lista a un xml
                                        foreach (var item in lst)
                                        {                                            
                                            builder.AppendLine(item.ToJson());
                                        }
                                    }
                                }
                                
                            }
                            // si la propiedad es un objeto leer sus propiedades
                            else if (propiedad.PropertyType.IsClass && (propiedad.PropertyType != typeof(String) & propiedad.PropertyType != typeof(decimal) & propiedad.PropertyType != typeof(DateTime)))
                            {

                                string valor = property.ToXml();                                                                

                                builder.AppendLine("  " + System.Xml.XmlConvert.EncodeName(propiedad.Name) + " : { ");
                            }
                            else
                            {


                                if (propertiesAs == ObjectPropertiesAs.Attributes)
                                {
                                    // verificar si la propiedad es una fecha
                                    if (propiedad.PropertyType == typeof(DateTime))
                                    {
                                        e.SetAttributeValue(propiedad.Name.ToCamelCase(), Convert.ToDateTime(propiedad.GetValue(obj, null)).ToString(dateFormat));
                                    }
                                    else
                                    {
                                        e.SetAttributeValue(propiedad.Name.ToCamelCase(), propiedad.GetValue(obj, null));
                                    }
                                }
                                else
                                {
                                    var el = new XElement(propiedad.Name);

                                    if (propiedad.PropertyType == typeof(DateTime))
                                    {
                                        el.SetValue(Convert.ToDateTime(propiedad.GetValue(obj, null)).ToString(dateFormat));
                                    }
                                    else
                                    {
                                        el.SetValue(propiedad.GetValue(obj, null));
                                    }

                                    e.Add(el);
                                }
                            }
                        }
                    }


                    resultado = e.ToString();
                }

                return resultado;

            }
            catch (Exception ex)
            {
                throw new Exception("No se puede serializar el objeto, vea la excepcion interna para mas información.\n\nExcepcion: " + ex.Message, ex);
            }

        } */

        public static T Clone<T>(this object obj)
        {
            T copy = default(T);
            try
            {
                copy = Activator.CreateInstance<T>();

                foreach (var prop in copy.GetType().GetProperties())
                {
                    prop.SetValue(copy, obj.GetType().GetProperty(prop.Name).GetValue(obj, null), null);
                }

                foreach (var field in copy.GetType().GetFields())
                {
                    field.SetValue(copy, obj.GetType().GetField(field.Name).GetValue(obj));
                }

            }
            catch { }

            return copy;
        }
    }

    // enumeración para representar las propiedades como atributos o elementos
    public enum ObjectPropertiesAs
    {
        Attributes,
        Elements
    }

}
