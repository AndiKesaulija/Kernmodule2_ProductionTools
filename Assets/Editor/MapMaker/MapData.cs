using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProductionTools
{
    [Serializable]
    public class MapData
    {
        
        public List<GroupData> mapData =  new List<GroupData>();

        public void Add (GameObject obj, ObjectProperties item)
        {
            GroupData group = new GroupData();

            group.commandNum = item.commandNum;
            group.myType = item.myType;
            group.p0 = item.p0;
            group.p1 = item.p1;
            group.p2 = item.p2;
            group.lookAtPoint = item.lookAtPoint;

            group.spacing = item.spacing;
            group.buildingID = item.buildingID;
            group.rotateTo = item.rotateTo;
            group.rotationCounter = item.rotationCounter;

            group.position = obj.transform.TransformPoint(item.GetComponent<ObjectProperties>().p1);

            mapData.Add(group);
        }
    }

}
