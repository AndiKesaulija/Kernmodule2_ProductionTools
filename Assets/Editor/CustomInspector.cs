using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomInspectorTest))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CustomInspectorTest test = target as CustomInspectorTest;
        test.testFloat = EditorGUILayout.Slider("Size", test.testFloat, 0.1f, 5f);
        test.Scale();

        if(GUILayout.Button("Reset Float"))
        {
            test.ResetFloat();
        }
        
    }

}
