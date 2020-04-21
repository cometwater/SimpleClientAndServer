using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : MonoBehaviour
{
    public GameObject planePrefab;
    NetworkClient myClient;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void ServerListen()
    {
        NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnect);
        NetworkServer.RegisterHandler(MsgType.Ready, OnClientReady);
        if (NetworkServer.Listen(4444))
        {
            Debug.Log("Server started listening on port 4444");
        }
        LocalClientConnect();
    }

    public void LocalClientConnect()
    {
        NetworkClient localClient = ClientScene.ConnectLocalServer();
        localClient.RegisterHandler(MsgType.Connect, OnClientConnect);
    }

    // Create a client and connect to the server port
    public void ClientConnect()
    {
        ClientScene.RegisterPrefab(planePrefab);
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnClientConnect);
        myClient.Connect("127.0.0.1", 4444);
    }

    void OnServerConnect(NetworkMessage msg)
    {
        Debug.Log("New client connected: " + msg.conn);
    }

    void OnClientConnect(NetworkMessage msg)
    {
        ClientScene.Ready(msg.conn);
        Debug.Log("Connected to server: " + msg.conn);
        
    }

    // When client is ready spawn a few trees
    void OnClientReady(NetworkMessage msg)
    {
        Debug.Log("1Client is ready to start: " + msg.conn);
        //if (msg.conn.playerControllers.Count == 0)
        //{
        //    // this is now allowed (was not for a while)
        //    if (LogFilter.logDebug) { Debug.Log("Ready with no player object"); }
        //}
        NetworkServer.SetClientReady(msg.conn);
        Debug.Log("2Client is ready to start: " + msg.conn);
        SpawnPlane(msg);
    }

    void SpawnPlane(NetworkMessage msg)
    {
        Debug.Log("LocalClientPrefabsCount " + ClientScene.prefabs.Count);
        //var planeGo = Instantiate(planePrefab);
        //NetworkServer.SpawnWithClientAuthority(planeGo, msg.conn);
    }
}
