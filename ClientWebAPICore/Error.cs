using System;

namespace ClientWebAPICore
{
    /// <summary>
    /// The model representing an error
    /// </summary>
    public class Error
    {
        /// <summary>
        /// The error code
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// The error description
        /// </summary>
        public string ErrorDescription { get; set; }
    }
}
