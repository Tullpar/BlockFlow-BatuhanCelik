using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    bool IsLevelFinished = false;

    public LevelManager LevelManager;

    private void Start()
    {
        RetryLevel();
    }

    public void OpenNextLevel()
    {
        IsLevelFinished = false;
        LevelManager.NextLevel();
    }

    public void RetryLevel()
    {
        IsLevelFinished = false;
        LevelManager.RetryLevel();
    }

    public void Win()
    {
        if (!IsLevelFinished)
        {
            IsLevelFinished = true;
            UIManager.Instance.OpenWinPanel();
        }
    }

    public void Lose()
    {
        if (!IsLevelFinished)
        {
            IsLevelFinished = true; 
            UIManager.Instance.OpenLosePanel();

        }

    }


}
