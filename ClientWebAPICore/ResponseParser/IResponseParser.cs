
using System.Net.Http;

namespace ClientWebAPICore.ResponseParser
{
    /// <summary>
    /// Parses the HttpResponseMessage and returns an object of type Response of T
    /// </summary>
    public interface IResponseParser<T>
    {
        /// <summary>
        /// Parses the HttpResponseMessage and returns an object of type Response of T
        /// </summary>
        /// <param name="apiResponse">The HttpResponseMessage object</param>
        /// <returns>The response as an object of type Response of T </returns>
        Response<T> ParseResponse(HttpResponseMessage apiResponse);
    }
}
