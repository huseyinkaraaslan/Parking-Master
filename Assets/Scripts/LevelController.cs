using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public List<Button> levelButtons = new List<Button>();
    public int chosenLevel;
    private void Start()
    {
        for(int i=1; i<GameManager.Instance.lastUnlockedLevel; i++)
        {
            levelButtons[i].GetComponentInChildren<Text>().color = Color.green;
            levelButtons[i].transform.Find("Lock").gameObject.SetActive(false);
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
            GameManager.Instance.lastUnlockedLevel = 2;
    }

    private void Level3()
    {
        if (GameManager.Instance.lastUnlockedLevel <= 3)
            GameManager.Instance.lastUnlockedLevel = 3;
    }

    private void Level4()
    {
        if (GameManager.Instance.lastUnlockedLevel <= 4)
            GameManager.Instance.lastUnlockedLevel = 4;
    }

    private void Level5()
    {
        if (GameManager.Instance.lastUnlockedLevel <= 5)
            GameManager.Instance.lastUnlockedLevel = 5;
    }

    private void Level6()
    {
        if (GameManager.Instance.lastUnlockedLevel <= 6)
            GameManager.Instance.lastUnlockedLevel = 6;
    }

    private void Level7()
    {
        if (GameManager.Instance.lastUnlockedLevel <= 7)
            GameManager.Instance.lastUnlockedLevel = 7;
    }

    private void Level8()
    {
        if (GameManager.Instance.lastUnlockedLevel <= 8)
            GameManager.Instance.lastUnlockedLevel = 8;
    }
}
