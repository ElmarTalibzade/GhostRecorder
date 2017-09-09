using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OPC_Locomotion))]
[RequireComponent(typeof(OPC_Jump))]
public class OPC_Manager : MonoBehaviour
{
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode SprintKey = KeyCode.LeftShift;

    private OPC_Locomotion locomotion;
    private OPC_Jump jump;

    void Start()
    {
        locomotion = GetComponent<OPC_Locomotion>();
        jump = GetComponent<OPC_Jump>();
    }

    void Update()
    {
        locomotion.isRunning = Input.GetKeyDown(SprintKey) && jump.IsGrounded();

        locomotion.Move(new Vector2(
                -Input.GetAxis("Vertical"),
                Input.GetAxis("Horizontal")
            ));

        if (Input.GetKeyDown(JumpKey))
        {
            jump.Jump();
        }
    }
}