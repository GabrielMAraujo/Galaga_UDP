using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public ServerData serverData;

    private ServerService serverService;

    //Stores the last received server response
    private byte[] currentByteResponse;
    private ServerPacketResponse currentResponse;

    //Request time window
    private float currentTimer;

    private void Awake()
    {
        serverService = new ServerService(serverData.ip, serverData.port);
        currentTimer = serverData.timeWindow;
    }

    // Start is called before the first frame update
    void Start()
    {
        serverService.ConnectToServer();
        currentByteResponse = serverService.SendDiscoveryRequest();

        //Debug.Log(ByteUtils.BitArrayToString(new BitArray(currentResponse)));
        currentResponse =  PacketUtils.ParseResponseObject(currentByteResponse);
    }

    // Update is called once per frame
    void Update()
    {
        //if(currentTimer > 0f)
        //{
        //    currentTimer -= Time.deltaTime;
        //}
        //else
        //{
        //    //Emit default request with NOTHING input
        //    ServerPacketRequest req = new ServerPacketRequest(
        //        currentResponse.Frame + 1,
        //        InputTypeEnum.NOTHING,
        //        currentResponse.SEQ
        //    );
        //    byte[] request = PacketUtils.ParseRequestObject(req);

        //    currentByteResponse = serverService.SendRequest(request);
        //    currentResponse = PacketUtils.ParseResponseObject(currentByteResponse);

        //    currentTimer = 0.5f;
        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Emit default request with NOTHING input
            ServerPacketRequest req = new ServerPacketRequest(
                currentResponse.Frame + 1,
                InputTypeEnum.NOTHING,
                currentResponse.SEQ
            );
            byte[] request = PacketUtils.ParseRequestObject(req);
            Debug.Log("Request: " + ByteUtils.ByteArrayToString(request));
            Debug.Log(Convert.ToString(request[0], toBase: 2) + " " + Convert.ToString(request[1], toBase: 2));

            currentByteResponse = serverService.SendRequest(request);
            currentResponse = PacketUtils.ParseResponseObject(currentByteResponse);

            currentTimer = 0.5f;
        }
    }
}
