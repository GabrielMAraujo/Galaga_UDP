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
}
