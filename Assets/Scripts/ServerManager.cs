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
        serverService = new ServerService(serverData);
        currentTimer = serverData.timeWindow;
    }

    // Start is called before the first frame update
    void Start()
    {
        serverService.ConnectToServer();
        //Discovery request is all zeroes (frame 0, instruction b00 - do nothing, first ACK is 0)
        byte[] discoveryRequest = new byte[2];
        currentByteResponse = serverService.SendRequest(discoveryRequest);

        //Debug.Log(ByteUtils.BitArrayToString(new BitArray(currentResponse)));
        currentResponse =  PacketUtils.ParseResponseObject(currentByteResponse);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTimer > 0f)
        {
            if (currentResponse != null)
            {
                currentTimer -= Time.deltaTime;
            }
        }
        else
        {
            //Emit default request with NOTHING input
            ServerPacketRequest req = new ServerPacketRequest(
                currentResponse.Frame + 1,
                InputTypeEnum.NOTHING,
                currentResponse.SEQ
            );
            Debug.Log("Frame: " + req.Frame);
            byte[] request = PacketUtils.ParseRequestObject(req);

            currentByteResponse = serverService.SendRequest(request);
            currentResponse = PacketUtils.ParseResponseObject(currentByteResponse);

            currentTimer = 0.5f;
        }
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
