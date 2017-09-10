using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GhostShot
{
    public bool isFinal;

    public float timeMark = 0.0f;       // mark at which the position and rotation are of af a given shot

    public Vector3 posMark;
    public Quaternion rotMark;

    public GhostShot()
    {
        isFinal = false;
    }
}