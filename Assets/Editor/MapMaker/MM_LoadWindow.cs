using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace ProductionTools
{
    [ExecuteInEditMode]

    public class MM_LoadWindow : EditorWindow
    {
        static MapMaker myOwner;
        public List<GameObject> sources = new List<GameObject>();
        //bool copy = false;
        string assetPath = "Assets/Resources/MapMaker/Objects/";

        SerializedObject so;
        SerializedProperty sourcesProperty;

        public static void Init(MapMaker owner)
        {
            MM_LoadWindow window = (MM_LoadWindow)EditorWindow.GetWindow(typeof(MM_LoadWindow));

            window.Show();
            window.minSize = new Vector2(800, 300);
            Debug.Log("Open MM_LoadWindow");
            myOwner = owner;
        }

        private void OnEnable()
        {
            so = new SerializedObject(this);
            sourcesProperty = so.FindProperty("sources");

        }

        public void OnGUI()
        {
            EditorGUILayout.PropertyField(sourcesProperty, true);
            so.ApplyModifiedProperties();


            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                string path = EditorUtility.OpenFilePanel("Select files", "", "fbx");

                if (path.Length != 0)
                {
                    ModelImporter modelImporter = AssetImporter.GetAtPath(path) as ModelImporter;
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.Default);//ImportFBX
                                                                                //tps://docs.unity3d.com/ScriptReference/AssetDatabase.ImportAsset.html
                }
            }
            if (GUILayout.Button("Save"))
            {
                LoadSelection();

                this.Close();
            }

            if (GUILayout.Button("Close"))
            {
                this.Close();
            }
            GUILayout.EndHorizontal();

        }

        void LoadSelection()
        {
            int currID = myOwner.currentProject.myObjectPool.objectList.Count + 1;

            for (int i = 0; i < sources.Count; i++)
            {
                if (sources[i] != null)
                {

                    GameObject tempInstance = Instantiate(sources[i]);
                    tempInstance.name = sources[i].name;

                    PrefabUtility.SaveAsPrefabAsset(tempInstance, assetPath + tempInstance.name + ".prefab");
                    DestroyImmediate(tempInstance);

                }
            }

        }
    }
}