using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [Header("Gameobject")]
    public List<GameObject> lockedCars;
    public List<GameObject> unLockedCars;
    
    public GameObject levelCompletedScene;
    public GameObject playerCar;
    public GameObject parkingArea;

    [Header("Configurations")]
    public List<LevelConfig> levelConfigs;
    public List<CarConfig> carInformations = new List<CarConfig>();
    public LevelConfig chosenLevel;
    
    [Header("Player Progress")]
    public int money = 0;
    public int lastUnlockedLevel = 1;
    private int chosenLevelValue;
    
    [Header("Buttons")]
    public Button nextLevelButton;
    public Button restartButton;
    public Button menuButton;
    public Button gearButton;
    
    private void Awake()
    {
        Instance = this;
        unLockedCars = new List<GameObject>(new GameObject[12]);

        if(!(unLockedCars.Contains(lockedCars[0])))
            unLockedCars.Insert(0,lockedCars[0]);

        if (SceneManager.GetActiveScene().name != "Game")
            return;
        
        levelCompletedScene.SetActive(false);
        lastUnlockedLevel = lastUnlockedLevel == 0 ? 1 : lastUnlockedLevel;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Game")
            return;
        
        playerCar = unLockedCars[PlayerPrefs.GetInt("chosenCar")];
        chosenLevel = levelConfigs[PlayerPrefs.GetInt("chosenLevel")];

        playerCar = Instantiate(playerCar);
        playerCar.transform.position = chosenLevel.carSpawnPoint;
        playerCar.transform.rotation = chosenLevel.carSpawnRotation;
        
        parkingArea.transform.position = chosenLevel.carParkingPoint;
        parkingArea.transform.rotation = chosenLevel.parkingAreaRotation;
        
        nextLevelButton.onClick.AddListener(NextLevel);
        restartButton.onClick.AddListener(RestartLevel);
        menuButton.onClick.AddListener(Menu);
        gearButton.onClick.AddListener(ChangeGear);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Game")
            return;
        if (parkingArea.GetComponent<BoxCollider>().bounds.Contains(playerCar.GetComponent<BoxCollider>().bounds.min) &&
            parkingArea.GetComponent<BoxCollider>().bounds.Contains(playerCar.GetComponent<BoxCollider>().bounds.max))  
            StartCoroutine(LevelComplete());
    }

    private IEnumerator LevelComplete()
    {
        yield return new WaitForSeconds(1);
        levelCompletedScene.SetActive(true);
        Debug.Log("load");
    }
    
    private void Menu()
    {
        PlayerPrefs.SetInt("lastUnlockedLevel",lastUnlockedLevel++);
        SceneManager.LoadScene("Menu");
    }

    private void RestartLevel()
    {
        PlayerPrefs.SetInt("lastUnlockedLevel",lastUnlockedLevel++);
        SceneManager.LoadScene("Game");
    }

    private void NextLevel()
    {
        PlayerPrefs.SetInt("lastUnlockedLevel",lastUnlockedLevel++);
        PlayerPrefs.SetInt("chosenLevel", PlayerPrefs.GetInt("chosenLevel") + 1);
        SceneManager.LoadScene("Game");
    }
    
    private void ChangeGear()
    {
        gearButton.GetComponentInChildren<TMP_Text>().text =
            gearButton.GetComponentInChildren<TMP_Text>().text == "R" ? "D" : "R";
    }

    private void OnDestroy()
    {
        nextLevelButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        menuButton.onClick.RemoveAllListeners();
        gearButton.onClick.RemoveAllListeners();
    }
}
