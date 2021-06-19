using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProductionTools
{
    public class MM_Map_New : IPopUp
    {
        private MapMaker owner;
        private MM_PopUpWindow parent;
        private string name = "MapName";
        public MM_Map_New(MapMaker owner, MM_PopUpWindow parent)
        {
            this.owner = owner;
            this.parent = parent;
        }
        public void ShowContent()
        {
            name = EditorGUILayout.TextField(name);
            EditorGUILayout.Space();

            

        }
        public void ShowButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create"))
            {
                if (owner.currentProject != null)
                {
                    owner.currentProject.NewMap(name);
                }
                else
                {
                    Debug.Log("No Project Active");
                }


                parent.Close();
            }
            if (GUILayout.Button("Cancel"))
            {
                parent.Close();
            }
            EditorGUILayout.EndHorizontal();
        }


    }
}