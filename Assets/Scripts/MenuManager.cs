using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Gameobject")]
    private GameObject currentCar;

    public GameObject carStand;
    public GameObject garage;
    public GameObject levels;
    
    [Header("Button")]
    public Button nextCarButton;
    public Button previousCarButton;
    public Button buyCarButton;
    public Button selectButton;
    public Button playButton;

    [Header("Text (Car Informations)")]
    public TMP_Text carPrice;
    public TMP_Text money;

    [Header("Slider")]
    public Slider carMaxSpeedSlider;
    public Slider carBrakingSlider;
    public Slider carAccelerationSlider;

    [Header("Lock Sprite")]
    public Image lockImage;

    [Header("Variable")]
    private int currentCarIndex;
    public int currentCarValue 
    { 
        get 
        { 
            return currentCarIndex; 
        }
        set 
        { 
            currentCarIndex = value;
            UpdateCarOnStand(); 
        } 
    }

    private void Awake()
    {
        garage.SetActive(true);
        levels.SetActive(false);
    }

    private void Start()
    {
        nextCarButton.onClick.AddListener(NextCar);
        previousCarButton.onClick.AddListener(PreviousCar);
        buyCarButton.onClick.AddListener(BuyCar);
        selectButton.onClick.AddListener(SelectCar);
        playButton.onClick.AddListener(Play);

        money.text = GameManager.Instance.money.ToString();
        currentCar = Instantiate(GameManager.Instance.lockedCars[0], new Vector3(0, 5, 0), Quaternion.identity);
        GetCarInformation(GameManager.Instance.carInformations[currentCarValue]);
    }
    
    private void FixedUpdate()
    {
        carStand.transform.Rotate(Vector3.up * Time.deltaTime * 10);
        currentCar.transform.Rotate(Vector3.up * Time.deltaTime * 10);
    }

    private void PreviousCar()
    {
        currentCarValue = currentCarValue == 0 ? GameManager.Instance.lockedCars.Count - 1 : --currentCarValue;
    }

    private void NextCar()
    {
        currentCarValue = currentCarValue == GameManager.Instance.lockedCars.Count - 1 ? 0 : ++currentCarValue;
    }

    private void BuyCar()
    {
        if(GameManager.Instance.money >= GameManager.Instance.carInformations[currentCarValue].price)
        {
            GameManager.Instance.unLockedCars[currentCarValue] = GameManager.Instance.lockedCars[currentCarValue];           
            DataManager.Instance.unlockedCars = GameManager.Instance.unLockedCars;
            DataManager.Instance.SaveData();
            GetCarInformation(GameManager.Instance.carInformations[currentCarValue]);
            
            GameManager.Instance.money -= GameManager.Instance.carInformations[currentCarValue].price;
            money.text = GameManager.Instance.money.ToString();
        }
    }

    private void SelectCar()
    {
        carPrice.text = "Selected";
        PlayerPrefs.SetInt("chosenCar", currentCarValue);
        GetCarInformation(GameManager.Instance.carInformations[currentCarValue]);
    }
    private void Play()
    {
        garage.SetActive(false);      
        levels.SetActive(true);
    }

    private void UpdateCarOnStand()
    {
        if (currentCar != null)
        {
            GameObject oldCar = currentCar;
            Destroy(currentCar);
            currentCar = Instantiate(GameManager.Instance.lockedCars[currentCarValue],new Vector3(0,5,0), 
                oldCar.transform.rotation);
            GetCarInformation(GameManager.Instance.carInformations[currentCarValue]);
        }   
    }

    private void GetCarInformation(CarConfig car)
    {
        int chosenCar = PlayerPrefs.GetInt("chosenCar");
        foreach(GameObject tempCar in GameManager.Instance.unLockedCars)
        {
            if(tempCar != null)
            {
                if (currentCar.name.IndexOf(tempCar.name) == -1)
                {
                    buyCarButton.gameObject.SetActive(true);
                    selectButton.gameObject.SetActive(false);
                    playButton.gameObject.SetActive(false);
                    lockImage.enabled = true;
                    carPrice.text = car.price + " $";                   
                }
                else if (tempCar.name == GameManager.Instance.unLockedCars[chosenCar].name)
                {
                    buyCarButton.gameObject.SetActive(false);
                    selectButton.gameObject.SetActive(false);
                    playButton.gameObject.SetActive(true);
                    lockImage.enabled = false;
                    carPrice.text = "Selected";
                    break;
                }
                else
                {
                    buyCarButton.gameObject.SetActive(false);
                    selectButton.gameObject.SetActive(true);
                    playButton.gameObject.SetActive(false);
                    lockImage.enabled = false;
                    carPrice.text = "Owned";
                    break;
                }
            }

        }           
        carMaxSpeedSlider.maxValue = carBrakingSlider.maxValue = carAccelerationSlider.maxValue = 100;

        carMaxSpeedSlider.value = car.maxSpeed;
        carBrakingSlider.value = car.braking;
        carAccelerationSlider.value = car.acceleration;
    }

    private void OnDestroy()
    {
        nextCarButton.onClick.RemoveAllListeners();
        previousCarButton.onClick.RemoveAllListeners();
        buyCarButton.onClick.RemoveAllListeners();
        selectButton.onClick.RemoveAllListeners();
        playButton.onClick.RemoveAllListeners();
    }

    
}
