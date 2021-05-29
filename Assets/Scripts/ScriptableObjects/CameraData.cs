using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Data", menuName = "Camera Data")]
public class CameraData : ScriptableObject
{
    public float animationTime;
    public Vector3 animationTarget;
    public float animationFinalScale;
}