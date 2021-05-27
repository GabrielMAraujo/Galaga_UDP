using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Server Data", menuName = "Server Data")]
public class ServerData : ScriptableObject
{
    public string ip;
    public int port;
    public float timeWindow;
}
