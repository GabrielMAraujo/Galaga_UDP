using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectTypeEnum: byte
{
    SHIP = 0b_0000_0000,
    PROJECTILE = 0b_0000_0001,
    UNKNOWN = 0b_0000_0010,
    UNKNOWN_2 = 0b_0000_0011
}