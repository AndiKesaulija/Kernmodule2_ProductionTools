﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace ProductionTools
{
    public class SceneGuiHandler : Editor
    {
        public MapMaker owner;

        public List<ObjectProperties> ObjectProperties = new List<ObjectProperties>();

        public Transform handleTransform;
        public Quaternion handleRotation;
        public ObjectProperties currentObject;

        Vector3 p0;
        Vector3 p1;
        Vector3 p2;

        Vector3 lookAtPoint;

        


        public void OnEnable()
        {
            ObjectProperties.Clear();

        }
     
        public void OnSceneGUI()
        {
            if (Selection.activeObject != null)
            {
                
                try
                {
                    if (Selection.activeGameObject.GetComponent<ObjectProperties>() != null)
                    {

                        owner.newselection = Selection.activeObject;
                        if (owner.selection != owner.newselection)
                        {
                            //Debug.Log("new Object Selected");
                            owner.myInputManager.currentAction.settings.Update();
                            owner.Repaint();
                            owner.selection = owner.newselection;

                        }

                        owner.currentObject = owner.allObjects[Selection.activeGameObject.GetComponent<ObjectProperties>().commandNum - 1];
                        currentObject = Selection.activeGameObject.GetComponent<ObjectProperties>();

                        if (currentObject != null)
                        {
                            
                            if (owner.myInputManager.currentAction.myType == ObjectType.single)
                            {
                                HandleSingle();
                            }
                            if (owner.myInputManager.currentAction.myType == ObjectType.line)
                            {
                                HandleLine();

                                //DisableTool
                                if (owner.myInputManager.currentAction != null)
                                {
                                    Tools.current = Tool.None;
                                }
                            }
                            if (owner.myInputManager.currentAction.myType == ObjectType.curve)
                            {
                                HandleCurve();

                                //DisableTool
                                if (owner.myInputManager.currentAction != null)
                                {
                                    Tools.current = Tool.None;
                                }
                            }
                            
                           
                        }
                    }
                    else
                    {

                        owner.currentObject = null;
                    }
                }
                catch(Exception e)
                {
                    //Debug.Log(e);
                }
                
            }
            else
            {
                currentObject = null;
                owner.currentObject = null;
            }

        }
        void HandleSingle()
        {
            handleTransform = currentObject.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

            p0 = handleTransform.TransformPoint(currentObject.p0);
            p1 = currentObject.transform.position;
            //p1 = handleTransform.TransformPoint(currentObject.p1);
            p2 = handleTransform.TransformPoint(currentObject.p2);
            lookAtPoint = handleTransform.TransformPoint(currentObject.lookAtPoint);
          
        }
        void HandleLine()
        {

            handleTransform = currentObject.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

            p0 = handleTransform.TransformPoint(currentObject.p0);
            p1 = currentObject.transform.position;
            p2 = handleTransform.TransformPoint(currentObject.p2);
            lookAtPoint = handleTransform.TransformPoint(currentObject.lookAtPoint);

            Handles.color = Color.red;
            Handles.DrawLine(p0, p2);

            EditorGUI.BeginChangeCheck();
            p0 = Handles.DoPositionHandle(p0, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                currentObject.p0 = handleTransform.InverseTransformPoint(p0);
                owner.myInputManager.currentAction.Update();
            }
            EditorGUI.BeginChangeCheck();
            p2 = Handles.DoPositionHandle(p2, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                currentObject.p2 = handleTransform.InverseTransformPoint(p2);
                owner.myInputManager.currentAction.Update();

            }

            if (currentObject.rotateTo == true)
            {
                Handles.color = Color.white;
                Handles.DrawLine(p0, lookAtPoint);
                Handles.DrawLine(p2, lookAtPoint);

                EditorGUI.BeginChangeCheck();
                lookAtPoint = Handles.DoPositionHandle(lookAtPoint, handleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    currentObject.lookAtPoint = handleTransform.InverseTransformPoint(lookAtPoint);
                    owner.myInputManager.currentAction.Update();
                }
            }
        }
        void HandleCurve()
        {
            handleTransform = currentObject.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

            p0 = handleTransform.TransformPoint(currentObject.p0);
            //p1 = currentObject.transform.position;
            p1 = handleTransform.TransformPoint(currentObject.p1);
            p2 = handleTransform.TransformPoint(currentObject.p2);
            lookAtPoint = handleTransform.TransformPoint(currentObject.lookAtPoint);



            Handles.color = Color.red;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p1, p2);

            EditorGUI.BeginChangeCheck();
            p0 = Handles.DoPositionHandle(p0, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                currentObject.p0 = handleTransform.InverseTransformPoint(p0);
                owner.myInputManager.currentAction.Update();
            }

            EditorGUI.BeginChangeCheck();
            p1 = Handles.DoPositionHandle(p1, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                currentObject.p1 = handleTransform.InverseTransformPoint(p1);
                owner.myInputManager.currentAction.Update();

            }

            EditorGUI.BeginChangeCheck();
            p2 = Handles.DoPositionHandle(p2, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                currentObject.p2 = handleTransform.InverseTransformPoint(p2);
                owner.myInputManager.currentAction.Update();

            }

            if (currentObject.rotateTo == true)
            {
                Handles.color = Color.white;
                Handles.DrawLine(p0, lookAtPoint);
                Handles.DrawLine(p1, lookAtPoint);
                Handles.DrawLine(p2, lookAtPoint);

                EditorGUI.BeginChangeCheck();
                lookAtPoint = Handles.DoPositionHandle(lookAtPoint, handleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    currentObject.lookAtPoint = handleTransform.InverseTransformPoint(lookAtPoint);
                    owner.myInputManager.currentAction.Update();
                }
            }
        }
    }

    

    
}