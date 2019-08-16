
# Fritz Unity Pose Plugin

## Installation instructions

1. Download Fritz Frameworks (Make sure to run script from the `FritzVisionUnity/` folder:

```
./download_fritz_frameworks.sh
```

Check to make sure that the script succeeded. In `Frameworks/` you should see a few Fritz frameworks, including `FritzVision.framework/`, `FritzVisionPoseModel.framework/`.

Note: If you are using Xcode 10, use version 3.7.0. If you are using Xcode 11 Beta 5, use 3.7.0-beta.1.


2. Update the `FritzVisionUnity/Source/Fritz-Info.plist` file with your own API Key.

You'll have to create an app in Fritz. Make sure that your bundle identifier matches the app you create. The easiest way is to make sure to set the bundle identifier in the iOS player settings in Unity (pre-iOS build).
