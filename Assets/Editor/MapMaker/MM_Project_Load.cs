using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace ProductionTools
{
    public class MM_Project_Load : IPopUp
    {
        private MapMaker owner;
        private MM_PopUpWindow parent;
        public List<string> projects;

        private bool[] buttons;
        private int selectedProject;
        public MM_Project_Load(string path, MapMaker owner, MM_PopUpWindow parent)
        {
            this.owner = owner;
            this.parent = parent;
            projects = new List<string>();

            foreach (string file in Directory.GetFiles(path, ".", SearchOption.AllDirectories))
            {
                if (getExtention(file) == "unitypackage")
                {
                    projects.Add(file);
                }
            }


            buttons = new bool[projects.Count];
        }
        public void ShowContent()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] = GUILayout.Toggle(buttons[i], getName(projects[i]), "Button"))
                {
                    if (buttons[i] == true)
                    {
                        selectedProject = i;
                    }

                    for (int j = 0; j < buttons.Length; j++)
                    {
                        if (j != i)
                        {
                            buttons[j] = false;
                        }
                    }
                }
            }

            EditorGUILayout.Space();
           
        }
        public void ShowButtons()
        {
            if (GUILayout.Button("Load Project"))
            {
                owner.LoadProject(projects[selectedProject]);
                parent.Close();
            }
        }


        private string getName(string path)
        {
            string[] mapPath = path.Split('.', '/', '\\');
            string mapName = mapPath[mapPath.Length - 2];

            return mapName;
        }
        private string getExtention(string path)
        {
            string[] mapPath = path.Split('.', '/', '\\');
            string ExtentionName = mapPath[mapPath.Length - 1];

            return ExtentionName;
        }
    }
}