using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

public class SerializationOverview : MonoBehaviour
{
    public enum Docent
    {
        VALENTIJN,
        AARON,
        TON,
        VINCENT
    }

    // Promises that a class type can be serialized (it still might not be)
    [System.Serializable]
    public class MyDataType
    {
        public string name = "Aaron";
        public Docent docent = Docent.AARON;
        public double ageInMilliseconds;

        [OptionalField(VersionAdded = 2)]
        public string middleName = "John";          // defaults for XmlSerializer
        [OptionalField(VersionAdded = 3)]
        public string lastName = "Oostdijk";        // defaults for XmlSerializer

        // Defaults for BinaryFormatter
        [OnDeserializing()]
        void OnDeserializing(StreamingContext context)
        {
            middleName = "John";
            lastName = "Oostdijk";
        }
    }

    public MyDataType someData = new MyDataType();

    void Start()
    {
        // Fill data with values
        System.DateTime birthday = new System.DateTime(1986, 2, 8);
        someData.ageInMilliseconds = (System.DateTime.Now - birthday).TotalMilliseconds;

        // Get my formatters ready

        // Binary -> raw bytes, with some structure information (text) about the Assembly
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        // XML -> stores human-readable XML, can be edited more easily, also a bit bigger (text padding)
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(MyDataType));

        // FileStream is handy when using formatters, because it expects raw bytes
        FileStream fStream;


        // Try-catch to capture errors, which are expected when using streams
        try
        {
            string fileName = "file.txt";
            string path = Application.streamingAssetsPath; //-> Editor-time save folder, files are in build "as-is", no compiling
                                                           // Other folders
                                                           //  Application.dataPath              -> Assets folder, useful for creating prefabs etc. (editor only)
                                                           //  Application.persistentDataPath;   -> Runtime save folder (different per platform)

            string url = Path.Combine(path, fileName);
            fStream = new FileStream(url, FileMode.Create);

            binaryFormatter.Serialize(fStream, someData);
            //xmlSerializer.Serialize(fStream, someData);

            fStream.Flush();
            fStream.Close();
            fStream.Dispose();
        }
        catch (FileNotFoundException e)
        {
            Debug.Log(e.Message);
        }

        // Read the written data back
        MyDataType someOtherData = null;
        try
        {
            string fileName = "file.txt";
            string path = Application.streamingAssetsPath; //-> Editor-time save folder, files are in build "as-is", no compiling
                                                           // Other folders
                                                           //  Application.dataPath              -> Assets folder, useful for creating prefabs etc. (editor only)
                                                           //  Application.persistentDataPath;   -> Runtime save folder (different per platform)

            string url = Path.Combine(path, fileName);

            fStream = new FileStream(url, FileMode.Open);
            someData = (MyDataType)binaryFormatter.Deserialize(fStream);
            //someData = (MyDataType)xmlSerializer.Deserialize(fStream);
            fStream.Close();
            fStream.Dispose();
        }
        catch (FileNotFoundException e)
        {
            Debug.Log(e.Message);
        }
        catch (SerializationException e)
        {
            Debug.Log(e.Message);
        }

        // You can also use more specialized classes to do things like write text

        // Using statement calls "Dispose" at the end-of-scope
        //  You could wrap this in a try/catch as well in case the new() fails
        using (StreamWriter sWriter = new StreamWriter("textfile.txt", false))
        {
            //sWriter.WriteLine("abcd!");
            //sWriter.WriteLine("abcd!2");

            sWriter.WriteLine(JsonUtility.ToJson(someOtherData));

            sWriter.Flush();
            sWriter.Close();
            sWriter.Dispose();
        }

        // Read it back
        StreamReader sReader = null;
        try
        {
            sReader = new StreamReader("textfile.txt");

            while (!sReader.EndOfStream)
            {
                string s = sReader.ReadLine();
                Debug.Log(s);
            }

            sReader.Close();
            sReader.Dispose();
        }
        catch (System.Exception e)
        {
            // New formatting in C#
            Debug.Log($"Epic Fail: {e.Message}");
        }

        // But even using FileStream you can do it manually:
        FileStream customFStream = new FileStream("custom.txt", FileMode.Create);
        byte[] bytes = new byte[4];
        bytes[0] = 97;      //ASCII 'a'
        bytes[1] = 98;      //ASCII 'b'
        bytes[2] = 99;      //ASCII 'c'
        bytes[3] = 100;     //ASCII 'd'

        // Write the bytes, from the start of the list (0), and write 4 of them
        customFStream.Write(bytes, 0, 4);

        customFStream.Flush();
        customFStream.Close();
        customFStream.Dispose();
    }
}