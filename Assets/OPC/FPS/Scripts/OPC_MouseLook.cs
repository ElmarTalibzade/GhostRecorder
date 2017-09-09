using UnityEngine;
using System.Collections;

public class OPC_MouseLook : MonoBehaviour
{
    public GameObject CameraObj;

    public float lookSensitivity = 5;
    public float lookSmoothDamp = 0.1f;

    public float minXRotation = -90;
    public float maxXRotation = 90;

    float XRotation;
    float YRotation;

    float XRotationV;
    float YRotationV;

    public float currentXRotation;
    public float currentYRotation;

    private bool canLook = true;

    void Update()
    {
        transform.eulerAngles = new Vector3(0, currentYRotation, 0);
    }

    /// <summary>
    /// Makes the camera look based on X and Y axis
    /// </summary>
    /// <param name="horizontal">X axis</param>
    /// <param name="vertical">Y axis (inverted)</param>
    public void Look(float horizontal, float vertical)
    {
        if (canLook)
        {
            XRotation += -vertical * lookSensitivity;
            YRotation += horizontal * lookSensitivity;

            XRotation = Mathf.Clamp(XRotation, minXRotation, maxXRotation);

            currentXRotation = Mathf.SmoothDamp(currentXRotation, XRotation, ref XRotationV, lookSmoothDamp);
            currentYRotation = Mathf.SmoothDamp(currentYRotation, YRotation, ref YRotationV, lookSmoothDamp);

            CameraObj.transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);
        }
    }

    /// <summary>
    /// Sets the cursor state, which also affects its visibility
    /// </summary>
    /// <param name="state">If set <c>true</c>, mouse look is disabled and cursor is unlocked from its position, also enabling its visibility</param>
    public void SetCursorState(bool state)
    {
        if (state)
        {
            canLook = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            canLook = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}