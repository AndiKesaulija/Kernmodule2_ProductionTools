using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

namespace ProductionTools
{
    [EditorTool("Selection Tool")]
    public class SelectionTool : EditorTool
    {

        public override void OnToolGUI(EditorWindow window)
        {
            EditorGUI.BeginChangeCheck();

            GameObject selection = Selection.activeGameObject;
            Vector3 position = Tools.handlePosition;
            Quaternion rotation = Tools.handleRotation;

            if (selection != null)
            {
                if (selection.transform.parent != null)
                {
                    Selection.activeGameObject = selection.transform.parent.gameObject;
                }

                
                //using (new Handles.DrawingScope(Color.green))
                //{
                //    //position = Handles.PositionHandle(selection.transform.position, selection.transform.rotation);

                //    position.x = Handles.Slider(selection.transform.position, Vector3.right).x;
                //    position.z = Handles.Slider(selection.transform.position, Vector3.forward).z;

                //    rotation = Handles.RotationHandle(selection.transform.rotation, selection.transform.position);

                //    rotation.w = Handles.RadiusHandle(selection.transform.rotation, selection.transform.position, 20);
                //}


            }



            if (EditorGUI.EndChangeCheck())
            {
                Vector3 delta = position - Tools.handlePosition;

                Undo.RecordObjects(Selection.transforms, "Move Platform");

                foreach (var transform in Selection.transforms)
                {
                    transform.position += delta;
                    transform.rotation = rotation;
                }

            }



        }

        public Vector3 Ray()
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Plane hPlane = new Plane(Vector3.up, Vector3.zero);

            float distance = 0;

            if (hPlane.Raycast(ray, out distance))
            {
                return ray.GetPoint(distance);
            }

            return new Vector3(0, 0, 0);
        }

    }


}
