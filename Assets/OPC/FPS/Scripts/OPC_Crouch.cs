using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPC_Crouch : MonoBehaviour
{
    public float crouchSpeed = 1.5f;            //player's speed when he's crouching
    public float crouchAmount = 1;
    public float crouchStateSpeed = 0.5f;       //how fast player switches to crouch speed

    public bool isCrouching = false;

    GameObject CameraObj;

    Vector3 camera_initPos;
    Vector3 camera_crouchPos;

    OPC_Locomotion locomotion;
    CapsuleCollider capsule;
    OPC_Jump jump;

    void Start()
    {
        locomotion = GetComponent<OPC_Locomotion>();
        jump = GetComponent<OPC_Jump>();
        capsule = GetComponent<CapsuleCollider>();
        CameraObj = GetComponent<OPC_MouseLook>().CameraObj;

        camera_initPos = CameraObj.transform.localPosition;
        camera_crouchPos = new Vector3(camera_initPos.x, camera_initPos.y - crouchAmount, camera_initPos.z);
    }

    public void SetCrouch(bool state)
    {
        if (state)
        {
            if (jump.IsGrounded())
            {
                isCrouching = true;
            }
        }
        else
        {
            if (!CheckIfColliderAbove())
            {
                isCrouching = false;
            }
        }

        if (isCrouching)
        {
            CameraObj.transform.localPosition = Vector3.Lerp(CameraObj.transform.localPosition, camera_crouchPos, Time.deltaTime / crouchStateSpeed);
            capsule.center = new Vector3(0, -0.5f, 0);
            capsule.height = 1;
        }
        else
        {
            CameraObj.transform.localPosition = Vector3.Lerp(CameraObj.transform.localPosition, camera_initPos, Time.deltaTime / crouchStateSpeed);

            capsule.center = Vector3.zero;
            capsule.height = 2;
        }
    }
    /// <summary>
    /// Checks if there is any collider above this instance
    /// </summary>
    /// <returns>Returns <c>true</c> if this instance is colliding with a physical object above</returns>
    public bool CheckIfColliderAbove()
    {
        return Physics.Raycast(transform.position, Vector3.up, 1);
    }
}