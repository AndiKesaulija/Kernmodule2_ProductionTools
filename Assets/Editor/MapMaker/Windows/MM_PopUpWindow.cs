using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProductionTools
{
    public enum WindowType
    {
        NewProject,
        LoadProject,
        NewMap,
        LoadMap,
        SaveMap,
        AddObjects,
        ImportObjects
    }

    public class MM_PopUpWindow : EditorWindow
    {
        static MapMaker myOwner;

        static IPopUp windowContent;

        public static void Init(MapMaker owner, WindowType windowType)
        {
            MM_PopUpWindow window = (MM_PopUpWindow)EditorWindow.GetWindow(typeof(MM_PopUpWindow));

            window.Show();
            window.minSize = new Vector2(800, 300);
            window.maxSize = window.minSize;
            myOwner = owner;
            if (windowType == WindowType.LoadMap)
            {
                windowContent = new MM_Map_Load(myOwner.currentProject.mapDirectory, myOwner, window);
            }
            else if (windowType == WindowType.ImportObjects)
            {
                windowContent = new MM_Object_Load(myOwner, window, true);
            }
            else if (windowType == WindowType.AddObjects)
            {
                windowContent = new MM_Object_Load(myOwner, window, false);
            }
            else if (windowType == WindowType.NewMap)
            {
                windowContent = new MM_Map_New(myOwner, window);
            }
            else if (windowType == WindowType.NewProject)
            {
                windowContent = new MM_Project_New(myOwner, window);
            }
            else if (windowType == WindowType.LoadProject)
            {
                windowContent = new MM_Project_Load(myOwner.projectFolder, myOwner, window);
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(GUILayout.Height(280));
            windowContent.ShowContent();
            EditorGUILayout.EndVertical();
            windowContent.ShowButtons();

        }


    }

}
