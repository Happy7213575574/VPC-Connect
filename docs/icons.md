# Icons

Application icons deserve a special mention, as they are not trivial!

## Generate icons

Provide a large source file to [MakeAppIcon](https://makeappicon.com/) - it will generate all the resized images you need for both Android and iOS.

## iOS

Resource: [Application Icons in Xamarin.iOS](https://docs.microsoft.com/en-us/xamarin/ios/app-fundamentals/images-icons/app-icons?tabs=macos)

* Icons are managed in an 'Asset Catalog' in the iOS application called `Assets.xcassets`.
* In `Assets.xcassets` create a new icon, and then drop icons from MakeAppIcon into it - each should be named helpfully.

NB. You may find that the Itunes icons have a black background - the icon generator will have assumed black where the source file was transparent. It's probably better to supply a white background image for this, and you may need to edit the image manually to do so.

## Android

* Add the new mipmap icons from MakeAppIcon to the `Resources/mipmap-` folders.
* Ensure that they have Build action: `AndroidResource`
* Ensure that they have Custom tool: `MSBuild:UpdateGeneratedFiles`
* Modify your choice of Application icon in `Properties/AndroidManifest.xml`
* Modify the attributes in `MainActivity.cs`:
  * Label - the icon text, eg. `Label = "VPC Connect"`
  * Icon - the icon itself, eg. `Icon = "@mipmap/ic_launcher"`
