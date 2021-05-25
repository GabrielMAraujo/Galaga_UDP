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

    //Sends an initial, simple request to reverse-enginner the secret and start the game
    public void SendDiscoveryRequest()
    {
        //Discovery request is all zeroes (frame 0, instruction b00 - do nothing, first ACK is 0)
        byte[] discoveryRequest = new byte[2];

        Debug.Log("Request packet: ");
        ByteUtils.PrintByteArray(discoveryRequest);
        Debug.Log("Sending data to server...");
        //Send data to server
        _udpClient.Send(discoveryRequest, 2);

        HandleResponse();
    }

    //Handle the server response
    private void HandleResponse()
    {
        //Receiving data from server
        var receivedData = _udpClient.Receive(ref endpoint);

        Debug.Log("Receiving data from " + endpoint.ToString());
        Debug.Log("Data recieved: ");
        ByteUtils.PrintByteArray(receivedData);

    }
}
