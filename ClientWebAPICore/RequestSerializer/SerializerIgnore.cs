using System;

namespace ClientWebAPICore.RequestSerializer
{
    /// <summary>
    /// This attribute is allowed on properties only.
    /// When you decorate a property with this attribute, it means that this property will be ignored by the KeyValuePairSerializer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SerializerIgnore : Attribute
    {

    }
}
