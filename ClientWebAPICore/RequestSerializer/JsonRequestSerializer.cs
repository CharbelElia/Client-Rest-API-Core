namespace ClientWebAPICore.RequestSerializer
{
    /// <summary>
    /// Serializes the data to Json using Newtonsoft
    /// </summary>
    public class JsonRequestSerializer : IRequestSerializer
    {
        /// <summary>
        /// Returns the media type
        /// </summary>
        /// <returns>the media type 'application/json' as string</returns>
        public string GetMediaType()
        {
            return "application/json";
        }
        /// <summary>
        /// Serializes the data to Json
        /// </summary>
        /// <typeparam name="DataType">The type of the data to be serialized to Json</typeparam>
        /// <param name="data">The data to be serialized to Json</param>
        /// <returns>The serialized data as string</returns>
        public string Serialize<DataType>(DataType data)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }
    }
}
