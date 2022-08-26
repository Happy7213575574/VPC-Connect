# Analytics

Anonymous analytics are collected using Google Firebase Analytics.

* `PageView`: { `PageType`: PageType }
* `UriRequest`: { `Uri`: (string) }
* `RegistrationState`: { `State`, (RegistrationState) }
* `AppActivity`: { `Event`: (AppEvent), `IsSuccess`: (bool) }
* `PushEvent`: { `Event`: (PushEvent) }
  
Possible values for `AppEvent` enum:

* `Initialising`
* `PushTokenCheckInitiated`
* `PushTokenCheckResult`
* `GetUserTokenInitiated`
* `GetUserTokenResult`
* `SubmitRegistrationInitiated`
* `SubmitRegistrationResult`
* `SubmitDeregistrationInitiated`
* `SubmitDeregistrationResult`

Possible values for `RegistrationState` enum:

* `NotKnown`
* `NotRegistered`
* `Registered`

Possible values for `PageView` enum:

* `HomePage`
* `ConnectionPage`
* `NotificationsPage`
* `ArchivedNotificationsPage`
* `TwitterFeedsPage`
* `SafeguardingPage`
* `DebugPage`

Possible values for `PushEvent` enum:

* `TokenRefresh`
* `NotificationReceived`
* `NotificationOpened`
* `NotificationDeleted`

## Live analytics for development

Under ordinary circumstances, Firebase Analytics batches up and transmits analytics info slowly (approximately hourly) to save power.

To see instant feedback in the Firebase Analytics debug view, you'll need to enable this behaviour.

### Android emulator

When testing from Android, use the following incantation to enable instant debug analytics:

```bash
adb shell setprop debug.firebase.analytics.app org.vpc.connect
```

You can view the value of this property with:

```bash
adb shell grtprop debug.firebase.analytics.app
```

### iOS simulator

Launch the simulator with the `-FIRDebugEnabled` option.

```bash
open -a Simulator --args -FIRDebugEnabled
```
