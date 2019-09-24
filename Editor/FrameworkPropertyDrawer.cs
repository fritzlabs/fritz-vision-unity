﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomPropertyDrawer (typeof(FritzFramework))]
public class FrameworkPropertyDrawer : PropertyDrawer
{

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
        var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
        var path = property.propertyPath;
        // Hacky way of getting path by referring to reference. The object for some reason
        // Points to the list of frameworks, not the framework itself.
        // Pull the index from the propertyPath where the index is in the 2nd to last
        // character.
        int arrayIndexChar = path.Length - 2;
        int index = int.Parse(property.propertyPath[arrayIndexChar].ToString());
        
        var frameworks = obj as FritzFramework[];
        var framework = frameworks[index];

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var version = framework.Version;
        if (version == null)
        {
            var statusRect = new Rect(position.x, position.y, 100, position.height);
            var status = "Not downloaded";
            EditorGUI.LabelField(statusRect, status);
        } else
        {
            var versionRect = new Rect(position.x, position.y, 70, position.height);
            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.LabelField(versionRect, version);
        }


        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

}
