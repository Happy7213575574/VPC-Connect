using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Firebase.CloudMessaging;

namespace ConnectApp.Maui.Services
{
    public interface IPushNotificationService
    {
        Task CheckIfValidAsync();
        Task<string> GetFcmTokenAsync();
        Task SubscribeToTopicAsync(string topic);
        Task UnsubscribeFromTopicAsync(string topic);
    }
}

