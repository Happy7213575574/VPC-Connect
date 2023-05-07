using System;
using System.Linq;
using ConnectApp.Maui.Data.Entities;
using ConnectApp.Maui.Extensions;
using ConnectApp.Maui.Text;
using RestSharp;

namespace ConnectApp.Maui.Api.DTO
{
    public class UserTokenServerResponse : ServerResponse
    {
        public string UserToken => IsSuccess ? RawContent.Trim(new[] { '"', ' ' }).Truncate(PushConfiguration.MAX_USER_TOKEN) : null;

        public static new UserTokenServerResponse From(RestResponse response)
        {
            return new UserTokenServerResponse()
            {
                IsSuccess = response.IsSuccessful,
                StatusDescription = ServerResponses.Describe(response.StatusCode),
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
