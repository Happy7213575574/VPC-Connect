using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ConnectApp.Text;
using RestSharp;

namespace ConnectApp.DTO
{
    public class ServerResponse
    {
        public bool IsSuccess { get; set; }
        public string StatusDescription { get; set; }
        public string RawContent { get; set; }
        public HttpStatusCode Code { get; set; }
        public string ErrorMessage { get; set; }
        public Exception ErrorException { get; set; }
        public Dictionary<string,object> Headers { get; set; }

        public static ServerResponse From(Exception exception)
        {
            return new ServerResponse()
            {
                IsSuccess = false,
                StatusDescription = ServerResponses.Describe(exception),
                RawContent = exception.Message,
                Code = 0,
                ErrorMessage = exception.Message,
                ErrorException = exception,
                Headers = new Dictionary<string,object>()
            };

        }

        public static ServerResponse From(RestResponse response)
        {
            switch (response.ResponseStatus)
            {
                case ResponseStatus.Completed:
                    return new ServerResponse()
                    {
                        IsSuccess = response.IsSuccessful,
                        StatusDescription = ServerResponses.Describe(response.StatusCode),
                        RawContent = response.Content,
                        Code = response.StatusCode,
                        ErrorMessage = response.ErrorMessage,
                        ErrorException = response.ErrorException,
                        Headers = response.Headers.Where(p => p.Value != null).ToDictionary(p => p.Name, p => p.Value)
                    };
                default:
                    return new ServerResponse()
                    {
                        IsSuccess = response.IsSuccessful,
                        StatusDescription = ServerResponses.Describe(response.ResponseStatus),
                        RawContent = response.Content,
                        Code = response.StatusCode,
                        ErrorMessage = response.ErrorMessage,
                        ErrorException = response.ErrorException,
                        Headers = response.Headers.Where(p => p.Value != null).ToDictionary(p => p.Name, p => p.Value)
                    };

            }

        }
    }
}
