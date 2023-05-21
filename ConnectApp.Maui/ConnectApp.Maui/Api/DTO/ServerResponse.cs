using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ConnectApp.Maui.Text;

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

        public static ServerResponse From(Exception exception, string reason = null)
        {
            return new ServerResponse()
            {
                IsSuccess = false,
                StatusDescription = ServerResponses.Describe(exception),
                RawContent = exception.Message,
                Code = 0,
                ErrorMessage = reason ?? exception.Message,
                ErrorException = exception,
                Headers = new List<Tuple<string,object>>()
            };
        }

        public static async Task<ServerResponse> FromAsync(HttpResponseMessage response)
        {
            return new ServerResponse()
            {
                IsSuccess = response.IsSuccessStatusCode,
                StatusDescription = ServerResponses.Describe(response.StatusCode),
                RawContent = await response.Content.ReadAsStringAsync(),
                Code = response.StatusCode,
                ErrorMessage = response.ReasonPhrase,
                ErrorException = null,
                Headers = response.Headers?.SelectMany(h => h.Value.Select(v => Tuple.Create(h.Key, (object)v))).ToList()
                    ?? new List<Tuple<string, object>>()
            };
        }

    }
}
