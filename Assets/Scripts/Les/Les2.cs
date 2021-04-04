using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

public class Les2 : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        //ExampleClass test = new ExampleClass();
        //FieldInfo[] fields = typeof(ExampleClass).GetFields(BindingFlags.Public);

        //Type mytype = typeof(ExampleClass);
        //MethodInfo[] myArrayMethodInfo = mytype.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly);

        //foreach(MethodInfo info in myArrayMethodInfo)
        //{
        //    Debug
        //}

        //MethodInfo[] methods = typeof(ExampleClass).GetMethods(BindingFlags.Public);

        //Debug.Log(methods.Length);

        //typeof(ExampleClass).GetMethod("Prepare").Invoke(test, null);


        MethodInfo[] methods = typeof(ExampleClass).GetMethods(BindingFlags.Public |BindingFlags.Instance| BindingFlags.DeclaredOnly);

        foreach(MethodInfo method in methods)
        {
            //Debug.Log(method.Name);

            if(method.GetParameters().Length == 0)
            {
                //Debug.Log("With Parameter: " + method.Name);
            }
        }

        ExampleClass test = new ExampleClass();

        MethodInfo getSecret = typeof(ExampleClass).GetMethod("GetSecret", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Debug.Log(getSecret.Invoke(test, null));

        MethodInfo setSecret = typeof(ExampleClass).GetMethod("SetSecret", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        setSecret.Invoke(test, new object[] {"new Secret"});//Edit Secret

        Debug.Log(getSecret.Invoke(test, null));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
