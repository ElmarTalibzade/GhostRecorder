using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;

    public Vector3 cameraOffset;

    private float finalX = 0;
    private float finalY = 0;
    private float finalZ = 0;

    void Update()
    {
        finalX = 0;
        finalY = 0;
        finalZ = 0;

        finalX = followTarget.position.x + cameraOffset.x;

        finalY = cameraOffset.y;

        finalZ = followTarget.position.z + cameraOffset.z;

        transform.position = new Vector3(finalX, finalY, finalZ);
    }
}