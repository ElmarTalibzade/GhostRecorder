using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPC_Jump : MonoBehaviour
{
    public float jumpForce = 2.5f;

    /// <summary>
    /// Makes this instance jump
    /// </summary>
    public void Jump()
    {
        if (IsGrounded() && CanJump())
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Checks if this instance is grounded
    /// </summary>
    /// <returns>Returns true if this instance is touching the ground</returns>
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.01f);
    }

    /// <summary>
    /// Checks if this instance can jump
    /// </summary>
    /// <returns>Returns true if this instance can jump</returns>
    public bool CanJump()
    {
        return true;
    }
}