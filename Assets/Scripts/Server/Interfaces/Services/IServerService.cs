using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IServerService
{
    void ConnectToServer();
    byte[] SendDiscoveryRequest();
    byte[] SendRequest(byte[] request);
}
