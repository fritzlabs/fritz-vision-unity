using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.Build.Player;


using System.IO;
using System.Linq;

public static class FritzInfoPostProcess
{

    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget != BuildTarget.iOS)
        {
            return;
        }

        string pluginPath = "Plugins/iOS/FritzVisionUnity/Source/";
        string sourceFolder = Path.Combine(Application.dataPath, pluginPath);
        string libraryPath = "Libraries/Plugins/iOS/FritzVisionUnity/Source/";
        string plistPath = Path.Combine(sourceFolder, "Fritz-Info.plist");
        string xcodePath = Path.Combine(buildPath, libraryPath, "Fritz-Info.plist");

        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        // TODO: Pull out API Key specification here, until then, set configuration in Source/Fritz-Info.plist file.
        FritzConfiguration config = FritzConfiguration.GetOrCreateSettings();
        plist.root.SetString("apiKey", config.iOSAppID);
        File.WriteAllText(xcodePath, plist.WriteToString());

        var projPath = buildPath + "/Unity-Iphone.xcodeproj/project.pbxproj";
        var proj = new PBXProject();
        proj.ReadFromFile(projPath);

        var targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

        string plistGuid = proj.AddFile(xcodePath, Path.Combine(libraryPath, "Fritz-Info.plist"));
        proj.AddFileToBuild(targetGuid, plistGuid);

        proj.WriteToFile(projPath);

        // Update Info with Camera usage description
        string infoPath = Path.Combine(buildPath, "Info.plist");
        PlistDocument infoPlist = new PlistDocument();

        infoPlist.ReadFromFile(infoPath);
        infoPlist.root.SetString("NSCameraUsageDescription", "For ML Camera Usage");
        File.WriteAllText(infoPath, infoPlist.WriteToString());

    }

}
