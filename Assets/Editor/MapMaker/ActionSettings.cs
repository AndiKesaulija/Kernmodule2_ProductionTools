using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace ProductionTools
{
    public class ActionSettings : ScriptableObject
    {
        public MapMaker owner;
        public CreateObjectCommand currCommand;

        public ObjectType myType;
        public int buildingID;
        [Range(1f, 10f)]
        public float spacing = 1;
        public bool rotateTo;
        public Vector3 lookAtPoint;
        [Range(0, 4)]
        public int rotationCounter;
        
        public void Init(MapMaker owner)
        {
            this.owner = owner;
        }
        public void Update()
        {
            myType = (ObjectType)Selection.activeGameObject.GetComponent<ObjectProperties>().myType;
            owner.setAction(owner.actions, (int)myType);

            //Set Active Building
            owner.SetActiveObject(Selection.activeGameObject.GetComponent<ObjectProperties>().buildingID);
            buildingID = Selection.activeGameObject.GetComponent<ObjectProperties>().buildingID;

            spacing = Selection.activeGameObject.GetComponent<ObjectProperties>().spacing;
            rotateTo = Selection.activeGameObject.GetComponent<ObjectProperties>().rotateTo;
            lookAtPoint = Selection.activeGameObject.GetComponent<ObjectProperties>().lookAtPoint;
            rotationCounter = Selection.activeGameObject.GetComponent<ObjectProperties>().rotationCounter;

        }

    }
}

