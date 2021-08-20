using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Score Collider is responsible for increasing score when it is triggered by the correct layer specified in ScoreLayers
/// </summary>
public class ScoreCollider : MonoBehaviour
{
    public LayerMask ScoreLayers;

    private void OnTriggerEnter(Collider other)
    {
        // if layer of object entering is within our layermask
        if (ScoreLayers == (ScoreLayers | (1 << other.gameObject.layer)))
        {
            other.gameObject.SetActive(false);
            Debug.Log("Score!");
        }
    }
}
