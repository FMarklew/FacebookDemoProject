using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shot Manager handles the throwables shooting out from the camera
/// </summary>
public class ShotManager : MonoBehaviour
{
    public Image CrossHairImage;
    public bool ShootingEnabled = true; // in case we have a cutscene or pause menu, we can just toggle this

    public ThrowableItem ThrowablePrefab; // this can be a list in the future
    [Tooltip("Force to throw objects")]
    public float ThrowForce = 200f;
    public float SpawnDistance = 0.25f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if(ShootingEnabled)
        {
            // get mouse pos
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // convert from screen to world
            var point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

            // move cross hair to position
            CrossHairImage.transform.position = new Vector3(point.x, point.y, point.z);
            // TODO: Fix the sometimes glitchy look of the crosshair when rotating the camera

            if (Input.GetButtonUp("Fire1"))
            {
                // get forward direction of cam
                Vector3 camForward = (cam.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y)).direction);
                GameObject pooledObj = ObjectPooler.GetObject(ThrowablePrefab.gameObject);
                
                // move object to the spawn point and throw it
                pooledObj.transform.position = point + (camForward.normalized * SpawnDistance);
                ThrowableItem throwable = pooledObj.GetComponent<ThrowableItem>(); // TODO: Avoid the get component here
                throwable.ThrowForward(camForward.normalized * ThrowForce);
            }
        }
    }
}
