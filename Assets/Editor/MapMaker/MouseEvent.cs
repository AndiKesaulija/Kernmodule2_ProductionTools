using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProductionTools
{
    [CustomEditor(typeof(MapMaker))]
    public class MouseEvent : Editor
    {
        private void OnSceneGUI()
        {
            Debug.Log("MOUSEEVENT TEST");

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(SceneView.lastActiveSceneView.camera.transform.position, Event.current.mousePosition, out hit, Mathf.Infinity))
            {
                Debug.Log($"Did Hit: {hit.transform.tag}");
            }
        }
    }
}

