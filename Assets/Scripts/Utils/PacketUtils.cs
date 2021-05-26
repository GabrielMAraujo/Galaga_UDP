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
        Debug.Log(ByteUtils.ByteArrayToString(ByteUtils.BitArrayToByteArray(currentBitWindow)));
        responseObject.Frame = Convert.ToUInt32(ByteUtils.BitArrayToByteArray(currentBitWindow)[0]);
        Debug.Log(responseObject.Frame);

        //2 bits - input
        currentBitWindow = ByteUtils.ByteArrayToBitArray(dataArray, 7, 2);
        Debug.Log(ByteUtils.ByteArrayToString(ByteUtils.BitArrayToByteArray(currentBitWindow)));
        responseObject.Input = (InputTypeEnum)ByteUtils.BitArrayToByteArray(currentBitWindow)[0];
        Debug.Log(responseObject.Input);

        //7 bits - SEQ
        currentBitWindow = ByteUtils.ByteArrayToBitArray(dataArray, 9, 7);
        Debug.Log(ByteUtils.ByteArrayToString(ByteUtils.BitArrayToByteArray(currentBitWindow)));
        responseObject.SEQ = currentBitWindow;
        //8 bits - object count
        currentBitWindow = ByteUtils.ByteArrayToBitArray(dataArray, 16, 8);
        Debug.Log(ByteUtils.ByteArrayToString(ByteUtils.BitArrayToByteArray(currentBitWindow)));
        responseObject.ObjectCount = Convert.ToUInt32(ByteUtils.BitArrayToByteArray(currentBitWindow)[0]);
        Debug.Log(responseObject.ObjectCount);

        List<ServerObject> objectList = new List<ServerObject>();
        for (int i = 0; i < responseObject.ObjectCount; i++)
        {
            ServerObject so = new ServerObject();
            //8 bits - type
            currentBitWindow = ByteUtils.ByteArrayToBitArray(dataArray, 24 + (i * 24), 8);
            Debug.Log(ByteUtils.ByteArrayToString(ByteUtils.BitArrayToByteArray(currentBitWindow)));
            so.Type = (ObjectTypeEnum)ByteUtils.BitArrayToByteArray(currentBitWindow)[0];
            Debug.Log(so.Type);

            //8 bits - horizontal position
            currentBitWindow = ByteUtils.ByteArrayToBitArray(dataArray, 32 + (i * 24), 8);
            Debug.Log(ByteUtils.ByteArrayToString(ByteUtils.BitArrayToByteArray(currentBitWindow)));
            so.XPos = Convert.ToUInt32(ByteUtils.BitArrayToByteArray(currentBitWindow)[0]);
            Debug.Log(so.XPos);

            //8 bits - vertical position
            currentBitWindow = ByteUtils.ByteArrayToBitArray(dataArray, 40 + (i * 24), 8);
            Debug.Log(ByteUtils.ByteArrayToString(ByteUtils.BitArrayToByteArray(currentBitWindow)));
            so.YPos = Convert.ToUInt32(ByteUtils.BitArrayToByteArray(currentBitWindow)[0]);
            Debug.Log(so.YPos);
        }

        return responseObject;
    }
}
