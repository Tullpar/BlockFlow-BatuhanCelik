using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI LevelText;

    public GameObject WinPanel;
    public GameObject LosePanel;

    public void SetTimer(float remainingTime)
    {
        TimeSpan timespan = TimeSpan.FromSeconds(remainingTime);
        string text = timespan.Minutes.ToString() + ":" + timespan.Seconds.ToString();
        TimeText.text = text;
    }

    public void SetLevel(int levelIndex)
    {
        LevelText.text = "Level:" + levelIndex.ToString();
    }


    public void OpenWinPanel()
    {
        WinPanel.SetActive(true);
    }

    public void OpenLosePanel()
    {
        LosePanel.SetActive(true);
    }

    public void ClosePanels()
    {
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
    }

    public void NextLevelButton()
    {
        GameManager.Instance.OpenNextLevel();
        ClosePanels();
    }
    public void RetryButton()
    {
        GameManager.Instance.RetryLevel();
        ClosePanels();
    }

}
