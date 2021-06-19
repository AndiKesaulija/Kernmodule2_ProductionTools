using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProductionTools
{
    [Serializable]
    public class MapData
    {
        public string mapName;
        public string folderpath;
        public string fileName;

        public List<GroupData> mapData =  new List<GroupData>();

        public void Add (GameObject obj, ObjectProperties item)
        {
            GroupData group = new GroupData();

            group.commandNum = item.commandNum;
            group.myType = item.myType;

            //group.p0 = item.p0;
            //group.p1 = item.p1;
            //group.p2 = item.p2;
            //group.lookAtPoint = item.lookAtPoint;

            //group.position = obj.transform.TransformPoint(item.GetComponent<ObjectProperties>().p1);

            //Binary
            group.pos0x = item.p0.x;
            group.pos0y = item.p0.y;
            group.pos0z = item.p0.z;

            group.pos1x = item.p1.x;
            group.pos1y = item.p1.y;
            group.pos1z = item.p1.z;

            group.pos2x = item.p2.x;
            group.pos2y = item.p2.y;
            group.pos2z = item.p2.z;

            group.lookposx = item.lookAtPoint.x;
            group.lookposy = item.lookAtPoint.y;
            group.lookposz = item.lookAtPoint.z;

            group.posx = obj.transform.TransformPoint(item.GetComponent<ObjectProperties>().p1).x;
            group.posy = obj.transform.TransformPoint(item.GetComponent<ObjectProperties>().p1).y;
            group.posz = obj.transform.TransformPoint(item.GetComponent<ObjectProperties>().p1).z;

            group.spacing = item.spacing;
            group.buildingID = item.buildingID;
            group.rotateTo = item.rotateTo;
            group.rotationCounter = item.rotationCounter;


            mapData.Add(group);
        }
    }

}
