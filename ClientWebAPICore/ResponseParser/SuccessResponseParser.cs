using System.Net;
using System.Net.Http;

namespace ClientWebAPICore.ResponseParser
{
    /// <summary>
    /// Read the Http status code and returns an object of type Response of string
    /// </summary>
    /// <typeparam name="T">The expected data type</typeparam>
    public class SuccessResponseParser : IResponseParser<string>
    {
        /// <summary>
        /// Read the Http status code and returns an object of type Response of string
        /// </summary>
        /// <param name="apiResponse">The HttpResponseMessage object</param>
        /// <returns>The response as an object of type Response of string </returns>
        public Response<string> ParseResponse(HttpResponseMessage apiResponse)
        {
            Response<string> responose = new Response<string>();
            if (apiResponse.IsSuccessStatusCode)
            {
                responose.Success = true;
                responose.Data = "Operation successfully done";
            }
            else
            {
                responose.Error = new Error()
                {
                    ErrorCode = ((int)apiResponse.StatusCode).ToString(),
                    ErrorDescription = apiResponse.ReasonPhrase
                };
            }
            return responose;
        }
    }
}
