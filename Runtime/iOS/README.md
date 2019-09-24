
# Fritz Unity Pose Plugin

## Installation instructions

1. Configure Fritz

  In the Unity project, click Edit -> Project Settings -> Fritz. (If you do not see Fritz, please make sure you imported the plugin correctly).

  ### Add Fritz API Key
  Create an app in Fritz that matches your bundle identifier set in Unity. You can change the bundle identifier in the iOS player settings in Unity (Edit -> Project Settings -> Player -> Other Settings).

  ### Download Fritz Frameworks
  Click the Download button to download the Fritz Frameworks.
  
  Note: If you are using Xcode 10, use version 4.0.1. If you are using Xcode 11 Beta 5, use at least 4.1.0.
  
3. Add ARKit Packages

  In unity click on Window -> Package Manager. From there, add the ARFoundation and ARKit XR Plugin.

3. Configure Unity

  You'll need to set a few settings in your Unity Project

   - Add a Camera Usage Description in Player Settings
   - Change Architecture to Arm64
   - Change Minimum Deployment Version to at least 11.0
