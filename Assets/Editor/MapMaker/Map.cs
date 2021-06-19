using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;//Maybe
using System.Xml.Serialization;

namespace ProductionTools
{
    public class Map : INamed
    {
        public string name { get; set; }
        public MapData myMapData;
       
        private string fileName;
        public  string folderPath;
        private string url;


        public Map(string mapName, string folderPath)
        {
            name = mapName;
            myMapData = new MapData();
            this.folderPath = folderPath;
            fileName = mapName;


            if (File.Exists(Path.Combine(folderPath, fileName)) == true)
            {
                Debug.Log("File Exists");
            }

            url = Path.Combine(folderPath, fileName);

        }

        public void WriteMap()
        {
            Debug.Log("Buildings in Map: " + myMapData.mapData.Count);

            BinaryFormatter bFormatter = new BinaryFormatter();
            FileStream fStream;

            try
            {
                if (Directory.Exists(folderPath) == true)
                {
                    fStream = new FileStream(url + ".txt", FileMode.Create);

                    bFormatter.Serialize(fStream, myMapData);

                    fStream.Flush();
                    fStream.Close();

                    Debug.Log("MapSaved: " + url);
                }
                else
                {
                    Debug.Log("NoStreamingAssets");
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }

            AssetDatabase.Refresh();
        }
        public void ReadMap()
        {
            BinaryFormatter bFormatter = new BinaryFormatter();
            FileStream binReader;

            try
            {
                binReader = new FileStream(url,FileMode.Open);
                    
                myMapData = (MapData)bFormatter.Deserialize(binReader);

                binReader.Close();
            }
            catch (FileNotFoundException e)
            {
                Debug.Log("FileNotFound: " + e);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            Debug.Log(myMapData.mapData.Count);
        }
    }
}