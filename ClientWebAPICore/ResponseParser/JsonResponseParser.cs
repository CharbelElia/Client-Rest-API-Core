using System;
using System.Net.Http;

namespace ClientWebAPICore.ResponseParser
{
    /// <summary>
    /// Read the Http response content as Json and parses it to Response of T
    /// </summary>
    /// <typeparam name="T">The expected data type</typeparam>
    public class JsonResponseParser<T> : IResponseParser<T>
    {
        //public Response<T> ParseResponse(HttpResponseMessage apiResponse)
        //{
        //    Response<T> resp = new Response<T>();
        //    if (!apiResponse.IsSuccessStatusCode)
        //    {
        //        resp.Error = new Error()
        //        {
        //            ErrorCode = apiResponse.StatusCode.ToString(),
        //            ErrorDescription = apiResponse.StatusCode.ToString()
        //        };
        //    }
        //    else
        //    {
        //        try
        //        {
        //            string content = apiResponse.Content.ReadAsStringAsync().Result;
        //            if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
        //            {
        //                resp.Error = new Error()
        //                {
        //                    ErrorCode = ((int)apiResponse.StatusCode).ToString(),
        //                    ErrorDescription = apiResponse.StatusCode.ToString()
        //                };
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    resp.Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
        //                    resp.Success = true;
        //                }
        //                catch(Exception ex)
        //                {
        //                    resp.Error = new Error()
        //                    {
        //                        ErrorCode = "JsonDeserializationError",
        //                        ErrorDescription = "Exception occured : " + ex.Message
        //                    };
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            resp.Error = new Error()
        //            {
        //                ErrorCode = apiResponse.StatusCode.ToString(),
        //                ErrorDescription = "Exception occured : " + ex.Message
        //            };
        //        }

        //    }
        //    return resp;
        //}
        /// <summary>
        /// Read the Http response content as Json and parses it to Response of T
        /// </summary>
        /// <param name="apiResponse">The HttpResponseMessage object</param>
        /// <returns>The response as an object of type Response of T </returns>
        public Response<T> ParseResponse(HttpResponseMessage apiResponse)
        {
            Response<T> resp = new Response<T>();
            string content = (apiResponse.Content!=null) ? apiResponse.Content.ReadAsStringAsync().Result :string.Empty;
            if (!apiResponse.IsSuccessStatusCode)
            {
                resp.Error = new Error()
                {
                    ErrorCode = apiResponse.StatusCode.ToString(),
                    ErrorDescription = (string.IsNullOrWhiteSpace(content)) ? apiResponse.StatusCode.ToString() : content
                };
            }
            else
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        resp.Error = new Error()
                        {
                            ErrorCode = ((int)apiResponse.StatusCode).ToString(),
                            ErrorDescription = apiResponse.StatusCode.ToString()
                        };
                    }
                    else
                    {
                        try
                        {
                            resp.Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
                            resp.Success = true;
                        }
                        catch (Exception ex)
                        {
                            resp.Error = new Error()
                            {
                                ErrorCode = "JsonDeserializationError",
                                ErrorDescription = "Exception occured : " + ex.Message
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    resp.Error = new Error()
                    {
                        ErrorCode = apiResponse.StatusCode.ToString(),
                        ErrorDescription = "Exception occured : " + ex.Message
                    };
                }

            }
            return resp;
        }
    }
}
