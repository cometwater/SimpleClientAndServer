using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{

    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = gameObject.name;

        if (isLocalPlayer)
        {
            Debug.Log("CmdSendPos is called from " + netId + " in Start()");
            CmdSendPos(new Vector2(netId.Value * 2f, netId.Value));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdSendName("Banana" + netId);
            CmdSendColor();
        }

        if (Input.GetKeyDown(KeyCode.P) && !isServer)
        {
            CmdCreatePlane();
        }
    }

    [Command]
    void CmdSendName(string name)
    {
        RpcSetName(name);
        Debug.Log("CmdSendName is called from " + netId);
    }

    [ClientRpc]
    void RpcSetName(string name)
    {
        //StartCoroutine(HoldForSeconds(name));
        gameObject.name = name;
        text.text = gameObject.name;
        Debug.Log("RpcSetName is called from " + netId);
    }

    //IEnumerator HoldForSeconds(string name)
    //{
    //    yield return new WaitForSeconds(10f);
    //    gameObject.name = name;
    //}

    [Command]
    void CmdSendPos(Vector2 pos)
    {
        Debug.Log("CmdSendPos is called from " + netId);
        RpcSetPos(pos);

    }

    [ClientRpc]
    void RpcSetPos(Vector2 pos)
    {
        transform.position = pos;

        Debug.Log("RpcSetPos is called from " + netId);
    }

    [Command]
    void CmdSendColor()
    {
        RpcSetColor();
    }

    [ClientRpc]
    void RpcSetColor()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    [Command]
    void CmdCreatePlane()
    {
        Debug.Log(ClientScene.prefabs.Count);
        var prefabs = ClientScene.prefabs;
        foreach (var b in prefabs) {
            if (b.Value.name == "Plane")
            {
                GameObject go = Instantiate(b.Value);
                NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
                return;
            }
        }
    }
}
