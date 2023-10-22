using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [Header("Gameobject")]
    public List<GameObject> lockedCars;
    public List<GameObject> unLockedCars;

    private GameObject playerCar;

    [Header("Level Config")]
    public List<LevelConfig> levelConfigs;

    [Header("Variable")]
    public int money = 50000;
    public int currentLevel = 5;
    public int choosenCar = 0;
    public int currentLevelIndex
    { 
        get 
        { 
            return currentLevel; 
        } 
        set 
        { 
            UnlockCar(); 
            currentLevel = value;
        } 
    }

    private void Awake()
    {
        Instance = this;
        unLockedCars = new List<GameObject>(new GameObject[11]);

        if(!(unLockedCars.Contains(lockedCars[0])))
            unLockedCars.Insert(0,lockedCars[0]);

        /*playerCar = Instantiate(unLockedCars[choosenCar]);
        playerCar.transform.position = levelConfigs[currentLevel].carSpawnPoint;*/
    }

    private void UnlockCar()
    {
        for (int i = 0; i < currentLevel; i++)
        {
            if (!(unLockedCars.Contains(lockedCars[i])))
                unLockedCars.Add(lockedCars[i]);
        }
    }
}
