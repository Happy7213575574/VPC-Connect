using Plugin.Firebase.CloudMessaging;
using Plugin.Firebase.Functions;

namespace ConnectApp.Maui.Services
{
    public sealed class PushNotificationService : IPushNotificationService
    {
        private readonly IFirebaseCloudMessaging _firebaseCloudMessaging;
        private readonly IFirebaseFunctions _firebaseFunctions;

        public PushNotificationService(
            IFirebaseCloudMessaging firebaseCloudMessaging,
            IFirebaseFunctions firebaseFunctions)
        {
            _firebaseCloudMessaging = firebaseCloudMessaging;
            _firebaseFunctions = firebaseFunctions;
        }

        public Task CheckIfValidAsync()
        {
            return _firebaseCloudMessaging.CheckIfValidAsync();
        }

        public Task<string> GetFcmTokenAsync()
        {
            return _firebaseCloudMessaging.GetTokenAsync();
        }

        public Task SubscribeToTopicAsync(string topic)
        {
            return _firebaseCloudMessaging.SubscribeToTopicAsync(topic);
        }

        public Task UnsubscribeFromTopicAsync(string topic)
        {
            return _firebaseCloudMessaging.UnsubscribeFromTopicAsync(topic);
        }

    }
}

