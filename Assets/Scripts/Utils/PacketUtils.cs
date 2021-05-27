using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PacketUtils
{
    //Parses byte array into an appropriate response object
    public static ServerPacketResponse ParseResponseObject(byte[] dataArray)
    {
        ServerPacketResponse responseObject = new ServerPacketResponse();

        //Frame - first 7 bits
        responseObject.Frame = (uint)dataArray[0] >> 1;

        //Input - 2 bits
        //Last bit in byte 0 (shift left 1 bit to give space to second bit)
        byte input = (byte)((dataArray[0] & 0x1) << 1);
        //Add first bit in byte 1 to LSB of input
        input = (byte)(input | (byte)(dataArray[1] >> 7));
        responseObject.Input = (InputTypeEnum)input;

        //SEQ - 7 bits
        //Ignore MSB by AND with all bits except the MSB
        responseObject.SEQ = (byte)(dataArray[1] & 0b_01111111);

        //Object count - 8 bits
        responseObject.ObjectCount = dataArray[2];

        //Packet object count error verification
        //Get the response packet number of objects by seeing how many object 3 byte blocks are in the response
        int packetObjectCount = (dataArray.Length - 3) / 3;
        if(packetObjectCount != responseObject.ObjectCount)
        {
            Debug.LogError("Response object count does not match the number of bytes in the response packet.");
            return null;
        }

        List<ServerObject> objectList = new List<ServerObject>();
        for (int i = 0; i < responseObject.ObjectCount; i++)
        {
            ServerObject so = new ServerObject();

            //Type - 8 bits
            so.Type = (ObjectTypeEnum)dataArray[3 + (3 * i)];

            //Horizontal position - 8 bits
            so.XPos = dataArray[4 + (3 * i)];

            //Vertical position - 8 bits
            so.YPos = dataArray[5 + (3 * i)];

            objectList.Add(so);
        }
        responseObject.Objects = objectList;

        return responseObject;
    }

    //Parses request object into a byte array to be sent to the server
    public static byte[] ParseRequestObject(ServerPacketRequest request)
    {
        byte[] result = new byte[2];

        byte currentByte;

        //7 bits - frame
        currentByte = (byte)request.Frame;
        //Shift left 1 bit - leave space for input bit
        currentByte = (byte)(currentByte << 1);

        //Add bit 1 of input in last bit
        currentByte = (byte)(currentByte | ((byte)request.Input >> 1));
        result[0] = currentByte;

        //Bit 2 of input
        currentByte = (byte)((byte)request.Input & 0x1);
        //Shift 7 bits to the left to free space for ACK
        currentByte = (byte)(currentByte << 7);

        //7 bits - ACK
        //Add ACK in the last 7 bits
        currentByte = (byte)(currentByte | request.ACK);
        result[1] = currentByte;

        return result;
    }
}