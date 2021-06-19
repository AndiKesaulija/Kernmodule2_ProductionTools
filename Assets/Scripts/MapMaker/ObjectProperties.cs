using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProductionTools
{
    public class ObjectProperties : MonoBehaviour
    {
        public int commandNum;
        public int myType;

        //set in SceneGUIHandler
        public Vector3 p0;
        public Vector3 p1;
        public Vector3 p2;
        public Vector3 lookAtPoint;

        //set in InputManager
        public float spacing = 1;
        public int buildingID;
        public bool rotateTo;
        public int rotationCounter;

        public void Highlight()
        {
            //get children (placed Object)
            List<GameObject> children = new List<GameObject>();

            foreach( Transform child in transform)
            {
                children.Add(child.gameObject);
            }

            //MakeGizmo of highlight
            //EnableGizmo
        }



    }

}

