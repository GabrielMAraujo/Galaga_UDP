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

        //Stores the current bit window data
        BitArray currentBitWindow;

        //7 bits - frame
        currentBitWindow = ByteUtils.ByteArrayToBitArray(dataArray, 0, 7);
        //Debug.Log(ByteUtils.ByteArrayToString(ByteUtils.BitArrayToByteArray(currentBitWindow)));
        responseObject.Frame = Convert.ToUInt32(ByteUtils.BitArrayToByteArray(currentBitWindow)[0]);
        Debug.Log("Frame: " + responseObject.Frame);

        //2 bits - input
        currentBitWindow = ByteUtils.ByteArrayToBitArray(dataArray, 7, 2);
        responseObject.Input = (InputTypeEnum)ByteUtils.BitArrayToByteArray(currentBitWindow)[0];
        Debug.Log("Input: " + responseObject.Input);

        //7 bits - SEQ
        currentBitWindow = ByteUtils.ByteArrayToBitArray(dataArray, 9, 7);
        responseObject.SEQ = currentBitWindow;
        Debug.Log("SEQ: " + ByteUtils.ByteArrayToString(ByteUtils.BitArrayToByteArray(currentBitWindow)));
        //8 bits - object count
        currentBitWindow = ByteUtils.ByteArrayToBitArray(dataArray, 16, 8);
        responseObject.ObjectCount = Convert.ToUInt32(ByteUtils.BitArrayToByteArray(currentBitWindow)[0]);
        Debug.Log("Object count: " + responseObject.ObjectCount);

        List<ServerObject> objectList = new List<ServerObject>();
        for (int i = 0; i < responseObject.ObjectCount; i++)
        {
            ServerObject so = new ServerObject();
            //8 bits - type
            currentBitWindow = ByteUtils.ByteArrayToBitArray(dataArray, 24 + (i * 24), 8);
            so.Type = (ObjectTypeEnum)ByteUtils.BitArrayToByteArray(currentBitWindow)[0];
            Debug.Log("Object " + (i + 1) + " - Type: " + so.Type);

            //8 bits - horizontal position
            currentBitWindow = ByteUtils.ByteArrayToBitArray(dataArray, 32 + (i * 24), 8);
            so.XPos = Convert.ToUInt32(ByteUtils.BitArrayToByteArray(currentBitWindow)[0]);
            Debug.Log("Object " + (i + 1) + " - XPos: " + so.XPos);

            //8 bits - vertical position
            currentBitWindow = ByteUtils.ByteArrayToBitArray(dataArray, 40 + (i * 24), 8);
            so.YPos = Convert.ToUInt32(ByteUtils.BitArrayToByteArray(currentBitWindow)[0]);
            Debug.Log("Object " + (i + 1) + " - YPos: " + so.YPos);
        }

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