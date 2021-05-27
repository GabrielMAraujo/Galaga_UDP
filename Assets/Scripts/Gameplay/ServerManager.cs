using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public ServerData serverData;
    public GameManager gameManager;


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
        //Create first request
        CreateRequest(InputTypeEnum.NOTHING, true);
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
            //Create request with NOTHING input
            CreateRequest(InputTypeEnum.NOTHING);
        }
    }

    //Create request based on received input type and last response information (if not first request)
    public void CreateRequest(InputTypeEnum input, bool firstRequest = false)
    {
        byte[] request;

        if (!firstRequest)
        {
            ServerPacketRequest req = new ServerPacketRequest(
                currentResponse.Frame + 1,
                input,
                currentResponse.SEQ
            );
            request = PacketUtils.ParseRequestObject(req);
        }
        else
        {
            //Discovery request is all zeroes (frame 0, instruction b00 - do nothing, first ACK is 0)
            request = new byte[2];
        }
        

        currentByteResponse = serverService.SendRequest(request);
        currentResponse = PacketUtils.ParseResponseObject(currentByteResponse);
        gameManager.InstantiateObjects(currentResponse.Objects);
        currentTimer = serverData.timeWindow;
    }
}
