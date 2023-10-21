using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Gameobject")]
    public GameObject carStand;
    private GameObject currentCar;

    [Header("Button")]
    public Button nextCarButton;
    public Button previousCarButton;
    public Button buyCarButton;

    [Header("Text (Car Informations)")]
    public TMP_Text carPrice;
    public TMP_Text carMaxSpeed;
    public TMP_Text carBraking;
    public TMP_Text carAcceleration;

    [Header("Slider")]
    public Slider carMaxSpeedSlider;
    public Slider carBrakingSlider;
    public Slider carAccelerationSlider;

    [Header("Car Config")]
    public List<CarConfig> carInformations = new List<CarConfig>();

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

    private void Start()
    {
        nextCarButton.onClick.AddListener(NextCar);
        previousCarButton.onClick.AddListener(PreviousCar);
        buyCarButton.onClick.AddListener(BuyCar);

        currentCar = Instantiate(GameManager.Instance.lockedCars[0], new Vector3(0, 5, 0), Quaternion.identity);
        GetCarInformation(carInformations[currentCarValue]);
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
        if(GameManager.Instance.money >= carInformations[currentCarValue].price)
        {
            GameManager.Instance.unLockedCars[currentCarValue] = GameManager.Instance.lockedCars[currentCarValue];
            UpdateCarOnStand();
        }
    }

    private void UpdateCarOnStand()
    {
        if (currentCar != null)
        {
            GameObject oldCar = currentCar;
            Destroy(currentCar);
            currentCar = Instantiate(GameManager.Instance.lockedCars[currentCarValue],new Vector3(0,5,0), 
                oldCar.transform.rotation);
            GetCarInformation(carInformations[currentCarValue]);
        }   
    }

    private void GetCarInformation(CarConfig car)
    {
        foreach(GameObject tempCar in GameManager.Instance.unLockedCars)
        {
            if(tempCar != null)
            {
                if (currentCar.name.IndexOf(tempCar.name) == -1)
                {
                    buyCarButton.gameObject.SetActive(true);
                    carPrice.text = car.price + " $";
                    lockImage.enabled = true;
                }
                else
                {
                    buyCarButton.gameObject.SetActive(false);
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

    private void SelectCar()
    {
        GameManager.Instance.choosenCar = currentCarValue;
    }

    private void OnDestroy()
    {
        nextCarButton.onClick.RemoveAllListeners();
        previousCarButton.onClick.RemoveAllListeners();
        buyCarButton.onClick.RemoveAllListeners();
    }
}
