using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ByteUtils
{
    public static string ByteArrayToString(byte[] byteArray)
    {
        return BitConverter.ToString(byteArray).Replace("-", " ");
    }

    public static string ByteToString(byte b)
    {
        return BitConverter.ToString(new byte[1] { b });
    }

    //Do a XOR operation with a byte array and a single byte
    public static byte[] XOR(byte[] array, byte b)
    {
        byte[] xorArray = new byte[array.Length];

        for(int i = 0; i < array.Length; i++)
        {
            xorArray[i] = (byte)(array[i] ^ b);
        }

        Debug.Log("Decoded packet: " + ByteArrayToString(xorArray));

        return xorArray;
    }
}
