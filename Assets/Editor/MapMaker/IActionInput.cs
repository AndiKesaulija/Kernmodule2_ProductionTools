using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace ProductionTools
{
    public enum ObjectType 
    {   
        single = 0,
        line = 1,
        curve = 2  
    }
    public interface IActionInput
    {
        ObjectType myType { get; set; }
        string name { get; }
        string actionInfo { get; }

        ActionSettings settings { get; set; }

        SerializedObject so { get; set; }
        void MouseOneDown();
        void MouseOneDrag();
        void MouseOneUp();
        void MouseTwoDown();
        void Execute();
        void Update();

    }


}