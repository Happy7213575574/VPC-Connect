# Known issues

## Open

### Android icon and splash screen scaling

At current time, the Android icon image is significantly larger than the icon itself, and cropped to fit.

Similarly, the splash screen is rendered wider than the display width and cropped.

### Visual Studio cannot build in `Release` mode

This issue relates to a bug in Visual Studio. Microsoft have indicated an intention to fix this in future releases.

## Resolved

### Build cannot find a path to the Android SDK

Symptom occurs on OS X:

* The Android build fails, because the path for the Android SDK cannot be found.
* The path for the Android SDK specified in the preferences at `Preferences / Projects / SDK Locations / Android / Locations` does not agree with the error message path.

Solution:

* Create a link to the real SDK location from the location in the error.

eg.
```
ln -s ~/Library/Developer/Xamarin/android-sdk-macosx ~/Library/Android/sdk
```

### Android emulator cannot launch on Apple M1

This was a known issue on new Apple hardware (specifically the M1).

* 2021-03-10 Preview 3 of a new Android Emulator is available.
* Visit: https://github.com/google/android-emulator-m1-preview/releases
* Download and install: `android-emulator-m1-preview.dmg`
* Launch and test - then confirm that it appears as an option in Visual Studio.

### Apple Certificates are not trusted

* Apple have updated their intermediate Developer Relations CA certificate
* Download and install from: https://www.apple.com/certificateauthority/AppleWWDRCAG3.cer

### Background notifications

Notifications were not appearing or being delivered to the app under some circumstances.

See: [FCM push notification content](push-content.md)

Resolved through notification content.

### ITMS-90809 UIWebView Deprecation (App Store Rejection)

Xamarin have documented the [ITMS-90809 workaround](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/webview?tabs=macos#uiwebview-deprecation-and-app-store-rejection-itms-90809).
