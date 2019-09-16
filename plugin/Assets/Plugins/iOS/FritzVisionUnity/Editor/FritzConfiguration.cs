using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Register a SettingsProvider using IMGUI for the drawing framework:
static class FritzConfigurationIMGUIRegister
{
    // The settings provider lets us add a Fritz configuration to the Project Settings page.
    [SettingsProvider]
    public static SettingsProvider CreateFritzConfigProvider()
    {
        // First parameter is the path in the Settings window.
        // Second parameter is the scope of this setting: it only appears in the Project Settings window.
        var provider = new SettingsProvider("Project/Fritz", SettingsScope.Project)
        {
            // By default the last token of the path is used as display name if no label is provided.
            label = "Fritz",

            // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
            guiHandler = (searchContext) =>
            {
                var settings = FritzConfiguration.GetSerializedSettings();
                string bundleID = UnityEditor.PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS);
                EditorGUILayout.LabelField("Bundle ID", bundleID);

                EditorGUILayout.PropertyField(settings.FindProperty("iOSAPIKey"), new GUIContent("Fritz iOS API Key"));
                EditorGUILayout.HelpBox("", MessageType.Info);

                EditorGUILayout.LabelField("Frameworks", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(settings.FindProperty("sdkVersion"), new GUIContent("SDK Version"));

                if (GUILayout.Button("Download"))
                {
                    var sdkVersion = settings.FindProperty("sdkVersion").stringValue;
                    var download = new DownloadFramework(sdkVersion, "FritzBase");
                    download.Download();
                    download = new DownloadFramework(sdkVersion, "FritzVisionPoseModel");
                    download.Download();
                }

                var property = settings.FindProperty("frameworks");
                for (int i = 0; i < property.arraySize; i++)
                {
                    var element = property.GetArrayElementAtIndex(i);
                    EditorGUILayout.PropertyField(element);
                }

            },

            // Populate the search keywords to enable smart search filtering and label highlighting:
            keywords = new HashSet<string>(new[] { "Pose", "Fritz" })
        };

        return provider;
    }
}

public class FritzConfiguration : ScriptableObject
{
    public const string k_FritzConfigurationPath =
        "Assets/Plugins/iOS/FritzVisionUnity/Editor/FritzConfig.asset";

    [SerializeField]
    public string iOSAPIKey;

    [SerializeField]
    public string sdkVersion;

    // Required Frameworks
    public FritzFramework[] frameworks =
    {
        new FritzFramework("FritzVision"),
        new FritzFramework("FritzVisionPoseModel")
    };

    internal static FritzConfiguration GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<FritzConfiguration>(k_FritzConfigurationPath);
        if (settings == null)
        {
            settings = ScriptableObject.CreateInstance<FritzConfiguration>();
            settings.sdkVersion = "4.1.1";

            AssetDatabase.CreateAsset(settings, k_FritzConfigurationPath);
            AssetDatabase.SaveAssets();
        }
        return settings;
    }

    internal static SerializedObject GetSerializedSettings()
    {
        return new SerializedObject(GetOrCreateSettings());
    }
}
