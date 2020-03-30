using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlaneBehaviour : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            float y = transform.position.y + 1;
            CmdSendPos(new Vector2(transform.position.x, y));
        }
    }

    [Command]
    void CmdSendPos(Vector2 pos)
    {
        Debug.Log("CmdSendPos in PlaneBehaviour is called from " + netId);
        RpcSetPos(pos);
    }

    [ClientRpc]
    void RpcSetPos(Vector2 pos)
    {
        Debug.Log("RpcSetPos in PlaneBehaviour is called from " + netId);
        transform.position = pos;
    }
}
