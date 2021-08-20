using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene Handler should be used to control scenes through its static methods
/// </summary>
public class SceneHandler : MonoBehaviour
{
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public static void ResetCurrentScene()
    {
        Debug.Log("Resetting scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ObjectPooler.ClearAll();
    }
}
