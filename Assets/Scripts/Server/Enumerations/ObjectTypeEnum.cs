using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectTypeEnum: byte
{
    SHIP = 0b_0000_0000,
    PROJECTILE = 0b_0000_0001,
    ENEMY_SHOOTER = 0b_0000_0010,
    ENEMY_PROJECTILE = 0b_0000_0011
}