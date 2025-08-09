using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int LevelIndex { get { return PlayerPrefs.GetInt("Level Index", 0); } set { PlayerPrefs.SetInt("Level Index", value); } }

    float LevelTimer;

    public void RetryLevel()
    {
        int realLevelIndex = LevelIndex % LevelConfigurator.Instance.LevelDatas.Count;
        LevelConfigurator.Instance.Load(realLevelIndex);
        LevelTimer = LevelSpawner.LevelData.LevelTimer;
        UIManager.Instance.SetLevel(LevelIndex);

    }

    public void NextLevel()
    {
        LevelIndex = LevelIndex + 1;
        int realLevelIndex = LevelIndex % LevelConfigurator.Instance.LevelDatas.Count;
        LevelConfigurator.Instance.Load(realLevelIndex);
        LevelTimer = LevelSpawner.LevelData.LevelTimer;
        UIManager.Instance.SetLevel(LevelIndex);
    }


    private void Update()
    {
        LevelTimer -= Time.deltaTime;
        UIManager.Instance.SetTimer(LevelTimer);
        if(LevelTimer <=0)
        {
            GameManager.Instance.Lose();
        }
    }


}
