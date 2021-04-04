using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProductionTools
{
    public class MM_Project_New : IPopUp
    {
        private MapMaker owner;
        private MM_PopUpWindow parent;
        private string name = "ProjectName";
        public MM_Project_New(MapMaker owner, MM_PopUpWindow parent)
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
                owner.ClearProject();

                owner.currentProject = new Project(name, owner);
                owner.ReloadMapMaker();

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