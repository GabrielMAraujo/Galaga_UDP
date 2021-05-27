using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public ServerData serverData;

    private ServerService serverService;

    //Stores the last received server response
    private byte[] currentResponse;

    private void Awake()
    {
        serverService = new ServerService(serverData.ip, serverData.port);
    }

    // Start is called before the first frame update
    void Start()
    {
        serverService.ConnectToServer();
        currentResponse = serverService.SendDiscoveryRequest();

        //Debug.Log(ByteUtils.BitArrayToString(new BitArray(currentResponse)));
        PacketUtils.ParseResponseObject(currentResponse);
        PacketUtils.ParseResponseObject2(currentResponse);

        ServerPacketRequest req = new ServerPacketRequest(
            2,
            InputTypeEnum.LEFT,
            37
        );
        byte[] request = PacketUtils.ParseRequestObject(req);
        Debug.Log("Request: " + ByteUtils.ByteArrayToString(request));
        Debug.Log(Convert.ToString(request[0], toBase: 2) + " " + Convert.ToString(request[1], toBase: 2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
