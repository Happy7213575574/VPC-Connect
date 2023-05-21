# Building the app

## Build

The project is built with Visual Studio or `dotnet build`.

The build requires access to XCode for the iOS app. This can either be installed on the local machine, or provided through an XCode server.

A known issue prevents Visual Studio from working with `Release` builds. (See below for workarounds.)

## Visual Studio debug builds

Debug builds work as expected.

* Select `Debug` mode from the menu button at the top left of the Visual Studio window.
* Select the target device, eg. `Debug` `Pixel 5 - API 33 (API 33)`
* Press the play button: `▶`

NB. If you do not have an Android device configured for the emulator, use the Android SDK Device Manager through the **Tools** menu to create one.

Similarly, select an iOS device from those available to launch the iOS simulator.

## Visual Studio release builds

### Known issue

**2023-05-21.** At current time, Visual Studio struggles to deploy `Release` builds to Android emulators or iOS simulators. A bug in Visual Studio causes it to halt and require _old_ Xamarin.Forms Android build tools. Microsoft have expressed an intention to fix this in future builds.

### Workaround

The workaround is to use the scripts available in the `ConnectApp.Maui` directory:

| Script | Notes |
|-|-|
| `clean-debug.sh` | Cleans the `Debug` build. |
| `clean-release.sh` | Cleans the `Release` build. |
| `build-release-android.sh` | Cleans, then builds the **Android** `Release` build. |
| `build-release-ios.sh` | Cleans, then builds the **iOS** `Release` build. |
| `launch-debug-android.sh` | Cleans, builds the **Android** `Debug` build, then launches it*. |
| `launch-debug-ios.sh` | Cleans, builds the **iOS** `Debug` build, then launches it on the **iPhone 14 Pro Max** simulator. |
| `launch-release-android.sh` | Cleans, builds the **Android** `Release` build, then launches it*. |
| `launch-release-ios.sh` | Cleans, builds the **iOS** `Release` build, then launches it on the **iPhone 14 Pro Max** simulator. |

_* Launches on an Android device connected in debug mode, or an Android emulator - whichever is visible to `adb`._

## Publishing

### Archive for publishing

* First, build the appropriate version of the application with either script:

  ```zsh
  ./build-release-android.sh
  ./build-release-ios.sh
  ```

* In Visual Studio, ensure that `Release` mode is selected.
* For an Android build, ensure that an Android target is selected.
* For an iOS build, ensure that **Generic Device** is selected.
* Archive with: **Build** / **Archive for Publishing**

### Sign and distribute

* Open the archive view with: **Build** / **View Archives**
* Select the new archive, and choose: **Sign and Distribute...**
* Follow the prompts to sign and distribute the application.

_You have the option of distributing to the Play Store, or App Store, directly using their APIs (untested), or signing, saving to disk and then uploading through their own tools._

#### Uploading to the Play Store (Android)

Having signed and saved your `.aab` (Android App Bundle), you can now create a release in the Play Store.

Recommended:

* Create an internal testing release.
* Distribute to colleagues for testing.
* When ready, _promote_ this release to Production.

#### Uploading to the App Store

Having signed and saved your `.ipa` (iOS Package for the App Store), you can now upload it to the App Store.

Recommended:

* Use the Mac OS [Transporter](https://apps.apple.com/us/app/transporter/id1450874784?mt=12) app provided by Apple.

### Distribution

| OS | Control panel | Live listing |
|-|-|-|
| iOS | [App Store Connect](https://appstoreconnect.apple.com/apps) | [VPC Connect](https://apps.apple.com/us/app/vpc-connect/id1515015468) (App Store) |
| Android | [Google Play Console](https://play.google.com/console/u/0/developers/8638401992351776230/app/4975489043703658288/app-dashboard) | [VPC Connect](https://play.google.com/store/apps/details?id=org.vpc.connect) (Google Play) |
