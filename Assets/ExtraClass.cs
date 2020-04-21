using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExtraClass //: MonoBehaviour
{
    public int int1 = 100;
    public int int2 = 100;
    public string str = "String";

    public void UpdateData()
    {
        int1++;
        int2--;
        str += "-";
    }

    public void Serialize(NetworkWriter writer)
    {
        writer.Write(int1);
        writer.Write(int2);
        writer.Write(str);
    }

    public void Deserialize(NetworkReader reader)
    {
        int1 = reader.ReadInt32();
        int2 = reader.ReadInt32();
        str = reader.ReadString();
    }
}
