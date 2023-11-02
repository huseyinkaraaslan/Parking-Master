using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    [Header("Gameobject")]
    public List<GameObject> lockedCars;
    public List<GameObject> unLockedCars;
    
    public GameObject endLevelScreen;
    public GameObject pauseScreen;
    public GameObject playScreen;
    public GameObject playerCar;
    public GameObject parkingArea;
    public GameObject roadSign;

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
    public Button pauseButton;
    public Button resumeButton;
    
    private void Awake()
    {
        Instance = this;
        unLockedCars = new List<GameObject>(new GameObject[12]);

        if(!(unLockedCars.Contains(lockedCars[0])))
            unLockedCars.Insert(0,lockedCars[0]);

        if (SceneManager.GetActiveScene().name != "Game")
            return;
        
        lastUnlockedLevel = lastUnlockedLevel == 0 ? 1 : lastUnlockedLevel;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Game")
            return;
        
        endLevelScreen.SetActive(false);
        pauseScreen.SetActive(false);
        playScreen.SetActive(true);
        
        playerCar = unLockedCars[PlayerPrefs.GetInt("chosenCar")];
        chosenLevel = levelConfigs[PlayerPrefs.GetInt("chosenLevel")];

        playerCar = Instantiate(playerCar);
        playerCar.transform.position = chosenLevel.carSpawnPoint;
        playerCar.transform.rotation = chosenLevel.carSpawnRotation;
        
        parkingArea.transform.position = chosenLevel.carParkingPoint;
        parkingArea.transform.rotation = chosenLevel.parkingAreaRotation;
        
        nextLevelButton.onClick.AddListener(NextLevelButton);
        restartButton.onClick.AddListener(RestartLevelButton);
        menuButton.onClick.AddListener(MenuButton);
        gearButton.onClick.AddListener(ChangeGearButton);
        pauseButton.onClick.AddListener(PauseButton);
        resumeButton.onClick.AddListener(ResumeButton);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Game")
            return;

        var roadDirection = parkingArea.transform.position - playerCar.transform.position;
        float angle = Vector3.Angle(playerCar.transform.forward, roadDirection);
        angle = playerCar.transform.forward.z <= 0 ? angle : -angle;
        roadSign.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            
        if (parkingArea.GetComponent<BoxCollider>().bounds.Contains(playerCar.GetComponent<BoxCollider>().bounds.min) &&
            parkingArea.GetComponent<BoxCollider>().bounds.Contains(playerCar.GetComponent<BoxCollider>().bounds.max) &&
            gearButton.GetComponentInChildren<TMP_Text>().text == "P")  
            StartCoroutine(LevelComplete());
        
    }

    private IEnumerator LevelComplete()
    {
        yield return new WaitForSeconds(1);
        endLevelScreen.SetActive(true);
        pauseScreen.SetActive(false);
        playScreen.SetActive(false);
    }

    private void PauseButton()
    {
        Time.timeScale = 0;
        endLevelScreen.SetActive(false);
        pauseScreen.SetActive(true);
        playScreen.SetActive(false);
    }

    private void ResumeButton()
    {
        Time.timeScale = 1;
        endLevelScreen.SetActive(false);
        pauseScreen.SetActive(false);
        playScreen.SetActive(true);
    }
    
    private void MenuButton()
    {
        PlayerPrefs.SetInt("lastUnlockedLevel",lastUnlockedLevel++);
        SceneManager.LoadScene("Menu");
    }

    private void RestartLevelButton()
    {
        PlayerPrefs.SetInt("lastUnlockedLevel",lastUnlockedLevel++);
        SceneManager.LoadScene("Game");
    }

    private void NextLevelButton()
    {
        PlayerPrefs.SetInt("lastUnlockedLevel",lastUnlockedLevel++);
        PlayerPrefs.SetInt("chosenLevel", PlayerPrefs.GetInt("chosenLevel") + 1);
        SceneManager.LoadScene("Game");
    }
    
    private void ChangeGearButton()
    {
        switch (gearButton.GetComponentInChildren<TMP_Text>().text)
        {
            case "D": 
                gearButton.GetComponentInChildren<TMP_Text>().text = "P";
                break;
            case "P": 
                gearButton.GetComponentInChildren<TMP_Text>().text = "R";
                break;
            case "R": 
                gearButton.GetComponentInChildren<TMP_Text>().text = "D";
                break;
            
        }
    }

    private void OnDestroy()
    {
        nextLevelButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        menuButton.onClick.RemoveAllListeners();
        gearButton.onClick.RemoveAllListeners();
        pauseButton.onClick.RemoveAllListeners();
        resumeButton.onClick.RemoveAllListeners();
    }
}
