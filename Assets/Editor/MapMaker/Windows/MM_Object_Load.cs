using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProductionTools
{
    public class MM_Object_Load : IPopUp
    {
        MapMaker owner;
        MM_PopUpWindow parent;
        bool importObject;

        string assetPath = "Assets/Resources/MapMaker/Objects/";

        MM_Object_Wrapper myObjects;
        SerializedProperty _sources;
        SerializedProperty _paths;

        SerializedObject so;
        private Vector2 scrollY = new Vector2(0, 0);

        public MM_Object_Load(MapMaker owner, MM_PopUpWindow parent, bool importObject)
        {
            myObjects = ScriptableObject.CreateInstance<MM_Object_Wrapper>();
            so = new SerializedObject(myObjects);
            _sources = so.FindProperty("sources");
            _paths = so.FindProperty("paths");

            this.owner = owner;
            this.parent = parent;
            this.importObject = importObject;
        }
        public void ShowContent()
        {
            EditorGUI.BeginChangeCheck();
            scrollY = EditorGUILayout.BeginScrollView(scrollY, GUILayout.Height(parent.minSize.y - 20));
            EditorGUILayout.PropertyField(_sources);
            EditorGUILayout.EndScrollView();


            
        }
        public void ShowButtons()
        {
            GUILayout.BeginHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                so.ApplyModifiedProperties();
            }

            if (GUILayout.Button("Save"))
            {
                LoadSelection(myObjects.sources);


                owner.SaveProject();
                parent.Close();
            }

            if (GUILayout.Button("Close"))
            {
                parent.Close();
            }
            GUILayout.EndHorizontal();
        }



        private void ImportObject()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_paths);

            EditorGUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                so.ApplyModifiedProperties();
            }
            if (GUILayout.Button("Add Object"))
            {
                string path = EditorUtility.OpenFilePanel("Select files", "", "fbx");

                if (path.Length != 0)
                {
                    myObjects.paths.Add(path);
                    so.Update();
                }
            }

            if (GUILayout.Button("Save"))
            {
                parent.Close();
            }

            if (GUILayout.Button("Close"))
            {
                parent.Close();
            }
            GUILayout.EndHorizontal();
        }
        private void AddFromAssets()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_sources);

            EditorGUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                so.ApplyModifiedProperties();
            }

            if (GUILayout.Button("Save"))
            {
                LoadSelection(myObjects.sources);


                owner.SaveProject();
                parent.Close();
            }

            if (GUILayout.Button("Close"))
            {
                parent.Close();
            }
            GUILayout.EndHorizontal();
        }

        void LoadSelection(List<GameObject> target)
        {
            int currID = owner.currentProject.myObjectPool.objectList.Count + 1;

            for (int i = 0; i < target.Count; i++)
            {
                if (target[i] != null)
                {

                    GameObject tempInstance = Object.Instantiate(target[i]);
                    tempInstance.name = target[i].name;

                    if (tempInstance.GetComponent<PlacedObject>() == null)
                    {
                        tempInstance.AddComponent<PlacedObject>();
                        
                    }

                    //tempInstance.AddComponent<BoxCollider>();

                    Renderer[] allRenderers = tempInstance.GetComponentsInChildren<Renderer>();
                    foreach (Renderer R in allRenderers)
                    {
                        R.gameObject.AddComponent<BoxCollider>();
                    }

                    PrefabUtility.SaveAsPrefabAsset(tempInstance, assetPath + tempInstance.name + ".prefab");
                    Object.DestroyImmediate(tempInstance);

                    //AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(source), "Assets/Resources/MapMaker/Objects/" + source.name + ".prefab");
                }
            }
        }

        void ImportSelection(List<string> target)
        {
            int currID = owner.currentProject.myObjectPool.objectList.Count + 1;

            for (int i = 0; i < target.Count; i++)
            {
                if (target[i] != null)
                {

                    AssetDatabase.ImportAsset(target[i], ImportAssetOptions.Default);

                    //if (tempInstance.GetComponent<ObjectProperties>() == null)
                    //{
                    //    tempInstance.AddComponent<ObjectProperties>();
                    //    Debug.Log("Added ObjectProperties to Object");
                    //}
                    //tempInstance.GetComponent<ObjectProperties>().buildingID = currID + i;

                    //PrefabUtility.SaveAsPrefabAsset(tempInstance, assetPath + tempInstance.name + ".prefab");
                    //Object.DestroyImmediate(tempInstance);

                    //AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(source), "Assets/Resources/MapMaker/Objects/" + source.name + ".prefab");
                }
            }
        }

    }
}