using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ConnectApp.Maui.Text;
using RestSharp;

namespace ConnectApp.Maui.Api.DTO
{
    public class ServerResponse
    {
        public bool IsSuccess { get; set; }
        public string StatusDescription { get; set; }
        public string RawContent { get; set; }
        public HttpStatusCode Code { get; set; }
        public string ErrorMessage { get; set; }
        public Exception ErrorException { get; set; }
        public List<Tuple<string,object>> Headers { get; set; }

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
                Headers = new List<Tuple<string,object>>()
            };

        }

        public static ServerResponse From(RestResponse response)
        {
            return new ServerResponse()
            {
                IsSuccess = response.IsSuccessful,
                StatusDescription = ServerResponses.Describe(response.ResponseStatus),
                RawContent = response.Content,
                Code = response.StatusCode,
                ErrorMessage = response.ErrorMessage,
                ErrorException = response.ErrorException,
                Headers = response.Headers?.Where(p => p.Value != null).Select(h => Tuple.Create(h.Name, h.Value)).ToList()
                    ?? new List<Tuple<string, object>>()
            };
        }
    }
}
