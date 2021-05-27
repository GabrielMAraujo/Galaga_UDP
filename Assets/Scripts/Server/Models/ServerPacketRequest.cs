using System.Collections;
using System.Collections.Generic;

public class ServerPacketRequest
{
    public ServerPacketRequest(uint frame, InputTypeEnum input, uint ack)
    {
        Frame = frame;
        Input = input;
        ACK = ack;
    }

    public uint Frame { get; set; }
    public InputTypeEnum Input { get; set; }
    public uint ACK { get; set; }
}
