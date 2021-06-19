using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;//Maybe
using System.Xml.Serialization;
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

            Map newMap = new Map(name, new List<GameObject>(), mapDirectory);
            currentMap = newMap;

            owner.SaveProject();
        }
        public void LoadMap(string path)
        {
            owner.myInputManager.currentAction = null;
            if(path != null)
            {
                MapData myMapData = new MapData();

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(MapData));
                StreamReader sReader;
                try
                {
                    sReader = new StreamReader(path);

                    myMapData = (MapData)xmlSerializer.Deserialize(sReader);

                    sReader.Close();
                }
                catch (FileNotFoundException e)
                {
                    Debug.Log("FileNotFound: " + e);
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }

                foreach (GroupData group in myMapData.mapData)
                {
                    owner.AddCommand(new CreateObjectCommand(myObjectPool.objectList[group.buildingID], group.position, group.p0, group.p1, group.p2, group.lookAtPoint, group.spacing, group.buildingID, group.myType, group.rotateTo, group.rotationCounter, owner));
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
                    catch (Exception e)
                    {
                        //Debug.Log(e);
                    }
                }

                target.WriteMap();
                //myMaps[0] = target;
            }
            else
            {
                Debug.Log("No Map Selected");
            }
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