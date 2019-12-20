using System;

namespace ClientWebAPICore.RequestSerializer
{
    /// <summary>
    /// This attribute is allowed on properties only.
    /// When you decorate a property with this attribute, it means that this property will be serialized by the KeyValuePairSerializer.
    /// In case no properties are decorated by this attribute, then all the properties are considered serializable except those decorated by SerializerIgnore attribute.
    /// If at least one property has the Serializable attribute then only properties with this attribute are considered serializable.
    /// This is just intended to make it easier in case only few properties among many are requested to be serialized.
    /// In this case instead of adding SerializerIgnore attribute to many properties, Serializable attribute can be added to few of them.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Serializable : Attribute
    {

    }
}
