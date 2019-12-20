using System.IO;
using System.Xml.Serialization;

namespace ClientWebAPICore.RequestSerializer
{
    /// <summary>
    /// Serializes the data to Xml using XmlSerializer
    /// </summary>
    public class XmlRequestSerializer : IRequestSerializer
    {
        private bool omitXmlDeclaration;
        private XmlSerializerNamespaces xmlSerializerNamespaces;

        /// <summary>
        /// The construtor of the class
        /// </summary>
        /// <param name="omitXmlDeclaration">The boolean value indicating whether to ommit or not the xml decralation when serilizing the data</param>
        /// <param name="xmlSerializerNamespaces">The xml namespaces to be mentioned when serializing the data</param>
        public XmlRequestSerializer(bool omitXmlDeclaration = true, XmlSerializerNamespaces xmlSerializerNamespaces = null)
        {
            this.omitXmlDeclaration = omitXmlDeclaration;
            this.xmlSerializerNamespaces = xmlSerializerNamespaces;
        }
        /// <summary>
        /// Returns the media type
        /// </summary>
        /// <returns>the media type 'application/xml' as string</returns>
        public string GetMediaType()
        {
            return "application/xml";
        }
        /// <summary>
        /// Serializes the data to Xml
        /// </summary>
        /// <typeparam name="DataType">The type of the data to be serialized to Xml</typeparam>
        /// <param name="data">The data to be serialized to Xml</param>
        /// <returns>The serialized data as string</returns>
        public string Serialize<DataType>(DataType data)
        {
            return Helper.SerializeToStringXml<DataType>(data, this.omitXmlDeclaration, this.xmlSerializerNamespaces);
        }
    }
}
