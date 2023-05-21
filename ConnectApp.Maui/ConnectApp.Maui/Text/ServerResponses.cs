using System;
using System.Net;

namespace ConnectApp.Maui.Text
{
    public class ServerResponses
    {
        public static string Describe(Exception exception)
        {
            return $"Unexpected error: {exception.GetType().ToString()}";
        }

        public static string Describe(HttpStatusCode code)
        {
            switch ((int)code)
            {
                case (int)HttpStatusCode.Unauthorized:
                    return "Username or password not recognised.";

                case (int)HttpStatusCode.BadRequest:
                    return "Too many incorrect attempts.";

                case (int)HttpStatusCode.InternalServerError:
                    return "Unexpected portal error.";

                case (int)HttpStatusCode.NotFound:
                    return "Not found.";

                case int n when (n >= 200 && n < 300):
                    return "Success.";

                case int n when (n >= 300 && n < 400):
                    return "Server returned a redirection code: " + code;

                case int n when (n >= 400 && n < 500):
                    return "Server rejected this request: " + code;

                case int n when (n >= 500 && n < 600):
                    return "Server error: " + code;

                default:
                    return "Unrecognised server response: " + code;
            }
        }


    }
}
