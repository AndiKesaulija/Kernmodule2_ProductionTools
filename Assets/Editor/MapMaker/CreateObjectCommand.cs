using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

namespace ProductionTools
{
    public class CreateObjectCommand : ICommand
    {
        public ObjectType myType;
        public GameObject Prefab;
        
        public Vector3 position;
        public Vector3 lookAtPoint;
        public GameObject GameObjectInstance;
        public MapMaker owner;
        public GameObject parent;

        public Vector3 posA;
        public Vector3 posB;
        public Vector3 posC;
        public float spacing;
        public int buildingID;
        public bool rotateTo;
        public int rotationCounter;

        Transform handleTransform;
        public int commandNum { get; set; }
        public string name{ get{ return "Place Object"; } }
        public CreateObjectCommand(GameObject prefab, Vector3 position, Vector3 posA, Vector3 posB, Vector3 posC, Vector3 lookAtPoint, float spacing,int buildingID,int myType, bool rotateTo,int rotationCounter, MapMaker owner)
        {
            Prefab = prefab;
            this.posA = posA;
            this.posB = posB;
            this.posC = posC;
            this.lookAtPoint = lookAtPoint;
            this.owner = owner;
            this.position = position;
            this.spacing = spacing;
            this.buildingID = buildingID;
            this.rotateTo = rotateTo;
            this.rotationCounter = rotationCounter;
            this.myType = (ObjectType) myType;

        }

        public void Execute()
        {
            GameObjectInstance = SpawnObject(posA, posB, posC, spacing, owner);
            commandNum = GameObjectInstance.GetComponent<ObjectProperties >().commandNum;
            GameObjectInstance.GetComponent<ObjectProperties>().myType = (int) myType;

            GameObjectInstance.GetComponent<ObjectProperties >().p0 = posA;
            GameObjectInstance.GetComponent<ObjectProperties >().p1 = posB;
            GameObjectInstance.GetComponent<ObjectProperties >().p2 = posC;
            GameObjectInstance.GetComponent<ObjectProperties >().lookAtPoint = lookAtPoint;

            GameObjectInstance.GetComponent<ObjectProperties >().spacing = spacing;
            GameObjectInstance.GetComponent<ObjectProperties >().buildingID = buildingID;
            GameObjectInstance.GetComponent<ObjectProperties >().rotateTo = rotateTo;
            GameObjectInstance.GetComponent<ObjectProperties >().rotationCounter = rotationCounter;

            GameObjectInstance.transform.position = position;

            //Selection.activeGameObject = GameObjectInstance;

            Update();
        }
        public void Undo()
        {
            GameObject.DestroyImmediate(GameObjectInstance);

        }
      
        public void Update()
        {
            Clear();
            buildingID = GameObjectInstance.GetComponent<ObjectProperties>().buildingID;
            Prefab = owner.currentProject.myObjectPool.objectList[buildingID];

            switch (myType)
            {
                case ObjectType.single:
                    {
                        UpdateSingle();
                    }
                break;
                case ObjectType.line:
                    {
                        UpdateLine();

                    }
                    break;
                case ObjectType.curve:
                    {
                        UpdateCurve();
                    }
                    break;
            }
          
        }
        public void Clear()
        {
            List<GameObject> Children = new List<GameObject>();
            for (int i = 0; i < GameObjectInstance.transform.childCount; i++)
            {
                //GameObject.DestroyImmediate(GameObjectInstance.transform.GetChild(i).gameObject);
                Children.Add(GameObjectInstance.transform.GetChild(i).gameObject);
            }

            foreach(GameObject child in Children)
            {
                GameObject.DestroyImmediate(child);
            }

        }
        private Vector3 GetPos(Vector3 pointA, Vector3 pointC, float jumpPos)
        {
            return Vector3.Lerp(pointA, pointC, jumpPos);
        }
        private Vector3 GetPos(Vector3 pointA, Vector3 pointB, Vector3 pointC, float jumpPos)
        {
            return Vector3.Lerp(Vector3.Lerp(pointA, pointB, jumpPos), Vector3.Lerp(pointB, pointC, jumpPos), jumpPos);
        }

        public GameObject SpawnObject(Vector3 p0, Vector3 p1, Vector3 p2, float spacing, MapMaker owner)
        {
            GameObject PlaceObject = new GameObject("PlaceObject");
            handleTransform = PlaceObject.transform;

            PlaceObject.transform.position = p1;//Set transform to midpoint
            PlaceObject.AddComponent<ObjectProperties >();

            posA = handleTransform.InverseTransformPoint(p0);
            posB = handleTransform.InverseTransformPoint(p1);
            posC = handleTransform.InverseTransformPoint(p2);

            //SetCommandNumber
            PlaceObject.GetComponent<ObjectProperties>().commandNum = owner.allObjects.Count;
            PlaceObject.GetComponent<ObjectProperties>().spacing = spacing;

            
            owner.myHandler.ObjectProperties.Add(PlaceObject.GetComponent<ObjectProperties>());

            Selection.activeGameObject = PlaceObject;
            return PlaceObject;

        }
        void UpdateSingle()
        {
            //GameObjectInstance.transform.position = handleTransform.InverseTransformPoint(posA);
            Vector3 pos = posA;

            Quaternion rotation;

            Vector3 relativePos = pos - posC;
            rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            rotation *= Quaternion.Euler(0, 90 * rotationCounter, 0);
            
            GameObject test = GameObject.Instantiate(Prefab, GameObjectInstance.transform, false);

            //Add Object Script
            test.AddComponent<PlacedObject>();

            test.transform.localPosition = new Vector3(0,0,0);
            test.transform.localRotation = rotation;

        }
        void UpdateLine()
        {
            float size = (Vector3.Distance(posA, posB) + Vector3.Distance(posB, posC)) * 0.01f;
            float count = size * spacing;

            for (int i = 0; i < (int)count; i++)
            {
                Vector3 pos = GetPos(posA, posC, (1.0f / ((int)count + 1)) * (i + 1));

                Quaternion rotation;
                if (rotateTo == true)
                {
                    Vector3 relativePos = pos - lookAtPoint;
                    rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    rotation *= Quaternion.Euler(0, 90 * rotationCounter, 0);
                }
                else
                {
                    Vector3 relativePos = pos - GetPos(posA, posC, (1.0f / ((int)count + 1)) * (i + 2));
                    rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    rotation *= Quaternion.Euler(0, 90 * rotationCounter, 0);

                }
                //newCommand[i] = new CreateObjectCommand(prefab, pos, rot, owner, parentObject);
                //GameObject.Instantiate(Prefab, pos, rotation, GameObjectInstance.transform);
                GameObject test = GameObject.Instantiate(Prefab, GameObjectInstance.transform, false);
                test.transform.localPosition = pos;
                test.transform.localRotation = rotation;

            }
        }
        void UpdateCurve()
        {
            float size = (Vector3.Distance(posA, posB) + Vector3.Distance(posB, posC)) * 0.01f;
            float count = size * spacing;

            for (int i = 0; i < (int)count; i++)
            {
                Vector3 pos = GetPos(posA, posB, posC, (1.0f / ((int)count + 1)) * (i + 1));

                Quaternion rotation;
                if (rotateTo == true)
                {
                    Vector3 relativePos = pos - lookAtPoint;
                    rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    rotation *= Quaternion.Euler(0, 90 * rotationCounter, 0);
                }
                else
                {
                    Vector3 relativePos = pos - GetPos(posA, posB, posC, (1.0f / ((int)count + 1)) * (i + 2));
                    rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    rotation *= Quaternion.Euler(0, 90 * rotationCounter, 0);

                }
                //newCommand[i] = new CreateObjectCommand(prefab, pos, rot, owner, parentObject);
                //GameObject.Instantiate(Prefab, pos, rotation, GameObjectInstance.transform);
                GameObject test = GameObject.Instantiate(Prefab, GameObjectInstance.transform, false);
                test.transform.localPosition = pos;
                test.transform.localRotation = rotation;

            }
        }


    }
   
}