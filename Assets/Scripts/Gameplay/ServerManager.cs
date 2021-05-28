using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public ServerData serverData;
    private GameManager gameManager;


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
        gameManager = GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        serverService.ConnectToServer();
        //Create first request
        //First request is all zeroes (frame 0, instruction b00 - do nothing, first ACK is 0)
        CreateRequest(InputTypeEnum.NOTHING, new byte[2]);
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

    //Create request based on received input type and last response information. It can also recieve the byte array to be sent directly
    public void CreateRequest(InputTypeEnum input, byte[] byteRequest = null)
    {
        //If server is already communicating, ignore input
        //TODO - input bus
        if (serverService.IsCommunicating())
        {
            Debug.Log("Package ignored, communication already happening");
            return;
        }

        byte[] request;

        if (byteRequest == null)
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
            request = byteRequest;
        }
        

        currentByteResponse = serverService.SendRequest(request);

        if(currentByteResponse == null)
        {
            Debug.Log("Resending last package...");
            //CreateRequest(InputTypeEnum.NOTHING, request);
            return;
        }
        if (currentResponse != null)
        {
            Debug.Log("Frame: " + currentResponse.Frame);
        }

        if(!PacketUtils.IsPacketValid(currentByteResponse))
        {
            Debug.Log("Resending last package...");
            CreateRequest(InputTypeEnum.NOTHING, request);
            return;
        }
        currentResponse = PacketUtils.ParseResponseObject(currentByteResponse);

        gameManager.InstantiateObjects(currentResponse.Objects);
        currentTimer = serverData.timeWindow;
    }
}
