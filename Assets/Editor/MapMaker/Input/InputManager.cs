using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProductionTools
{
    [ExecuteInEditMode]
    public enum MouseState
    {
        NONE,
        UP,
        DRAGGED,
        DOWN
    }

    public class InputManager
    {
        public bool onHover;

        public MouseState state = MouseState.NONE;

        public IActionInput currentAction;
        public MapMaker owner;

        public InputManager(MapMaker owner)
        {
            this.owner = owner;
        }
        public void HandleEvent(Event currEvent)
        {
            //Disable selection in SceneView
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));

            if (!currEvent.alt)
            {
                switch (state)
                {
                    case MouseState.NONE:
                        {
                            if (onHover == true)
                            {
                                if (currEvent.type == EventType.MouseDown && currEvent.button == 0)
                                {
                                    SelectObject();
                                    state = MouseState.NONE;//Reset

                                }
                            }
                            else
                            {
                                if (currEvent.type == EventType.MouseDown && currEvent.button == 0)
                                {
                                    if (currentAction != null)
                                    {
                                        currentAction.MouseOneDown();
                                        state = MouseState.DOWN;
                                    }
                                }
                            }
                        }
                        break;
                    case MouseState.DOWN:
                        {
                            if (currEvent.type == EventType.MouseDrag && currEvent.button == 0)
                            {
                                if (currentAction != null)
                                {
                                    currentAction.MouseOneDrag();
                                    state = MouseState.DRAGGED;
                                }
                            }
                            else
                            {
                                state = MouseState.DRAGGED;
                            }
                        }
                        break;
                    case MouseState.DRAGGED:
                        {
                            if (currEvent.type == EventType.MouseUp && currEvent.button == 0)
                            {
                                if (currentAction != null)
                                {
                                    currentAction.MouseOneUp();
                                    state = MouseState.NONE;
                                }
                            }
                        }
                        break;
                    case MouseState.UP:
                        {
                            state = MouseState.NONE;
                        }
                        break;
                }
            }
          
        }
      
        public void SelectObject()
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    if(hit.collider.tag == "SelectableObject")
                    {
                        Selection.activeObject = hit.transform.root;
                    }
                }
            }
        }

    }

   

   
    public class PlaceObject : IActionInput
    {
        MapMaker owner;
        public ObjectType myType { get; set; }

        public string name { get; set; }
        public string actionInfo { get { return "Place a ObjectProperties  of selected objects"; } }


        Vector3 positionA = new Vector3();
        Vector3 positionB = new Vector3();
        Vector3 positionC = new Vector3();

        CreateObjectCommand newObject;
        public ActionSettings settings { get; set; }
        public SerializedObject so { get; set; }

        public PlaceObject(MapMaker owner, ObjectType myType)
        {
            this.owner = owner;

            //settings = ScriptableObject.CreateInstance<ActionSettings>();
            settings = owner.globalSettings;
            //settings.Init(owner);
            so = new SerializedObject(settings);
            this.myType = myType;

            name = myType.ToString();

        }
        public void MouseOneDown()
        {
            positionA = owner.RayPosition();
        }

        public void MouseOneDrag()
        {
            //null
        }

        public void MouseOneUp()
        {
            positionC = owner.RayPosition();
            positionB = Vector3.Lerp(positionA, positionC, 0.5f);

            if (owner.currentObject == null)
            {
                //newObject = new CreateObjectCommand(owner.currentProject.myObjectPool.objectList[owner.sourceKey], positionB, positionA, positionB, positionC, settings.lookAtPoint, settings.spacing, owner.sourceKey, (int)myType, settings.rotateTo, settings.rotationCounter, owner);
                //owner.AddCommand(newObject);
                //Execute();
            }

            if(Vector3.Distance(positionA,positionC) > 1)
            {
                newObject = new CreateObjectCommand(owner.currentProject.myObjectPool.objectList[owner.sourceKey], positionB, positionA, positionB, positionC, settings.lookAtPoint, settings.spacing, owner.sourceKey, (int)myType, settings.rotateTo, settings.rotationCounter, owner);
                owner.AddCommand(newObject);
                Execute();
            }
            
            so.Update();
            owner.Repaint();

        }
        public void MouseTwoDown()
        {
            //owner.RemoveCommand();
            //owner.currentObject.Undo();
        }
        public void Execute()
        {
            //positionA = Selection.activeGameObject.GetComponent<ObjectProperties>().p0;
            //positionB = Selection.activeGameObject.GetComponent<ObjectProperties>().p1;
            //positionC = Selection.activeGameObject.GetComponent<ObjectProperties>().p2;

            so.Update();
            owner.currentObject = newObject;

            owner.currentObject.myType = myType;
            owner.currentObject.posA = newObject.GameObjectInstance.GetComponent<ObjectProperties>().p0;
            owner.currentObject.posB = newObject.GameObjectInstance.GetComponent<ObjectProperties>().p1;
            owner.currentObject.posC = newObject.GameObjectInstance.GetComponent<ObjectProperties>().p2;

            owner.currentObject.rotateTo = settings.rotateTo;
            owner.currentObject.lookAtPoint = newObject.GameObjectInstance.GetComponent<ObjectProperties>().lookAtPoint;
            owner.currentObject.rotationCounter = settings.rotationCounter;
            owner.currentObject.spacing = settings.spacing;

            //SetLineInfo
            newObject.GameObjectInstance.GetComponent<ObjectProperties>().spacing = owner.currentObject.spacing;
            newObject.GameObjectInstance.GetComponent<ObjectProperties>().buildingID = owner.sourceKey;
            newObject.GameObjectInstance.GetComponent<ObjectProperties>().rotateTo = owner.currentObject.rotateTo;
            newObject.GameObjectInstance.GetComponent<ObjectProperties>().rotationCounter = owner.currentObject.rotationCounter;

            owner.currentObject.Update();
            owner.currentObject = null;

        }
        public void Update()
        {
            so.Update();

            if (owner.currentObject != null && Selection.activeGameObject != null)
            {
                if (Selection.activeGameObject.GetComponent<ObjectProperties>() != null)
                {
                    owner.currentObject.posA = Selection.activeGameObject.GetComponent<ObjectProperties>().p0;
                    owner.currentObject.posB = Selection.activeGameObject.GetComponent<ObjectProperties>().p1;
                    owner.currentObject.posC = Selection.activeGameObject.GetComponent<ObjectProperties>().p2;
                    owner.currentObject.lookAtPoint = Selection.activeGameObject.GetComponent<ObjectProperties>().lookAtPoint;

                    owner.currentObject.myType = settings.myType;
                    owner.currentObject.buildingID = settings.buildingID;
                    owner.currentObject.spacing = settings.spacing;
                    owner.currentObject.rotateTo = settings.rotateTo;
                    owner.currentObject.rotationCounter = settings.rotationCounter;



                    //SetObjectDataInfo
                    //settings.Update();
                    Selection.activeGameObject.GetComponent<ObjectProperties>().myType = (int)owner.currentObject.myType;
                    Selection.activeGameObject.GetComponent<ObjectProperties>().spacing = owner.currentObject.spacing;
                    Selection.activeGameObject.GetComponent<ObjectProperties>().rotateTo = owner.currentObject.rotateTo;
                    Selection.activeGameObject.GetComponent<ObjectProperties>().rotationCounter = owner.currentObject.rotationCounter;

                    //Selection.activeGameObject.GetComponent<ObjectProperties>().buildingID = owner.sourceKey;


                    owner.currentObject.Update();
                }

            }
        }
    }


}

