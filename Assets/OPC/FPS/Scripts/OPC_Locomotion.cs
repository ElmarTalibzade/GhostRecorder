using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPC_Locomotion : MonoBehaviour
{

    public float walkSpeed = 5;
    public float sprintSpeed = 5;

    private Vector3 moveDirection;
    private Vector3 finalVelocity;

    private Rigidbody Body;

    public bool isRunning = false;

    void Start()
    {
        Body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Body.velocity = new Vector3(finalVelocity.x, Body.velocity.y, finalVelocity.z);
    }

    /// <summary>
    /// Moves the player in a specified direction
    /// </summary>
    /// <param name="DIR">The direction in which player will move.</param>
    public void Move(Vector2 DIR)
    {
        moveDirection = new Vector3(DIR.x * GetSpeed(), (DIR.y * GetSpeed()) + (DIR.x * GetSpeed()), DIR.y * GetSpeed());
        moveDirection = transform.TransformDirection(moveDirection);

        finalVelocity = Body.velocity;
        finalVelocity = moveDirection;
    }

    /// <summary>
    /// Determines whether this instance can move.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance can move; otherwise, <c>false</c>.
    /// </returns>
    public bool CanMove()
    {
        return true;
    }

    /// <summary>
    /// Determines whether this instance can run.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance can run; otherwise, <c>false</c>.
    /// </returns>
    public bool CanRun()
    {
        return CanMove();
    }

    /// <summary>
    /// Gets the speed at which player should move
    /// </summary>
    /// <returns>Speed at which player should be moving</returns>
    public float GetSpeed()
    {
        if (isRunning)
        {
            return sprintSpeed;
        }
        else
        {
            return walkSpeed;
        }
    }

}