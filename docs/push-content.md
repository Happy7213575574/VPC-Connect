# FCM push notification content

Both Android and iOS distinguish between push notifications that are designed to be automatically visible to users, and those that are passed to applications in the background.

When the application receives a notification, the `CrossFirebasePushNotification.Current.OnMessageReceived` event should be triggered with details of the notification.

## Android push notification types

Android push notifications come in a few modes:

* `Notification` - a push notification displayed to the user.
* `Data` - a push notification that will be delivered to the application in the background.

### Notification messages

Notification messages contain a `notification` section, with `title` and `body` fields that contain information for the user.

When the app is backgrounded, these will be rendered by Firebase Cloud Messaging as notifications for the user.

They are designed to be passed to the app when it comes to the foreground.

eg.
```json
{
   "notification" : 
   {
    "body" : "hello",
    "title": "firebase",
    "sound": "default"
   },
    "registration_ids" : ["<an FCM push token>"]
}
```

### Data messages

Data messages contain a `data` section, with arbitrary fields for the app to process.

Data messages are delivered to the app immediately even if it is backgrounded/killed. Firebase does not generate a visual notification for data messages - it is up to the app to do so if appropriate.

eg.
```json
{
    "data": {
        "message" : "my_custom_value",
        "other_key" : true,
        "body":"test"
     },
     "priority": "high",
     "condition": "'general' in topics"
}
```

## iOS push notification types

iOS push notifications come in a few modes, too:

* `Alert` - a push notification that will be automatically displayed for the user.
* `Silent` - a push notification that will be silently delivered to the application in the background.
* `Mixed mode` - a push notification with aspects of both alert and silent modes.

To receive silent notifications, an app must declare the **Remote notifications** setting enabled in `Info.plist`.

### Mixed mode messages

NB. Data notifications will not display on iOS devices either. To generate a visual notification, use the `notification` section.

NB. In order to send a silent notification, the `notification` section should set `content_available: true`. The consequence of not setting `content_available: true` is that if the app is backgrounded, the user must tap the notification before it is sent to the app.

ie.
```json
"notification": {
    "content_available" : true
},
```

eg. a visual notification is created, and background data is sent to the app:
```json
{
    "data": {
        "message" : "my_custom_value",
        "other_key" : true,
        "body":"test"
     },
     "notification": {
       "body" : "hello",
       "title": "firebase",
       "sound": "default",
        "content_available" : true
     },
     "priority": "high",
     "condition": "'general' in topics"
}
```

eg. background data is sent to the app, but no notification is created:
```json
{
    "data": {
        "message" : "my_custom_value",
        "other_key" : true,
        "body":"test"
     },
     "notification": {
        "content_available" : true
     },
     "priority": "high",
     "condition": "'general' in topics"
}
```

## Resources

* [Firebase Push Notifications](https://github.com/CrossGeeks/FirebasePushNotificationPlugin/blob/master/docs/FirebaseNotifications.md) (CrossGeeks / FirebasePushNotificationPlugin documentation)
