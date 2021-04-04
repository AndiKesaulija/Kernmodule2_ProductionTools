using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProductionTools
{
    public class ObjectPool
    {

        public Dictionary<int, GameObject> objectList = new Dictionary<int, GameObject>();
        //public List<GameObject> placedObjects = new List<GameObject>();
        public void ReloadObjectList()
        {
            objectList.Clear();//EmptyList

            GameObject[] loadList = Resources.LoadAll<GameObject>("MapMaker/Objects");

            foreach (GameObject obj in loadList)
            {
                objectList.Add(objectList.Count, obj);
            }
        }



    }
}