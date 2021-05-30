using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputTypeEnum: byte
{
    NOTHING = 0b_00,
    LEFT = 0b_01,
    RIGHT = 0b_10,
    SHOOT = 0b_11,

    //Menu inputs
    UP,
    DOWN,
    SELECT
}
