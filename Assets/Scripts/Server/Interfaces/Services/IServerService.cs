using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IServerService
{
    void ConnectToServer();
    byte[] SendRequest(byte[] request);
    bool IsCommunicating();
}
