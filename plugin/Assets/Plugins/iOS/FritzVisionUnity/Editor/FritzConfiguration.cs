using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Register a SettingsProvider using IMGUI for the drawing framework:
static class FritzConfigurationIMGUIRegister
{

    private static readonly string apiKeyMessage = "To use the Fritz Unity Plugin, you must have a valid Fritz API key for your Bundle ID. Register for a Fritz account below or login to create your app in Fritz";
    private static readonly string fritzSignup = "https://app.fritz.ai/register?utm_campaign=fritzunity&utm_source=unity";
    private static readonly string fritzLogin = "https://app.fritz.ai?utm_campaign=fritzunity&utm_source=unity";
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

                EditorGUILayout.HelpBox(apiKeyMessage, MessageType.Info);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Register", GUILayout.Width(60f)))
                {
                    Application.OpenURL(fritzSignup);
                }
                if (GUILayout.Button("Login", GUILayout.Width(60f)))
                {
                    Application.OpenURL(fritzLogin);
                }
                GUILayout.EndHorizontal();

                string bundleID = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS);
                EditorGUILayout.LabelField("Bundle ID", bundleID);
                EditorGUILayout.PropertyField(settings.FindProperty("iOSAPIKey"), new GUIContent("Fritz iOS API Key"));

                EditorGUILayout.LabelField("Frameworks", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(settings.FindProperty("sdkVersion"), new GUIContent("SDK Version"));

                if (GUILayout.Button("Download", GUILayout.Width(80f)))
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
