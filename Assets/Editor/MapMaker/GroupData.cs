using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class GroupData
{
    /// <summary>
    /// Wrapper for ObjectProperties 
    /// </summary>
    /// 
    public int commandNum;
    public int myType;

    public string mapName;
    public string folderPath;

    //Binary
    public float pos0x;
    public float pos0y;
    public float pos0z;

    public float pos1x;
    public float pos1y;
    public float pos1z;

    public float pos2x;
    public float pos2y;
    public float pos2z;

    public float lookposx;
    public float lookposy;
    public float lookposz;

    public float posx;
    public float posy;
    public float posz;

    //set in SceneGUIHandler
    //public Vector3 p0;
    //public Vector3 p1;
    //public Vector3 p2;
    //public Vector3 lookAtPoint;
    //public Vector3 position;

    //set in InputManager
    public float spacing;
    public int buildingID;
    public bool rotateTo;
    public int rotationCounter;


}
