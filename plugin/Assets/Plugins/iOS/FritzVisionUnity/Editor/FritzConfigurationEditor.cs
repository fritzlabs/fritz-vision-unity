//using UnityEngine;
//using UnityEditor;


//[CustomEditor(typeof(FritzConfiguration))]
//public class FritzConfigurationEditor : Editor
//{
//    private SerializedProperty property;

//    void OnEnable()
//    {
//        property = serializedObject.FindProperty("frameworks");
//    }

//    public override void OnInspectorGUI()
//    {
//        FritzConfiguration config = (FritzConfiguration)target;
//        string bundleID = UnityEditor.PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS);
//        EditorGUILayout.LabelField("Bundle ID", bundleID);

//        config.iOSAppID = EditorGUILayout.TextField("Fritz iOS API Key", config.iOSAppID);
//        EditorGUILayout.HelpBox("", MessageType.Info);

//        EditorGUILayout.LabelField("Frameworks", EditorStyles.boldLabel);
//        config.sdkVersion = EditorGUILayout.TextField("SDK Version", config.sdkVersion);

//        if (GUILayout.Button("Download"))
//        {
//            //var download = new DownloadFramework(config.sdkVersion, "FritzBase");
//            //download.Download();
//            //download = new DownloadFramework(config.sdkVersion, "FritzVisionPoseModel");
//            //download.Download();
//        }

//        for (int i = 0; i < property.arraySize; i++)
//        {
//            var element = property.GetArrayElementAtIndex(i);
      
//            EditorGUILayout.PropertyField(element);
//        }

//    }
//}
