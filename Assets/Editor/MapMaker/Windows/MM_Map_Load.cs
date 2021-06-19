using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace ProductionTools
{
    public class MM_Map_Load : IPopUp
    {
        private MapMaker owner;
        private MM_PopUpWindow parent;
        public List<string> maps;


        private bool[] buttons;
        private int selectedMap;
        public MM_Map_Load(string path, MapMaker owner, MM_PopUpWindow parent)
        {
            this.owner = owner;
            this.parent = parent;
            maps = new List<string>();

            foreach (string file in Directory.GetFiles(path))
            {
                if(getExtention(file) == "txt")
                {
                    maps.Add(file);
                }
            }
            
            
            buttons = new bool[maps.Count];
        }
        public void ShowContent()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] = GUILayout.Toggle(buttons[i], getName(maps[i]), "Button"))
                {
                    if (buttons[i] == true)
                    {
                        selectedMap = i;
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
            if (GUILayout.Button("Load Map"))
            {
                owner.currentProject.ClearMap();
                owner.currentProject.LoadMap(maps[selectedMap]);
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
            string[] mapPath = path.Split('.','/','\\');
            string ExtentionName = mapPath[mapPath.Length - 1];

            return ExtentionName;
        }
    }
}