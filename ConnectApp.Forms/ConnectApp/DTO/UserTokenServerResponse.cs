using System;
using System.Linq;
using ConnectApp.Entities;
using ConnectApp.Extensions;
using ConnectApp.Text;
using RestSharp;

namespace ConnectApp.DTO
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
                Headers = response.Headers.Where(p => p.Value != null).ToDictionary(p => p.Name, p => p.Value)
            };
        }
    }
}
