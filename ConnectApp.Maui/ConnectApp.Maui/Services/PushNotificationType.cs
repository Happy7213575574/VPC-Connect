using System;
using System.Runtime.Serialization;

namespace ConnectApp.Maui.Services
{
    public enum PushNotificationType
    {
        [EnumMember(Value = "TOKENS")]
        Tokens,

        [EnumMember(Value = "TOPIC")]
        Topic
    }
}

