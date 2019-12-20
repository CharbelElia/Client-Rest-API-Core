using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ClientWebAPICore.RequestSerializer
{
    /// <summary>
    /// This class is responsible of serializing the request’s content to a string of key value pairs seperated by semicolon.
    /// </summary>
    public class KeyValuePairRequestSerializer : IRequestSerializer
    {
        /// <summary>
        /// Serializes the data to a string of key value pairs seperated by semicolon
        /// </summary>
        /// <typeparam name="DataType">The type of the data to be serialized to string of key value pairs seperated by semicolon</typeparam>
        /// <param name="data">The data to be serialized to string of key value pairs seperated by semicolon</param>
        /// <returns>The serialized data as string</returns>
        public string Serialize<DataType>(DataType data)
        {
            StringBuilder serializedData = new StringBuilder();
            List<PropertyInfo> properties = typeof(DataType).GetProperties().ToList();
            var serializableProperties = (from property in properties where property.IsDefined(typeof(Serializable)) select property).ToList();
            //In case there is at least one property decorated with Serializable attribute then consider only the properties having this attribute
            //otherwise take all properties
            //This is only intended to make it easier for developer creating the entity. If the entity has 20 properties and only 2 are needed to 
            //be serialized then instead of setting SerializeIgnore attribute on 18 properties, you can set Serializable attribute to two properties
            if (serializableProperties.Count > 0)
                properties = serializableProperties;
            var unSerializableProperties = (from property in properties where property.IsDefined(typeof(SerializerIgnore)) select property).ToList();
            if (unSerializableProperties.Count > 0)
                properties = properties.Except(unSerializableProperties).ToList();
            string propertySerializerName;
            foreach (PropertyInfo property in properties)
            {
                propertySerializerName = string.Empty;
                SerializerName serializerName = property.GetCustomAttribute<SerializerName>();
                propertySerializerName = (serializerName != null) ? serializerName.Name : string.Empty;
                if (property.PropertyType.IsGenericType &&
                        property.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    IList values = property.GetValue(data) as IList;
                    foreach (object value in values)
                    {
                        serializedData.AppendUrlEncoded((string.IsNullOrWhiteSpace(propertySerializerName))?property.Name : propertySerializerName, value.ToString());
                    }
                }
                else
                {
                    serializedData.AppendUrlEncoded((string.IsNullOrWhiteSpace(propertySerializerName)) ? property.Name : propertySerializerName, property.GetValue(data).ToString());
                }
            }
            return serializedData.ToString();
        }
        /// <summary>
        /// Returns the media type
        /// </summary>
        /// <returns>the media type 'application/x-www-form-urlencoded' as string</returns>
        public string GetMediaType()
        {
            return "application/x-www-form-urlencoded";
        }
    }
}
