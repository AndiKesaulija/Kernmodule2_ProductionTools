using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace ProductionTools
{
    [ExecuteInEditMode]
    public class MapMaker : EditorWindow
    {
        public Project currentProject;

        public InputManager myInputManager;

        public string projectFolder;
        public string ObjectFolder;


        public int sourceKey;
        public GameObject source;

        public List<IActionInput> actions = new List<IActionInput>();//List of all available Actions/Stamps
        public int currentActionNum;
        public ActionSettings globalSettings;
        public List<ICommand> commands = new List<ICommand>();//List of all Commands done in session
        public List<CreateObjectCommand> allObjects = new List<CreateObjectCommand>();

        public CreateObjectCommand currentObject;


        public List<GameObject> placeObjects = new List<GameObject>();

        //Editor buttons
        public bool isSelected;
        private bool[] placeableObjects = new bool [0];
        public bool[] buttons = new bool[0];

        public bool isActive;
        //GUI
        private Vector2 scrollObjectList = new Vector2(0,0);
        public object selection;
        public object newselection;

        //SceneGUI
        public SceneGuiHandler myHandler;

        static MapMaker window;
        [MenuItem("Window/Map Maker")]


        static void Init()
        {
            window = (MapMaker)EditorWindow.GetWindow(typeof(MapMaker));

            window.Show();
            window.minSize = new Vector2(300, 500);
            window.maxSize = new Vector2(300, 800);

            Debug.Log("Open MM");


        }

       

        private void OnEnable()
        {
            //Set owner of Postprocessor
            MyAssetPostprocessor.owner = this;

            Tools.hidden = true;
            isActive = true;

            projectFolder = Path.Combine(Application.dataPath, "MapMaker", "Projects");
            
            ObjectFolder = Path.Combine(Application.dataPath, "Resources", "MapMaker", "Objects");


            if (Directory.Exists(projectFolder) == false)
            {
                Directory.CreateDirectory(projectFolder);
                Debug.Log("Created new projects folder at: " + projectFolder);
            }
            if (Directory.Exists(Path.Combine(Application.dataPath, "Resources")) == false)
            {
                Directory.CreateDirectory(Path.Combine(Application.dataPath, "Resources"));
            }
            if (Directory.Exists(Path.Combine(Application.dataPath, "Resources", "MapMaker")) == false)
            {
                Directory.CreateDirectory(Path.Combine(Application.dataPath, "Resources", "MapMaker"));
            }
            if (Directory.Exists(ObjectFolder) == false)
            {
                Directory.CreateDirectory(ObjectFolder);
            }


            SceneView.duringSceneGui += this.OnSceneGUI;

            globalSettings = ScriptableObject.CreateInstance<ActionSettings>();
            globalSettings.Init(this);
            myInputManager = new InputManager(this);

            AddAction(new PlaceObject(this, ObjectType.single));
            AddAction(new PlaceObject(this, ObjectType.line));
            AddAction(new PlaceObject(this, ObjectType.curve));


            myHandler = ScriptableObject.CreateInstance<SceneGuiHandler>();
            myHandler.owner = this;

            buttons = new bool[actions.Count];//MakeButtons for Actions


            ClearProject();

        }
        private void OnDisable()
        {
            Tools.hidden = false;

            if (currentProject != null)
            {
                currentProject.ClearMap();
            }

            foreach(GameObject obj in placeObjects)
            {
                DestroyImmediate(obj);
            }
            placeObjects.Clear();
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }
        public void ReloadMapMaker()
        {

            if (currentProject != null)
            {
                currentProject.myObjectPool.ReloadObjectList();
                placeableObjects = new bool[currentProject.myObjectPool.objectList.Count];//MakeButons for Objects
                buttons = new bool[actions.Count];//MakeButtons for Actions

                isActive = true;

            }
            else
            {
                Debug.Log("No current Project");
            }


            Repaint();
        }
        
        void MapMakerStatsGUI()
        {
            GUILayout.Label($"Current Project: {currentProject}");
            if(currentProject != null)
            {
                string currentMapName = "";
                if (currentProject.currentMap != null)
                {
                    currentMapName = currentProject.currentMap.name;
                }

                if( currentProject.currentMap != null)
                {
                    GUILayout.Label($"ObjectPool Count: {currentProject.myObjectPool.objectList.Count} Map: {currentMapName}");
                }
                else
                {
                    GUILayout.Label($"ObjectPool Count: {currentProject.myObjectPool.objectList.Count} Map: No map selected");

                }


            }
            else
            {
                GUILayout.Label($"ObjectPoolCount: No Current Project");
            }
            



            GUILayout.Label($"MouseState: {myInputManager.state}");
            GUILayout.Label($"onHover: {myInputManager.onHover}");
        }
        void OnGUI()
        {
            //Menu Bar
            EditorGUILayout.BeginHorizontal();
            ProjectDrawDropdown(new GUIContent("Project"));
            MapDrawDropdown(new GUIContent("Map"));
            ObjectDrawDropdown(new GUIContent("Objects"));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            //MapMaker Sats
            MapMakerStatsGUI();
            EditorGUILayout.Space();

            //Current placeable objects
            GUIObjectPool();
            EditorGUILayout.Space();

            //Tool bar to place objects with
            GUITools();
            EditorGUILayout.Space();

            //Current tool settings
            GUIToolSettings(myInputManager.currentAction);
        }
        public void ProjectDrawDropdown(GUIContent label)
        {
            GUIContent buttonText = label;
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("New Project"), false, MenuNewProject, null);
            menu.AddItem(new GUIContent("Load Project"), false, MenuLoadProject, null);
            if (currentProject != null)
            {
                menu.AddItem(new GUIContent("Save Project"), false, MenuSaveProject, null);
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Save Project"));
            }




            Rect rt = GUILayoutUtility.GetRect(buttonText, GUIStyle.none, GUILayout.Width(100));
            if (GUI.Button(rt, buttonText))
            {
                menu.DropDown(rt);
            }

            void MenuNewProject(object property)
            {
                MM_PopUpWindow.Init(this, WindowType.NewProject);
            }
            void MenuLoadProject(object property)
            {
                MM_PopUpWindow.Init(this, WindowType.LoadProject);
            }
            void MenuSaveProject(object property)
            {
                //string path = EditorUtility.SaveFilePanel("Save Project", currentProject.projectPath, currentProject.name, "unitypackage");
                string path = currentProject.savePath;

                if (path.Length != 0)
                {
                    SaveProject();
                }
                else
                {
                    Debug.Log("Save Canceled");
                }
            }
        }
        public void MapDrawDropdown(GUIContent label)
        {
            GUIContent buttonText = label;
            GenericMenu menu = new GenericMenu();
            if(currentProject != null)
            {
                menu.AddItem(new GUIContent("New Map"), false, MapNewProject, null);
                menu.AddItem(new GUIContent("Load Map"), false, MenuLoadMap, null);

                if(currentProject.currentMap != null)
                {
                    menu.AddItem(new GUIContent("Save Map"), false, MenuSaveMap, null);
                }
                else
                {
                    menu.AddDisabledItem(new GUIContent("Save Map"));
                }

                menu.AddItem(new GUIContent("Clear Map"), false, MenuClearMap, null);
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("New Map"));
                menu.AddDisabledItem(new GUIContent("Load Map"));
                menu.AddDisabledItem(new GUIContent("Save Map"));
                menu.AddDisabledItem(new GUIContent("Clear Map"));
            }
            


            Rect rt = GUILayoutUtility.GetRect(buttonText, GUIStyle.none, GUILayout.Width(100));
            if (GUI.Button(rt, buttonText))
            {
                menu.DropDown(rt);
            }

            void MapNewProject(object property)
            {
                MM_PopUpWindow.Init(this, WindowType.NewMap);
            }
            void MenuLoadMap(object property)
            {
                MM_PopUpWindow.Init(this, WindowType.LoadMap);
            }
            void MenuSaveMap(object property)
            {
                currentProject.SaveMap(currentProject.currentMap, allObjects);
            }
            void MenuClearMap(object property)
            {
                currentProject.ClearMap();
            }
        }
        public void ObjectDrawDropdown(GUIContent label)
        {
            GUIContent buttonText = label;
            GenericMenu menu = new GenericMenu();
            if(currentProject != null)
            {
                menu.AddItem(new GUIContent("Add Objects From Assets"), false, ObjectsAdd, null);
                //menu.AddItem(new GUIContent("Import Objects"), false, ImportObjects, null);
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Add Objects From Assets"));
                //menu.AddDisabledItem(new GUIContent("Import Objects"));
            }


            Rect rt = GUILayoutUtility.GetRect(buttonText, GUIStyle.none, GUILayout.Width(100));
            if (GUI.Button(rt, buttonText))
            {
                menu.DropDown(rt);
            }

            //void ImportObjects(object property)
            //{
            //    MM_PopUpWindow.Init(this, WindowType.ImportObjects);
            //}
            void ObjectsAdd(object property)
            {
                MM_PopUpWindow.Init(this, WindowType.AddObjects);
            }

        }
        private void GUIObjectPool()
        {
            GUILayout.Label("ObjectList");
            scrollObjectList = EditorGUILayout.BeginScrollView(scrollObjectList, GUILayout.Height(400),GUILayout.Width(position.width));
            isSelected = false;
            source = null;

            if(currentProject != null && isActive == true)
            {
                try
                {
                    for (int i = 0; i < placeableObjects.Length; i++)
                    {
                        if (placeableObjects[i] = GUILayout.Toggle(placeableObjects[i], currentProject.myObjectPool.objectList[i].name, "Button"))
                        {
                            if (placeableObjects[i] == true)
                            {
                                sourceKey = i;
                                source = currentProject.myObjectPool.objectList[i];
                                isSelected = true;
                            }

                            if (Selection.activeGameObject != null && myInputManager.currentAction != null)
                            {
                                if (Selection.activeGameObject.GetComponent<ObjectProperties>() != null)
                                {
                                    Selection.activeGameObject.GetComponent<ObjectProperties>().buildingID = i;
                                    myInputManager.currentAction.settings.Update();
                                }

                            }


                            for (int j = 0; j < placeableObjects.Length; j++)
                            {
                                if (j != i)
                                {
                                    placeableObjects[j] = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    Debug.Log(e.StackTrace);

                }


            }
            
            EditorGUILayout.EndScrollView();
        }
        public void SetActiveObject(int num)
        {
            for (int i = 0; i < placeableObjects.Length; i++)
            {
                placeableObjects[i] = false;
            }
            placeableObjects[num] = true;

        }
        private void GUITools()
        {
            GUILayout.Label("Tools");
            myInputManager.currentAction = GUIList(actions);



        }
        private void GUIToolSettings(IActionInput currentTool)
        {
            GUILayout.Label("Settings");
            

            
            if (currentTool != null)
            {

                EditorGUI.BeginChangeCheck();
                //EditorGUILayout.PropertyField(currentTool.so.FindProperty("buildingID"));
                //EditorGUILayout.Space();

                EditorGUILayout.PropertyField(currentTool.so.FindProperty("spacing"));
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(currentTool.so.FindProperty("rotateTo"));
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(currentTool.so.FindProperty("rotationCounter"));
                EditorGUILayout.Space();


                if (EditorGUI.EndChangeCheck())
                {
                    currentTool.so.ApplyModifiedProperties();
                    currentTool.Update();
                }
                


                EditorGUILayout.Space();
            }

        }
        public void SaveProject()
        {
            string path = currentProject.savePath;
            string SaveName = currentProject.name;
            string saveLocation = Path.Combine(path, SaveName);

            Debug.Log(saveLocation);
            if(currentProject != null)
            {
                if(currentProject.currentMap != null)
                {
                    currentProject.SaveMap(currentProject.currentMap, allObjects);
                }

                var exportedPackageAssetList = new List<string>();

                exportedPackageAssetList.Add("Assets/Resources/MapMaker/Objects");
                exportedPackageAssetList.Add("Assets/MapMaker/Projects/" + currentProject.name);

                AssetDatabase.ExportPackage(exportedPackageAssetList.ToArray(), saveLocation + ".unitypackage", ExportPackageOptions.Recurse);

                AssetDatabase.Refresh();
            }
            else
            {
                Debug.Log("No active Project");
            }
            
        }
        public void LoadProject(string path)
        {
            isActive = false;

            ClearProject();

            string[] projectPath = path.Split('/', '\\', '.');
            string projectName = projectPath[projectPath.Length - 2];

            currentProject = new Project(projectName, this);
            
            AssetDatabase.ImportPackage(path, false);

        }

       




        public void ClearProject()
        {
            if(currentProject != null)
            {
                currentProject.ClearMap();
            }

            //Delete Resources/MapMaker/Objects
            if (Directory.Exists("Assets/Resources/MapMaker/Objects") == true)
            {
                string[] currentObjects = Directory.GetFiles("Assets/Resources/MapMaker/Objects");

                foreach (string path in currentObjects)
                {
                    File.Delete(path);
                }

            }
            else
            {
                Debug.Log("Directory not found");
            }
            AssetDatabase.Refresh();
        }
        void OnSceneGUI(SceneView sceneView)
        {
            //Set Tool to SelectionTool
            //if (Tools.current != Tool.Custom)
            //{
            //    Tools.current = Tool.Custom;
            //}

            Event e = Event.current;

            myHandler.OnSceneGUI();//Show GUI Handlers for selected GroupObject

            //if (myInputManager.currentAction != null)
            //{
            //    myInputManager.HandleEvent(e);
            //}

            myInputManager.HandleEvent(e);

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue))
            {
                //Hover Bool true
                if(hit.collider.tag == "SelectableObject")
                {
                    myInputManager.onHover = true;
                }
                

            }
            else
            {
                myInputManager.onHover = false;
            }



        }
        public Vector3 RayPosition()
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Plane hPlane = new Plane(Vector3.up, Vector3.zero);

            float distance = 0;

            if (hPlane.Raycast(ray, out distance))
            {
                return ray.GetPoint(distance);
            }

            return new Vector3(0, 0, 0);
        }
        public void AddCommand(CreateObjectCommand command)
        {
            allObjects.Add(command);
            allObjects[allObjects.Count - 1].Execute();
        }
        public void RemoveCommand(int num)
        {
            allObjects[num].Undo();
            allObjects.RemoveAt(allObjects.Count - 1);
        }
        public void AddAction(IActionInput action)
        {
            actions.Add(action);
        }

        IActionInput GUIList(List<IActionInput> target)
        {
            IActionInput activeItem = null;
            if (currentProject == null)
            {
                GUI.enabled = false;
            }
            else
            {
                if(currentProject.currentMap == null)
                {
                    GUI.enabled = false;
                }
            }
            
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < target.Count; i++)
            {
                
                if (buttons[i] = GUILayout.Toggle(buttons[i],target[i].name, "Button"))
                {
                    if (buttons[i] == true)
                    {

                        activeItem = target[i];
                        currentActionNum = i;
                        activeItem.settings.myType = (ObjectType)i;
                        activeItem.Update();

                        //EditorGUILayout.HelpBox(activeItem.actionInfo, MessageType.Info);//Test

                    }
                    for (int j = 0; j < target.Count; j++)
                    {
                        if (j != i)
                        {
                            buttons[j] = false;
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            GUI.enabled = true;
            return activeItem;
        }
        public void setAction(List<IActionInput> target, int num)
        {
            buttons[num] = true;

            for (int j = 0; j < target.Count; j++)
            {
                if (j != num)
                {
                    buttons[j] = false;
                }
            }
        }

       
    }

}
