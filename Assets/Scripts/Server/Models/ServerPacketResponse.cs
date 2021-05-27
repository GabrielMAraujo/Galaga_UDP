using System.Collections;
using System.Collections.Generic;

public class ServerPacketResponse
{
    public uint Frame { get; set; }
    public InputTypeEnum Input { get; set; }
    public BitArray SEQ { get; set; }
    public uint ObjectCount { get; set; }
    public List<ServerObject> Objects { get; set; }
}
