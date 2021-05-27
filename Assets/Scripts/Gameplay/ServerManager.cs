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
    private List<GameObject> currentObjects = new List<GameObject>();

    //Request time window
    private float currentTimer;

    //Temporary function - instantiates new objects and deletes old ones
    private void InstantiateObjects()
    {
        if (currentObjects.Count > 0)
        {
            foreach (var co in currentObjects)
            {
                Destroy(co);
            }
        }

        currentObjects.Clear();

        foreach(var obj in currentResponse.Objects)
        {
            GameObject instance = null;
            switch (obj.Type)
            {
                case ObjectTypeEnum.SHIP:
                    instance = Instantiate(Resources.Load("Prefabs/Ship")) as GameObject;
                    break;
                case ObjectTypeEnum.PROJECTILE:
                    instance = Instantiate(Resources.Load("Prefabs/Projectile")) as GameObject;
                    break;
                case ObjectTypeEnum.UNKNOWN:
                    instance = Instantiate(Resources.Load("Prefabs/Unknown 1")) as GameObject;
                    break;
                case ObjectTypeEnum.UNKNOWN_2:
                    instance = Instantiate(Resources.Load("Prefabs/Unknown 2")) as GameObject;
                    break;
                default:
                    break;
            }

            if(instance != null)
            {
                instance.GetComponent<GameplayObject>().Move(new Vector2Int((int)obj.XPos, (int)obj.YPos));
                currentObjects.Add(instance);
            }
            else
            {
                Debug.LogWarning("Server object with undefined prefab type: " + obj.Type);
            }
        }
    }

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
        InstantiateObjects();
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
            InstantiateObjects();
            currentTimer = 0.5f;
        }
    }
}
