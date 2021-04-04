using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.Reflection;


public class GenerateCode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Generate("SomeNamespace", "Someclass");
    }

    void Generate(string nameSpace, string className)
    {
        StringBuilder sb = new StringBuilder();

        //write file
        Include(sb);
        NamespaceOpen(sb, nameSpace);
        Class(sb, className);
        NamespaceClose(sb);

        //send file to disk
        //Debug.Log(sb.ToString());
        StreamWriter sw = new StreamWriter(Path.Combine(Application.dataPath, "Scripts/Generated/" + className + ".cs"));
        sw.Write(sb);
        sw.Flush();
        sw.Close();

        AssetDatabase.Refresh();
    }

    void Include(StringBuilder sb)
    {
        sb.AppendLine("using UnityEngine;");
    }

    void NamespaceOpen(StringBuilder sb, string nameSpace)
    {
        sb.AppendLine("namespace " + nameSpace + " {");
    }

    void Class(StringBuilder sb, string className )
    {
        sb.AppendLine("public interface " + className + "{");
        // dit is waar de complexiteit zit...

        MethodInfo[] methods = typeof(Interface).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        Dictionary<System.Type, string> types = new Dictionary<System.Type, string>();

        foreach (MethodInfo info in methods)
        {
            Debug.Log(info.DeclaringType);

            //if (info.GetType() == typeof(void))
            //{
            //    sb.AppendLine("void ");
            //    sb.Append(info.Name + "();");
            //}
            //else
            //{
            //    sb.Append(info + ";");

            //}
        }

        sb.AppendLine("\t}");
    }

    void NamespaceClose(StringBuilder sb)
    {
        sb.AppendLine("}");
    }
}