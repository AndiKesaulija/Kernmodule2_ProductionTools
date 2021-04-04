using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEditor;

public class CodeGeneration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Generate("SomeNamespace", "SomeClass");
    }

    void Generate(string nameSpace, string className)
    {
        StringBuilder sb = new StringBuilder();

        //write file
        Include(sb);
        NamespaceOpen(sb, nameSpace);
        Class(sb, className);
        NamespaceClose(sb);

        //Send file to disk
        //Debug.Log(sb.ToString());
        StreamWriter sw = new StreamWriter(Path.Combine(Application.dataPath, "Scripts/Generated/ + className + " + ".cs"));
        sw.Write(sb);
        sw.Flush();
        sw.Close();

        AssetDatabase.Refresh();


    }

    void Include(StringBuilder sb)
    {
        //Niks
        sb.AppendLine("using UnityEngine;");
    }
    void NamespaceOpen(StringBuilder sb, string nameSpace)
    {
        sb.AppendLine("namespace" + nameSpace + "{");
    }
    void Class(StringBuilder sb, string className)
    {
        sb.AppendLine("\tpublic class " + className + "{");

        sb.AppendLine("\t");


    }
    void NamespaceClose(StringBuilder sb)
    {
        sb.AppendLine("]");

    }
}
