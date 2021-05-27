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

    public static string BitArrayToString(BitArray bitArray)
    {
        string result = "";

        for(int i = 0; i < bitArray.Length; i++)
        {
            char bit = bitArray[i] ? '1' : '0';
            result += bit;
        }

        return result;
    }

    //Do a XOR operation with a byte array and a single byte
    public static byte[] XOR(byte[] array, byte b)
    {
        if(array == null)
        {
            return null;
        }


        byte[] xorArray = new byte[array.Length];

        for(int i = 0; i < array.Length; i++)
        {
            xorArray[i] = (byte)(array[i] ^ b);
        }

        Debug.Log("Decoded packet: " + ByteArrayToString(xorArray));

        return xorArray;
    }

    //Converts a byte array to a bit array, passing a start bit and the number of bits to be selected
    public static BitArray ByteArrayToBitArray(byte[] byteArray, int startBit, int numBits)
    {
        BitArray result = new BitArray(numBits);

        for(int i = startBit; i < numBits + startBit; i++)
        {
            //Consider the initial index to see which byte has to be consumed
            //Right shifting byte 'i' times to set byte end at desired bit
            //Then, do an AND operation to get only the last bit, and set the bit in the result
            result.Set(i - startBit, (byteArray[i / 8] >> (i % 8) & 0b_01) > 0);
        }

        return result;
    }

    //Converts a bit array to a byte array
    public static byte[] BitArrayToByteArray(BitArray bitArray)
    {
        //Create byte array with appropriate size
        byte[] result = new byte[(bitArray.Length - 1) / 8 + 1];
        //Pass bits to created byte array
        bitArray.CopyTo(result, 0);
        return result;
    }
}
