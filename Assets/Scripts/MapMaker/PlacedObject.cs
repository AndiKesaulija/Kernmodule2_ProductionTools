using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PlacedObject : MonoBehaviour
{

    public void GetParent()
    {
        Selection.activeObject = this.transform.parent.gameObject;
    }
}
