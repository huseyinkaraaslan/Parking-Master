using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [Header("Gameobject")]
    public List<GameObject> lockedCars;
    public List<GameObject> unLockedCars;

    public GameObject playerCar;

    [Header("Level Config")]
    public List<LevelConfig> levelConfigs;

    [Header("Variable")]
    public int money = 50000;
    public int lastUnlockedLevel = 1;
    public LevelConfig chosenLevel;


    private void Awake()
    {
        lastUnlockedLevel = PlayerPrefs.GetInt(nameof(lastUnlockedLevel), 1);
        Instance = this;
        unLockedCars = new List<GameObject>(new GameObject[11]);

        if(!(unLockedCars.Contains(lockedCars[0])))
            unLockedCars.Insert(0,lockedCars[0]);   
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            playerCar = unLockedCars[PlayerPrefs.GetInt("chosenCar")];
            chosenLevel = levelConfigs[PlayerPrefs.GetInt("chosenLevel")];

            playerCar = Instantiate(playerCar);
            playerCar.transform.position = chosenLevel.carSpawnPoint;
        }
            
    }
}
