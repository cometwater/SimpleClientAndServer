using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomClass : MessageBase
{
    public int int1 = 0;
    public int int2 = 10000;
    public bool bool1 = true;
    public string string1 = "String";

    public void UpdateData()
    {
        int1++;
        int2--;
        bool1 = !bool1;
        string1 += "+";
    }
}
