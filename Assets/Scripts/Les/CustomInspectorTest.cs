using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInspectorTest : MonoBehaviour
{
    public float testFloat = 1f;

    public void OnValidate()
    {
        //Debug.Log("Test");
        //Scale();
    }
    public void Scale()
    {
        transform.localScale = new Vector3(testFloat, testFloat, testFloat);
    }
    public void ResetFloat()
    {
        testFloat = 1;
    }

    

    
}
