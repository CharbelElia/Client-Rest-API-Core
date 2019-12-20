using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ClientWebAPICore
{
    /// <summary>
    /// This class provides usefull methods
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// This static method is used to serialize a data of type T to Xml string
        /// </summary>
        /// <typeparam name="T">The data type of the data to be serialized</typeparam>
        /// <param name="value">The data to be serialized</param>
        /// <param name="omitXmlDeclaration">A boolean value indicating whether to ommit or not the xml declaration</param>
        /// <param name="xmlSerializerNamespaces">Xml namespaces to be mentioned in the serialized Xml string</param>
        /// <returns>Xml string</returns>
        public static string SerializeToStringXml<T>(T value, bool omitXmlDeclaration = true, XmlSerializerNamespaces xmlSerializerNamespaces=null)
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            if (xmlSerializerNamespaces != null)
                emptyNamespaces = xmlSerializerNamespaces;
            var serializer = new XmlSerializer(value.GetType());
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = omitXmlDeclaration;

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, value, emptyNamespaces);
                return stream.ToString();
            }
        }
        /// <summary>
        /// This static method is used to deserialize an Xml string to an object of type T
        /// </summary>
        /// <typeparam name="T">The expected data type of the deserialized object</typeparam>
        /// <param name="xmlStr">The xml string to be deserialized</param>
        /// <returns>The deserialized object of type T</returns>
        public static T DeserializeXml<T>(string xmlStr)
        {
            var serializer = new XmlSerializer(typeof(T));
            T result;
            using (TextReader reader = new StringReader(xmlStr))
            {
                result = (T)serializer.Deserialize(reader);
            }
            return result;
        }
    }
}
