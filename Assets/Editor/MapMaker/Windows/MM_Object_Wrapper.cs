using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProductionTools
{
    public class MM_Object_Wrapper : ScriptableObject
    {
        public List<GameObject> sources = new List<GameObject>();
        public List<string> paths = new List<string>();
    }
}

