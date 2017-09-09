using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GhostShot
{
    public bool isFinal;

    public float timeBegin = 0.0f;
    public float timeEnd = 0.0f;

    public Vector3 posBegin;
    public Vector3 posEnd;

    public Quaternion rotBegin;
    public Quaternion rotEnd;

    public GhostShot(bool _isFinal)
    {
        isFinal = _isFinal;
    }

    public Vector3 PosLerp(float time)
    {
        return Vector3.Lerp(posBegin, posEnd, Mathf.Clamp(time, timeBegin, timeEnd));
    }

    public Quaternion RotLerp(float time)
    {
        return Quaternion.Slerp(rotBegin, rotEnd, Mathf.Clamp(time, timeBegin, timeEnd));
    }
}