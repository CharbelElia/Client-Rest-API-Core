using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWebAPICore.RequestSerializer
{
    /// <summary>
    /// This attribute is allowed on properties only.
    /// When you decorate a property with this attribute, you can give the property the name to be considered by the KeyValuePairSerializer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SerializerName : Attribute
    {
        public string Name { get; private set; }
        public SerializerName(string name)
        {
            this.Name = name;
        }
    }
}
