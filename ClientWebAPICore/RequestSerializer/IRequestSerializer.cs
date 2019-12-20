using System;

namespace ClientWebAPICore.RequestSerializer
{
    /// <summary>
    /// Serializes data to string and returns the corresponding media type.
    /// </summary>
    public interface IRequestSerializer
    {
        //ICloneable
        /// <summary>
        /// Serializes the data to string
        /// </summary>
        /// <typeparam name="DataType">The type of the data to be serialized</typeparam>
        /// <param name="data">The data to be serialized</param>
        /// <returns>The serialized data as string</returns>
        string Serialize<DataType>(DataType data);
        /// <summary>
        /// Returns the media type
        /// </summary>
        /// <returns>the media type of the http request content as string</returns>
        string GetMediaType();
    }
}
