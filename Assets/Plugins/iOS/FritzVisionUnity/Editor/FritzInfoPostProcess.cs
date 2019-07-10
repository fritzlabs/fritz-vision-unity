using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.Build.Player;
using System.Diagnostics;

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
        string libraryPath = "Libraries/Plugins/iOS/FritzVisionUnity/Source/";
        string plistPath = Path.Combine(buildPath, libraryPath, "Fritz-Info.plist");
        PlistDocument plist = new PlistDocument();

        plist.ReadFromFile(plistPath);

        // TODO: Pull out API Key specification here, until then, set configuration in Source/Fritz-Info.plist file.

        //plist.root.SetString("apiKey", "YOUR API KEY HERE");
        //plist.root.SetString("apiUrl", "https://api.fritz.ai/sdk/v1");
        //plist.root.SetString("namespace", "production");

        File.WriteAllText(plistPath, plist.WriteToString());

        string infoPath = Path.Combine(buildPath, "Info.plist");
        PlistDocument infoPlist = new PlistDocument();

        infoPlist.ReadFromFile(infoPath);
        infoPlist.root.SetString("NSCameraUsageDescription", "For ML Camera Usage");
        File.WriteAllText(infoPath, infoPlist.WriteToString());

        var projPath = buildPath + "/Unity-Iphone.xcodeproj/project.pbxproj";
        var proj = new PBXProject();
        proj.ReadFromFile(projPath);

        var targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

        string plistGuid = proj.AddFile(plistPath, Path.Combine(libraryPath, "Fritz-Info.plist"));
        proj.AddFileToBuild(targetGuid, plistGuid);
        proj.WriteToFile(projPath);
    }

}
