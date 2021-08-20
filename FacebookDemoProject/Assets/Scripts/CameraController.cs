using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera controller is responsible for turning the player camera around a pivot object
/// </summary>
public class CameraController : MonoBehaviour
{
    [Tooltip("Speed of camera rotation")]
    public float CameraPanSpeed = 3f;

    [Tooltip("Transform to rotate camera relvant to")]
    public Transform PivotObject;

    [Tooltip("Minimum angle for the camera, clamped to value above this")]
    public float MinXAngleRotation;
    [Tooltip("Maximum angle for the camera, clamped to value below this")]
    public float MaxXAngleRotation;

    private float previousXrotation; // cached previous rotation to avoid rubber banding

    void Update()
    {
        if (Input.GetButton("Fire2"))
        {
            HandleAxis("Mouse X", Vector3.up);
            HandleAxis("Mouse Y", Vector3.left);
        }
    }

    void HandleAxis(string axisName, Vector3 axisPositiveDirection)
    {
        var dir = Input.GetAxis(axisName);
        if (Mathf.Abs(dir) > 0)
        {
            PivotObject.Rotate(dir * axisPositiveDirection * CameraPanSpeed * Time.deltaTime);

            // correct for pitch/yaw of camera to stabilize the z axis at 0
            float correctedRotation = PivotObject.localRotation.eulerAngles.x;
            if(Mathf.Abs(correctedRotation - previousXrotation) > 300)
            {
                correctedRotation -= 360f;
            }
            float clampedXRotation = Mathf.Clamp(correctedRotation, MinXAngleRotation, MaxXAngleRotation);
            
            PivotObject.localRotation = Quaternion.Euler(clampedXRotation, PivotObject.rotation.eulerAngles.y, 0f);
            previousXrotation = PivotObject.localRotation.x;
        }
    }
}
