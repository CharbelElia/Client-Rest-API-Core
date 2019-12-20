using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ClientWebAPICore.ResponseParser
{
    /// <summary>
    /// Read the Http response content as Xml and parses it to Response of T
    /// </summary>
    /// <typeparam name="T">The expected data type</typeparam>
    public class XmlResponseParser<T> : IResponseParser<T>
    {
        /// <summary>
        /// Read the Http response content as Xml and parses it to Response of T
        /// </summary>
        /// <param name="apiResponse">The HttpResponseMessage object</param>
        /// <returns>The response as an object of type Response of T </returns>
        public Response<T> ParseResponse(HttpResponseMessage apiResponse)
        {
            Response<T> response = new Response<T>();
            if (!apiResponse.IsSuccessStatusCode)
            {
                response.Error = new Error()
                {
                    ErrorCode = apiResponse.StatusCode.ToString(),
                    ErrorDescription = apiResponse.StatusCode.ToString()
                };
            }
            else
            {
                try
                {
                    string content = apiResponse.Content.ReadAsStringAsync().Result;
                    T xml = Helper.DeserializeXml<T>(content);
                    response.Success = true;
                    response.Data = xml;
                }
                catch (Exception ex)
                {
                    response.Error = new Error()
                    {
                        ErrorCode = apiResponse.StatusCode.ToString(),
                        ErrorDescription = "Exception occured : " + ex.Message
                    };
                }

            }
            return response;
        }
    }
}
