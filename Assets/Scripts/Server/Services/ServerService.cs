using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ServerService : IServerService
{
    //UDP communication references
    private readonly UdpClient _udpClient;
    private IPEndPoint endpoint;

    //Session 8-bit secret
    private byte secret;

    public ServerService(ServerData serverData)
    {
        _udpClient = new UdpClient();

        //Setting timeout limits
        _udpClient.Client.SendTimeout = (int)(serverData.sendTimeout * 1000);
        _udpClient.Client.ReceiveTimeout = (int)(serverData.receiveTimeout * 1000);

        endpoint = new IPEndPoint(IPAddress.Parse(serverData.ip), serverData.port);
    }

    //Establishes a connection with the UDP server
    public void ConnectToServer()
    {
        Debug.Log("Connecting to " + endpoint.ToString() + "...");
        _udpClient.Connect(endpoint);
    }

    //Sends a request byte array to the UDP server
    public byte[] SendRequest(byte[] request)
    {
        Debug.Log("Request packet: " + ByteUtils.ByteArrayToString(request));
        Debug.Log("Sending data to server...");
        //Send data to server
        _udpClient.Send(request, 2);

        var responseData = HandleResponse();

        //Every request has its own secret, re-extract secret
        ExtractSecret(responseData, request);

        //Decodes message and return
        return ByteUtils.XOR(responseData, secret);
    }

    //Handle the server response
    private byte[] HandleResponse()
    {
        byte[] receivedData;
        //Receiving data from server
        try
        {
            receivedData = _udpClient.Receive(ref endpoint);
        }
        catch(SocketException e)
        {
            Debug.LogWarning("Socket response timed out: " + e.ToString());
            return null;
        }

        Debug.Log("Receiving data from " + endpoint.ToString());
        //Debug.Log("Data recieved: " + ByteUtils.ByteArrayToString(receivedData));

        return receivedData;
    }

    //Reverse-engineers the 8-bit secret that encodes the server responses based on the packet server response
    private void ExtractSecret(byte[] responseData, byte[] requestData)
    {
        if (responseData != null)
        {
            //It is known that the first server response byte always has to be frame (7 bits) + first bit of input, which has to be the same of request
            //With that information, it is possible to reverse-engineer the secret by XORing the first response byte with the first request byte.
            secret = (byte)(responseData[0] ^ requestData[0]);
            Debug.Log("Decoded secret: " + ByteUtils.ByteToString(secret));
        }
    }
}
