using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SerializationTest : NetworkBehaviour
{
    //[SyncVar]
    public int int1 = 66;

    //[SyncVar]
    public int int2 = 23487;

    //[SyncVar]
    public string MyString = "Example string";

    public CustomClass customClass;

    public NormalClass normalClass;

    public List<ExtraClass> extraClass;

    // Start is called before the first frame update
    void Awake()
    {
        this.customClass = new CustomClass();
        //normalClass = GetComponentInChildren<NormalClass>();
        //extraClass = GetComponentInChildren<ExtraClass>();
        this.normalClass = new NormalClass();
        //this.extraClass = new ExtraClass();
        this.extraClass = new List<ExtraClass>();
        for (int i = 0; i < 5; i++)
        {
            ExtraClass e = new ExtraClass();
            e.int1 = 100 + i;
            e.int2 = 100 - i;
            this.extraClass.Add(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            int1++;
            int2--;
            MyString += "+";
            //customClass.UpdateData();
            this.normalClass.UpdateData();
            //this.extraClass.UpdateData();
            foreach(ExtraClass e in this.extraClass)
            {
                e.UpdateData();
            }
            SetDirtyBit(15);
        }
    }

    public override bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
        Debug.Log("OnSerialize");
        if (forceAll)
        {
            // The first time a GameObject is sent to a client, send all the data (and no dirty bits)
            writer.WritePackedUInt32((uint)this.int1);
            writer.WritePackedUInt32((uint)this.int2);
            writer.Write(this.MyString);
            //writer.Write(this.customClass);
            //this.customClass.Serialize(writer);
            //writer.Write(this.normalClass); //Doesn't work.
            this.normalClass.Serialize(writer);
            //this.extraClass.Serialize(writer);
            foreach (ExtraClass e in extraClass)
            {
                e.Serialize(writer);
            }
            return true;
        }
        bool wroteSyncVar = false;
        if ((base.syncVarDirtyBits & 1u) != 0u)
        {
            if (!wroteSyncVar)
            {
                // Write dirty bits if this is the first SyncVar written
                writer.WritePackedUInt32(base.syncVarDirtyBits);
                wroteSyncVar = true;
            }
            writer.WritePackedUInt32((uint)this.int1);
        }
        if ((base.syncVarDirtyBits & 2u) != 0u)
        {
            if (!wroteSyncVar)
            {
                // Write dirty bits if this is the first SyncVar written
                writer.WritePackedUInt32(base.syncVarDirtyBits);
                wroteSyncVar = true;
            }
            writer.WritePackedUInt32((uint)this.int2);
        }
        if ((base.syncVarDirtyBits & 4u) != 0u)
        {
            if (!wroteSyncVar)
            {
                // Write dirty bits if this is the first SyncVar written
                writer.WritePackedUInt32(base.syncVarDirtyBits);
                wroteSyncVar = true;
            }
            writer.Write(this.MyString);
        }
        if ((base.syncVarDirtyBits & 8u) != 0u)
        {
            if (!wroteSyncVar)
            {
                // Write dirty bits if this is the first SyncVar written
                writer.WritePackedUInt32(base.syncVarDirtyBits);
                wroteSyncVar = true;
            }
            //writer.Write(this.customClass);
            //this.customClass.Serialize(writer);
            //writer.Write(this.normalClass); //Doesn't work.
            this.normalClass.Serialize(writer);
            //this.extraClass.Serialize(writer);
            foreach (ExtraClass e in extraClass)
            {
                e.Serialize(writer);
            }
        }

        if (!wroteSyncVar)
        {
            // Write zero dirty bits if no SyncVars were written
            writer.WritePackedUInt32(0);
        }
        return wroteSyncVar;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        Debug.Log("OnDeserialize");
        if (initialState)
        {
            this.int1 = (int)reader.ReadPackedUInt32();
            this.int2 = (int)reader.ReadPackedUInt32();
            this.MyString = reader.ReadString();
            //this.customClass = reader.ReadMessage<CustomClass>();
            //this.customClass.Deserialize(reader);
            this.normalClass.Deserialize(reader);
            //this.extraClass.Deserialize(reader);
            foreach (ExtraClass e in extraClass)
            {
                e.Deserialize(reader);
            }
            return;
        }
        int num = (int)reader.ReadPackedUInt32();
        if ((num & 1) != 0)
        {
            this.int1 = (int)reader.ReadPackedUInt32();
        }
        if ((num & 2) != 0)
        {
            this.int2 = (int)reader.ReadPackedUInt32();
        }
        if ((num & 4) != 0)
        {
            this.MyString = reader.ReadString();
        }
        if ((num & 8) != 0)
        {
            //this.customClass = reader.ReadMessage<CustomClass>();
            //this.customClass.Deserialize(reader);
            this.normalClass.Deserialize(reader);
            //this.extraClass.Deserialize(reader);
            foreach (ExtraClass e in extraClass)
            {
                e.Deserialize(reader);
            }
        }
        //Debug.Log(customClass.int1 + ", " + customClass.int2 + ", " + customClass.string1 + ", " + customClass.bool1);
        Debug.Log("normalClass: " + this.normalClass.int1 + ", " + this.normalClass.int2 + ", " + this.normalClass.str);
        //Debug.Log("extraClass: " + this.extraClass.int1 + ", " + this.extraClass.int2 + ", " + this.extraClass.str);
        foreach (ExtraClass e in extraClass)
        {
            Debug.Log("extraClass: " +e.int1 + ", " +e.int2 + ", " + e.str);
        }
    }
}



