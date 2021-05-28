using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectTypeEnum: byte
{
    SHIP = 0x00,
    PROJECTILE = 0x01,
    ENEMY_SHOOTER = 0x02,
    ENEMY_PROJECTILE = 0x03,
    FINAL_OBJECT = 0xFF
}