using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Linq;

/// <summary>
/// Game Manager is responsible for score, throw counting and game over behaviour
/// </summary>
public class GameManager : MonoBehaviour
{
    private const string SCORE_CUBE_PARENT_NAME = "ScoreCubesParent"; // name of object to store all score cubes under

    [Header("Score")]
    public TextMeshProUGUI ScoreText; // TMPro for Score
    public Transform ScoreCubesParent; // Parent for Score Cubes
    private int currentScore = 0;
    private int maxScore = 0;
    public static UnityEvent IncreaseScoreEvent = new UnityEvent(); // Unity Event to increase score by 1

    [Header("Throwables")]
    public TextMeshProUGUI ThrowablesText; // TMPro for Throwables
    private int currentThrowablesUsed = 0;

    [Tooltip("Target throwables used during level to 'Win'")]
    public int TargetThrowables;
    public static UnityEvent IncreaseThrowablesUsed = new UnityEvent();// Unity Event to increase throwables used by 1

    [Header("Other")]
    private Coroutine resettingLevel; // coroutine reference which stops reset level from being spammed
    public TextMeshProUGUI DebugText; // optional debug text for when the game is resetting

#if UNITY_EDITOR
    private void OnValidate()
    {
        // here we want to make sure the score cubes parent is assigned, if it isn't, we will try to find it and failing that make a new parent for our score cubes
        if(ScoreCubesParent == null)
        {
            var allObjs = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == SCORE_CUBE_PARENT_NAME).ToList();
            if (allObjs.Count > 0)
            {
                ScoreCubesParent = allObjs[0].transform;
            }
        }

        // make a new parent if it's still null
        if (ScoreCubesParent == null)
        {
            GameObject newParent = new GameObject();
            newParent.name = SCORE_CUBE_PARENT_NAME;
            ScoreCubesParent = newParent.transform;
        }

        // this will move them all to the right parent
        ScoreCube[] allScoreCubes = GameObject.FindObjectsOfType<ScoreCube>();
        foreach (ScoreCube scoreCube in allScoreCubes)
        {
            scoreCube.transform.parent = ScoreCubesParent;
        }
    }
#endif

    private void Awake()
    {
        maxScore = ScoreCubesParent.childCount;
        SetScoreText();
        SetThrowablesText();
    }

    private void OnEnable()
    {
        // subscribe to static methods to respond when they are called
        IncreaseScoreEvent.AddListener(IncrementScore);
        IncreaseThrowablesUsed.AddListener(IncrementThrowablesUsed);
    }

    private void OnDisable()
    {
        // unsubscribe from static methods to prevent memory leaks
        IncreaseScoreEvent.RemoveListener(IncrementScore);
        IncreaseThrowablesUsed.RemoveListener(IncrementThrowablesUsed);
    }

    private void Update()
    {
        // debug key to reset level on user command
        if (Input.GetKeyUp(KeyCode.R))
        {
            ResetLevel(false);
        }
    }

    void IncrementScore()
    {
        currentScore += 1;
        SetScoreText();
        if(currentScore >= maxScore)
        {
            // if score is at max, let's reset the level
            LevelCompleted();
        }
    }
    void SetScoreText()
    {
        ScoreText.text = $"{currentScore}/{maxScore}";
    }

    void IncrementThrowablesUsed()
    {
        currentThrowablesUsed += 1;
        SetThrowablesText();
    }
    void SetThrowablesText()
    {
        ThrowablesText.text = $"{currentThrowablesUsed}/{TargetThrowables}";
    }

    void LevelCompleted()
    {
        if(currentThrowablesUsed > TargetThrowables)
        {
            Debug.Log("Level completed, but over par throws!");
        } else
        {
            Debug.Log("Level completed within par throws!");
        }
        ResetLevel(true);
    }

    void ResetLevel(bool delay)
    {
        if (resettingLevel == null && delay)
        {
            resettingLevel = StartCoroutine(IResetLevel());
        } else if (!delay)
        {
            SceneHandler.ResetCurrentScene();
            resettingLevel = null;
        }
    }

    IEnumerator IResetLevel()
    {
        Debug.Log("Resetting level");
        for (int i = 5; i >= 0; i--)
        {
            if(DebugText != null)
            {
                DebugText.text = $"Resetting level in {i}...";
            }
            yield return new WaitForSeconds(1f);
        }        
        SceneHandler.ResetCurrentScene();
        resettingLevel = null;
    }
}
