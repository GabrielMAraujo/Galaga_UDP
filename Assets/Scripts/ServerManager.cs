using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public ServerData serverData;

    private ServerService serverService;

    private void Awake()
    {
        serverService = new ServerService(serverData.ip, serverData.port);
    }

    // Start is called before the first frame update
    void Start()
    {
        serverService.ConnectToServer();
        serverService.SendDiscoveryRequest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
