using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Throwable Item is responsible for adding force and being thrown by ShotManager
/// </summary>
public class ThrowableItem : MonoBehaviour
{
    private Rigidbody rigidBody;
    public float TurnOffAfterDuration;

    private void OnValidate()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Throw the object with a specified force (relevant to mass, so higher power required for more dense objects)
    /// </summary>
    /// <param name="force"></param>
    public void ThrowForward(Vector3 force)
    {
        // just in case the onvalidate failed
        if(rigidBody == null)
        {
            rigidBody = GetComponentInChildren<Rigidbody>();
        }
        rigidBody.velocity = Vector3.zero;
        rigidBody.AddForce(force, ForceMode.Impulse);
        GameManager.IncreaseThrowablesUsed.Invoke();
        StartCoroutine(TurnOffAfterSeconds(TurnOffAfterDuration));
    }

    private IEnumerator TurnOffAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}
