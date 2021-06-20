using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;

namespace ProductionTools
{
    public class Project : INamed
    {
        public string name { get; set; }

        public MapMaker owner;
        public ObjectPool myObjectPool;
        public Map currentMap;
        

        public string projectPath;
        public string savePath;
        public string mapDirectory;
        public string objectDirectory;

        public Project(string projectName, MapMaker owner)
        {
            name = projectName;
            myObjectPool = new ObjectPool();
            this.owner = owner;

            projectPath = Path.Combine(owner.projectFolder, name);//Check if This Project Folder Exists
            savePath = Path.Combine(owner.projectFolder, name);
            if (Directory.Exists(projectPath) != true)
            {
                Directory.CreateDirectory(projectPath);

            }

            mapDirectory = Path.Combine(projectPath, "Maps");//MapsFolder
            if (Directory.Exists(mapDirectory) != true)
            {
                Directory.CreateDirectory(mapDirectory);

            }

            objectDirectory = "Assets/Resources/MapMaker/Objects";
            AssetDatabase.Refresh();
        }

        public void NewMap(string name)
        {
            owner.currentProject.ClearMap();

            Map newMap = new Map(name, mapDirectory);
            currentMap = newMap;

        }
        public void LoadMap(string path)
        {
            owner.myInputManager.currentAction = null;
            if(path != null)
            {
                MapData myMapData = new MapData();
                BinaryFormatter bFormatter = new BinaryFormatter();
                FileStream binReader;

                try
                {
                    binReader = new FileStream(path, FileMode.Open);

                    myMapData = (MapData)bFormatter.Deserialize(binReader);

                    binReader.Close();
                }
                catch (FileNotFoundException e)
                {
                    Debug.Log("FileNotFound: " + e);
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }

                //SetCurrent Map with new map data
                currentMap = new Map(myMapData.mapName, myMapData.folderpath);

                //SpawnObjects
                foreach (GroupData group in myMapData.mapData)
                {
                    owner.AddCommand(new CreateObjectCommand(
                        myObjectPool.objectList[group.buildingID],
                        new Vector3( group.posx,group.posy, group.posz),
                        new Vector3( group.pos0x, group.pos0y, group.pos0z),
                        new Vector3(group.pos1x, group.pos1y, group.pos1z),
                        new Vector3(group.pos2x, group.pos2y, group.pos2z),
                        new Vector3(group.lookposx, group.lookposy, group.lookposz),
                        group.spacing,
                        group.buildingID,
                        group.myType,
                        group.rotateTo,
                        group.rotationCounter,
                        owner
                        ));
                }
            }
            else
            {
                Debug.Log("No Map Selected");
            }

            
        }

        public void SaveMap(Map target, List<CreateObjectCommand> placedObjects)
        {

            if (target != null)
            {
                currentMap.myMapData.mapData.Clear();

                currentMap.myMapData.mapName = currentMap.name;
                currentMap.myMapData.folderpath = currentMap.folderPath;

                foreach (CreateObjectCommand obj in placedObjects)
                {
                    try
                    {
                        currentMap.myMapData.Add(obj.GameObjectInstance, obj.GameObjectInstance.GetComponent<ObjectProperties>());
                    }
                    catch (NullReferenceException)
                    {
                        Debug.Log("NO Line component");
                    }
                   
                }

                target.WriteMap();
            }
            else
            {
                Debug.Log("No Map Selected");
            }

            owner.SaveProject();

        }
        public void ClearMap()
        {
            for (int i = 0; i < owner.allObjects.Count; i++)
            {
                owner.allObjects[i].Undo();
            }
            owner.allObjects.Clear();
        }

    }
   
}