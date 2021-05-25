using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IServerService
{
    void ConnectToServer();
    void SendDiscoveryRequest();
}
