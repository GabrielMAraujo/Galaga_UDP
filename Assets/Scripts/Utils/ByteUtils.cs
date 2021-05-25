using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ByteUtils
{
    public static void PrintByteArray(byte[] byteArray)
    {
        Debug.Log(BitConverter.ToString(byteArray).Replace("-", " "));
    }
}
