using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [Header("Level Selection")]
    public List<Button> levelButtons = new List<Button>();
    public List<TMP_Text> levelTexts = new List<TMP_Text>();
    public List<Image> levelLocks = new List<Image>();
    
    private TMP_Text lockedLevelText;
    
    private void Start()
    {
        for(int i=0; i<PlayerPrefs.GetInt("lastUnlockedLevel"); i++)
        {
            levelTexts[i].color = Color.green;
            levelLocks[i].gameObject.SetActive(false);
        }

        levelButtons[0].onClick.AddListener(Level1);
        levelButtons[1].onClick.AddListener(Level2);
        levelButtons[2].onClick.AddListener(Level3);
        levelButtons[3].onClick.AddListener(Level4);
        levelButtons[4].onClick.AddListener(Level5);
        levelButtons[5].onClick.AddListener(Level6);
        levelButtons[6].onClick.AddListener(Level7);
        levelButtons[7].onClick.AddListener(Level8);
    }

    private void Level1()
    {
        PlayerPrefs.SetInt("chosenLevel", 0);
        LoadScene.Instance.LoadNextScene("Game");
    }

    private void Level2()
    {
        if (GameManager.Instance.lastUnlockedLevel <= 2)
        {
            PlayerPrefs.SetInt("chosenLevel", 1);
            LoadScene.Instance.LoadNextScene("Game");
        }
    }

    private void Level3()
    {
        if (GameManager.Instance.lastUnlockedLevel <= 3)
        {
            PlayerPrefs.SetInt("chosenLevel", 2);
            LoadScene.Instance.LoadNextScene("Game");
        }
    }

    private void Level4()
    {
        if (GameManager.Instance.lastUnlockedLevel <= 4)
        {
            PlayerPrefs.SetInt("chosenLevel", 3);
            LoadScene.Instance.LoadNextScene("Game");
        }
    }

    private void Level5()
    {
        if (GameManager.Instance.lastUnlockedLevel <= 5)
        {
            PlayerPrefs.SetInt("chosenLevel", 4);
            LoadScene.Instance.LoadNextScene("Game");
        }
    }

    private void Level6()
    {
        if (GameManager.Instance.lastUnlockedLevel <= 6)
        {
            PlayerPrefs.SetInt("chosenLevel", 5);
            LoadScene.Instance.LoadNextScene("Game");
        }
    }

    private void Level7()
    {
        if (GameManager.Instance.lastUnlockedLevel <= 7)
        {
            PlayerPrefs.SetInt("chosenLevel", 6);
            LoadScene.Instance.LoadNextScene("Game");
        }
    }

    private void Level8()
    {
        if (GameManager.Instance.lastUnlockedLevel <= 8)
        {
            PlayerPrefs.SetInt("chosenLevel", 7);
            LoadScene.Instance.LoadNextScene("Game");
        }
    }

    private void OnDestroy()
    {
        foreach(Button button in levelButtons)
            button.onClick.RemoveAllListeners();
    }
}
