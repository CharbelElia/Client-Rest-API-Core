using ClientWebAPICore.RequestSerializer;
using ClientWebAPICore.ResponseParser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ClientWebAPICore
{
    /// <summary>
    /// This class is responsible of sending requests to a Rest API
    /// </summary>
    public class ClientRestApi
    {
        private class MyHttpClient
        {
            public HttpClient HttpClient { get; set; }
            public HttpContent ContentData { get; set; }
        }
        private MyHttpClient CreateMyHttpClient<DataType, ResponseDataType>(string url, DataType data,
            IRequestSerializer requestSerializer,
            IResponseParser<ResponseDataType> responseParser, bool compressData, IDictionary<string, string> header, bool useStringContentWithoutCharset)
        {
            CheckUrl(url);
            HttpClient httpClient = null;
            HttpContent contentData = null;
            string mediaType = null;
            if (requestSerializer != null)
                mediaType = requestSerializer.GetMediaType();
            string serializedData = null;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);
            if (data != null)
            {
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue(mediaType);
                httpClient.DefaultRequestHeaders.Accept.Add(contentType);
                serializedData = requestSerializer.Serialize(data);
                contentData = new StringContent(serializedData, System.Text.Encoding.UTF8, mediaType);
                if (useStringContentWithoutCharset)
                {
                    contentData.Headers.ContentType = contentType;
                }

            }
            AddHeaderToHttpClient(httpClient, header);
            return new MyHttpClient()
            {
                HttpClient = httpClient,
                ContentData = contentData
            };
        }
        /// <summary>
        /// Send a POST request to the specified Url
        /// </summary>
        /// <typeparam name="DataType">The type of the data to be serialized before sending the request</typeparam>
        /// <typeparam name="ResponseDataType">The type of the Data in the Response object</typeparam>
        /// <param name="url">The Url the request is sent to</param>
        /// <param name="data">The data to be sent in the request content</param>
        /// <param name="requestSerializer">The request serializer to serialize the data before setting the request content</param>
        /// <param name="responseParser">The parser of the http response</param>
        /// <param name="compressData"></param>
        /// <param name="header">The http headers</param>
        /// <param name="useStringContentWithoutCharset">The boolean value indicating whether to remove or not the 'charset' from the content type, in order to deal with a server bad implementation</param>
        /// <returns>Response of the given type parameter 'ResponseDataType'</returns>
        public Response<ResponseDataType> Post<DataType, ResponseDataType>(string url, DataType data, IRequestSerializer requestSerializer, IResponseParser<ResponseDataType> responseParser, bool compressData = false, IDictionary<string, string> header = null, bool useStringContentWithoutCharset = false)
        {
            MyHttpClient myClient = CreateMyHttpClient(url, data, requestSerializer, responseParser, compressData, header, useStringContentWithoutCharset);
            HttpResponseMessage response = myClient.HttpClient.PostAsync(url, myClient.ContentData).Result;
            myClient.HttpClient.Dispose();
            return responseParser.ParseResponse(response);
        }
        /// <summary>
        /// Send a POST request to the specified Url as an asynchronous operation
        /// </summary>
        /// <typeparam name="DataType">The type of the data to be serialized before sending the request</typeparam>
        /// <typeparam name="ResponseDataType">The type of the Data in the Response object</typeparam>
        /// <param name="url">The Url the request is sent to</param>
        /// <param name="data">The data to be sent in the request content</param>
        /// <param name="requestSerializer">The request serializer to serialize the data before setting the request content</param>
        /// <param name="responseParser">The parser of the http response</param>
        /// <param name="compressData"></param>
        /// <param name="header">The http headers</param>
        /// <param name="useStringContentWithoutCharset">The boolean value indicating whether to remove or not the 'charset' from the content type, in order to deal with a server bad implementation</param>
        /// <returns>Task of Response of the given type parameter 'ResponseDataType'</returns>
        public async Task<Response<ResponseDataType>> PostAsync<DataType, ResponseDataType>(string url, DataType data, IRequestSerializer requestSerializer, IResponseParser<ResponseDataType> responseParser, bool compressData = false, IDictionary<string, string> header = null, bool useStringContentWithoutCharset = false)
        {
            MyHttpClient myClient = CreateMyHttpClient(url, data, requestSerializer, responseParser, compressData, header, useStringContentWithoutCharset);
            HttpResponseMessage response = await myClient.HttpClient.PostAsync(url, myClient.ContentData);
            myClient.HttpClient.Dispose();
            return responseParser.ParseResponse(response);
        }
        /// <summary>
        /// Send a POST request to the specified Url without body content
        /// </summary>
        /// <typeparam name="ResponseDataType">The type of the Data in the Response object</typeparam>
        /// <param name="url">The Url the request is sent to</param>
        /// <param name="responseParser">The parser of the http response</param>
        /// <param name="header">The http headers</param>
        /// <returns>Response of the given type parameter 'ResponseDataType'</returns>
        public Response<ResponseDataType> Post<ResponseDataType>(string url, IResponseParser<ResponseDataType> responseParser, IDictionary<string, string> header = null)
        {
            CheckUrl(url);
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                AddHeaderToHttpClient(httpClient, header);
                HttpResponseMessage response = httpClient.PostAsync(url, null).Result;
                return responseParser.ParseResponse(response);
            }
        }
        /// <summary>
        /// Send a POST request to the specified Url as an asynchronous operation without body content
        /// </summary>
        /// <typeparam name="ResponseDataType">The type of the Data in the Response object</typeparam>
        /// <param name="url">The Url the request is sent to</param>
        /// <param name="responseParser">The parser of the http response</param>
        /// <param name="header">The http headers</param>
        /// <returns>Task of Response of the given type parameter 'ResponseDataType'</returns>
        public async Task<Response<ResponseDataType>> PostAsync<ResponseDataType>(string url, IResponseParser<ResponseDataType> responseParser, IDictionary<string, string> header = null)
        {
            CheckUrl(url);
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                AddHeaderToHttpClient(httpClient, header);
                HttpResponseMessage response = await httpClient.PostAsync(url, null);
                return responseParser.ParseResponse(response);
            }
        }
        /// <summary>
        /// Send a DELETE request to the specified Url
        /// </summary>
        /// <typeparam name="ResponseDataType">The type of the Data in the Response object</typeparam>
        /// <param name="url">The Url the request is sent to</param>
        /// <param name="responseParser">The parser of the http response</param>
        /// <param name="header">The http headers</param>
        /// <returns>Response of the given type parameter 'ResponseDataType'</returns>
        public Response<ResponseDataType> Delete<ResponseDataType>(string url, IResponseParser<ResponseDataType> responseParser, IDictionary<string, string> header = null)
        {
            CheckUrl(url);
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                AddHeaderToHttpClient(httpClient, header);
                HttpResponseMessage response = httpClient.DeleteAsync(url).Result;
                return responseParser.ParseResponse(response);
            }
        }
        /// <summary>
        /// Send a DELETE request to the specified Url as an asynchronous operation
        /// </summary>
        /// <typeparam name="ResponseDataType">The type of the Data in the Response object</typeparam>
        /// <param name="url">The Url the request is sent to</param>
        /// <param name="responseParser">The parser of the http response</param>
        /// <param name="header">The http headers</param>
        /// <returns>Task of Response of the given type parameter 'ResponseDataType'</returns>
        public async Task<Response<ResponseDataType>> DeleteAsync<ResponseDataType>(string url, IResponseParser<ResponseDataType> responseParser, IDictionary<string, string> header = null)
        {
            CheckUrl(url);
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                AddHeaderToHttpClient(httpClient, header);
                HttpResponseMessage response = await httpClient.DeleteAsync(url);
                return responseParser.ParseResponse(response);
            }
        }
        /// <summary>
        /// Send a GET request to the specified Url
        /// </summary>
        /// <typeparam name="ResponseDataType">The type of the Data in the Response object</typeparam>
        /// <param name="url">The Url the request is sent to</param>
        /// <param name="responseParser">The parser of the http response</param>
        /// <param name="header">The http headers</param>
        /// <returns>Response of the given type parameter 'ResponseDataType'</returns>
        public Response<ResponseDataType> Get<ResponseDataType>(string url, IResponseParser<ResponseDataType> responseParser, IDictionary<string, string> header = null)
        {
            CheckUrl(url);
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                AddHeaderToHttpClient(httpClient, header);
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                return responseParser.ParseResponse(response);
            }
        }
        /// <summary>
        /// Send a GET request to the specified Url as an asynchronous operation
        /// </summary>
        /// <typeparam name="ResponseDataType">The type of the Data in the Response object</typeparam>
        /// <param name="url">The Url the request is sent to</param>
        /// <param name="responseParser">The parser of the http response</param>
        /// <param name="header">The http headers</param>
        /// <returns>Task of Response of the given type parameter 'ResponseDataType'</returns>
        public async Task<Response<ResponseDataType>> GetAsync<ResponseDataType>(string url, IResponseParser<ResponseDataType> responseParser, IDictionary<string, string> header = null)
        {
            CheckUrl(url);
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                AddHeaderToHttpClient(httpClient, header);
                HttpResponseMessage response = await httpClient.GetAsync(url);
                return responseParser.ParseResponse(response);
            }
        }
        /// <summary>
        /// Send a PUT request to the specified Url
        /// </summary>
        /// <typeparam name="DataType">The type of the data to be serialized before sending the request</typeparam>
        /// <typeparam name="ResponseDataType">The type of the Data in the Response object</typeparam>
        /// <param name="url">The Url the request is sent to</param>
        /// <param name="data">The data to be sent in the request content</param>
        /// <param name="requestSerializer">The request serializer to serialize the data before setting the request content</param>
        /// <param name="responseParser">The parser of the http response</param>
        /// <param name="header">The http headers</param>
        /// <param name="useStringContentWithoutCharset">The boolean value indicating whether to remove or not the 'charset' from the content type, in order to deal with a server bad implementation</param>
        /// <returns>Response of the given type parameter 'ResponseDataType'</returns>
        public Response<ResponseDataType> Put<DataType, ResponseDataType>(string url, DataType data, IRequestSerializer requestSerializer, IResponseParser<ResponseDataType> responseParser, IDictionary<string, string> header = null, bool useStringContentWithoutCharset = false)
        {
            MyHttpClient myClient = CreateMyHttpClient(url, data, requestSerializer, responseParser, false, header, useStringContentWithoutCharset);
            HttpResponseMessage response = myClient.HttpClient.PutAsync(url, myClient.ContentData).Result;
            myClient.HttpClient.Dispose();
            return responseParser.ParseResponse(response);
        }
        /// <summary>
        /// Send a PUT request to the specified Url as an asynchronous operation
        /// </summary>
        /// <typeparam name="DataType">The type of the data to be serialized before sending the request</typeparam>
        /// <typeparam name="ResponseDataType">The type of the Data in the Response object</typeparam>
        /// <param name="url">The Url the request is sent to</param>
        /// <param name="data">The data to be sent in the request content</param>
        /// <param name="requestSerializer">The request serializer to serialize the data before setting the request content</param>
        /// <param name="responseParser">The parser of the http response</param>
        /// <param name="header">The http headers</param>
        /// <param name="useStringContentWithoutCharset">The boolean value indicating whether to remove or not the 'charset' from the content type, in order to deal with a server bad implementation</param>
        /// <returns>Task of Response of the given type parameter 'ResponseDataType'</returns>
        public async Task<Response<ResponseDataType>> PutAsync<DataType, ResponseDataType>(string url, DataType data, IRequestSerializer requestSerializer, IResponseParser<ResponseDataType> responseParser, IDictionary<string, string> header = null, bool useStringContentWithoutCharset = false)
        {
            MyHttpClient myClient = CreateMyHttpClient(url, data, requestSerializer, responseParser, false, header, useStringContentWithoutCharset);
            HttpResponseMessage response = await myClient.HttpClient.PutAsync(url, myClient.ContentData);
            myClient.HttpClient.Dispose();
            return responseParser.ParseResponse(response);
        }

        private void AddHeaderToHttpClient(HttpClient httpClient, IDictionary<string, string> header)
        {
            if (header != null)
            {
                foreach (KeyValuePair<string, string> entry in header)
                {
                    httpClient.DefaultRequestHeaders.Add(entry.Key, entry.Value);
                }
            }
        }
        private void CheckUrl(string url)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url) || IsInvalidUrl(url))
                throw new ArgumentException("Invalid Url format", "url");
            if (url.StartsWith("https"))
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }
        private bool IsInvalidUrl(string url)
        {
            Uri uriResult;
            bool valid = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return !valid;
        }
    }
}
