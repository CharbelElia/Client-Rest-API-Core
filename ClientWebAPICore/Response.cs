using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWebAPICore
{
    /// <summary>
    /// This is a template class to represent the response of an http call. 
    /// It contains data of type T (the type parameter of the class), an error object of type Error, and a boolean value indicating whether the request was successful or not
    /// </summary>
    /// <typeparam name="T">The type of the Data property</typeparam>
    public class Response<T>
    {
        /// <summary>
        /// The data as an object of type T
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// The error as an object of type Error
        /// </summary>
        public Error Error { get; set; }
        /// <summary>
        /// A boolean value indicating whether the request was successful or not
        /// </summary>
        public bool Success { get; set; }
    }
}
