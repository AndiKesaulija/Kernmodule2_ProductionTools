using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProductionTools
{
    class MyAssetPostprocessor : AssetPostprocessor
    {
        public static MapMaker owner;

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            AssetDatabase.Refresh();
            if(owner != null)
            {
                owner.ReloadMapMaker();
            }
            else
            {
                Debug.Log("MapMaker not active");
            }
        }
    }
}

