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

    //set in SceneGUIHandler
    public Vector3 p0;
    public Vector3 p1;
    public Vector3 p2;
    public Vector3 lookAtPoint;

    //set in InputManager
    public float spacing;
    public int buildingID;
    public bool rotateTo;
    public int rotationCounter;

    public Vector3 position;

}
