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
            GroupData test = new GroupData();

            test.commandNum = item.commandNum;
            test.myType = item.myType;
            test.p0 = item.p0;
            test.p1 = item.p1;
            test.p2 = item.p2;
            test.lookAtPoint = item.lookAtPoint;

            test.spacing = item.spacing;
            test.buildingID = item.buildingID;
            test.rotateTo = item.rotateTo;
            test.rotationCounter = item.rotationCounter;

            test.position = obj.transform.TransformPoint(item.GetComponent<ObjectProperties>().p1);

            mapData.Add(test);
        }
    }

}
