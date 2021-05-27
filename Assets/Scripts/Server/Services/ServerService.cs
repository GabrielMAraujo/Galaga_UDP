using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ServerService : IServerService
{
    private readonly string _ip;
    private readonly int _port;

    //UDP communication references
    private readonly UdpClient _udpClient;
    private IPEndPoint endpoint;

    //Session 8-bit secret
    private byte secret;

    public ServerService(string ip, int port)
    {
        _ip = ip;
        _port = port;

        _udpClient = new UdpClient();
        endpoint = new IPEndPoint(IPAddress.Parse(_ip), _port);
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
        ExtractSecret(responseData);

        //Decodes message and return
        return ByteUtils.XOR(responseData, secret);
    }

    //Handle the server response
    private byte[] HandleResponse()
    {
        //Receiving data from server
        byte[] receivedData = _udpClient.Receive(ref endpoint);

        Debug.Log("Receiving data from " + endpoint.ToString());
        Debug.Log("Data recieved: " + ByteUtils.ByteArrayToString(receivedData));

        return receivedData;
    }

    //Reverse-engineers the 8-bit secret that encodes the server responses based on the discovery packet server response
    private void ExtractSecret(byte[] discoveryResponse)
    {
        //It is known that the first server response byte always has to be 0 (7 bits frame 0, input confirmation 00 first bit).
        //With that information, it is possible to reverse-engineer the secret by XORing the first response bit with the byte 0.
        //However, as any byte XOR 0 equals the same byte, we can directly assign the secret as the first request's first byte.
        secret = discoveryResponse[0];
        Debug.Log("Decoded secret: " + ByteUtils.ByteToString(secret));
    }
}
