using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool gameOver = false;
    //Counts how many request were tried
    private int tryCount = 0;

    private void Awake()
    {
        serverService = new ServerService(serverData);
        currentTimer = serverData.timeWindow;
        gameManager = GameManager.instance;
        gameManager.OnGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        gameManager.OnGameOver -= OnGameOver;
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
        if (!gameOver)
        {
            //Default request timer. If no input is registered in this time, the default request is sent
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
    }

    private void OnGameOver(bool winner)
    {
        gameOver = true;
    }

    //Create request based on received input type and last response information. It can also directly recieve the byte array to be sent
    public void CreateRequest(InputTypeEnum input, byte[] byteRequest = null)
    {
        byte[] request;

        tryCount++;

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

        if (currentResponse != null)
        {
            Debug.Log("Frame: " + currentResponse.Frame);
        }

        //Check for response packet validation. If not valid, resend package
        if(!PacketUtils.IsPacketValid(currentByteResponse))
        {
            if (tryCount <= serverData.maximumTries)
            {
                Debug.Log("Resending last package...");
                CreateRequest(InputTypeEnum.NOTHING, request);
            }
            //If exceeded the amount of tries, stop trying and emit the server desync modal
            else
            {
                Debug.LogWarning("Maximum amount of tries exceeded");
                gameOver = true;
                gameManager.gameOver = true;
                SceneManager.LoadScene("Server Desync", LoadSceneMode.Additive);
            }

            return;
        }
        else
        {
            //Valid packet, reset maximum try count
            tryCount = 0;
        }


        currentResponse = PacketUtils.ParseResponseObject(currentByteResponse);

        gameManager.UpdateObjects(currentResponse.Objects);
        currentTimer = serverData.timeWindow;
    }
}
