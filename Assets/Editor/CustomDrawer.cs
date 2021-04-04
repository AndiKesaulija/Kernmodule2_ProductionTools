using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PropertyDrawerTest))]
public class CustomDrawer : PropertyDrawer
{

    private int nameSize = 150;
    private int amountSize = 30;
    private int unitSize = 70;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        //Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);


        //Indent
        var indent = EditorGUI.indentLevel;//voor later
        EditorGUI.indentLevel = 0;

        //Rects
        var nameRect = new Rect(position.x, position.y, nameSize, position.height);
        var amountRect = new Rect(position.x + nameSize + 5, position.y, amountSize, position.height);
        var unityRect = new Rect(position.x + nameSize + 5 + amountSize + 5, position.y, unitSize, position.height);

        //Draw
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
        EditorGUI.PropertyField(unityRect, property.FindPropertyRelative("unit"), GUIContent.none);
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
