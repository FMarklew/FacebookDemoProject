using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Score Cube increments score when its GameObject is disabled
/// </summary>
public class ScoreCube : MonoBehaviour
{
    private void OnDisable()
    {
        GameManager.IncreaseScoreEvent.Invoke();
        // TODO: Add Particle system of diamond moving upwards at this location
    }
}
