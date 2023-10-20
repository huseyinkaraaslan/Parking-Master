using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Text (Car Informations)")]
    public TMP_Text carPrice;
    public TMP_Text carMaxSpeed;
    public TMP_Text carBraking;
    public TMP_Text carAcceleration;

    [Header("Car Config")]
    public List<CarConfig> carInformations = new List<CarConfig>();

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
    public Vector3 carPoint;

    private void Start()
    {
        nextCarButton.onClick.AddListener(NextCar);
        previousCarButton.onClick.AddListener(PreviousCar);

        currentCar = Instantiate(GameManager.Instance.lockedCars[0], carPoint, Quaternion.identity);
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

    private void UpdateCarOnStand()
    {
        if (currentCar != null)
        {
            GameObject oldCar = currentCar;
            Destroy(currentCar);
            currentCar = Instantiate(GameManager.Instance.lockedCars[currentCarValue],carPoint, 
                oldCar.transform.rotation);
            GetCarInformation(carInformations[currentCarValue]);
        }   
    }

    private void GetCarInformation(CarConfig car)
    {
        carPrice.text = car.price.ToString();
        carMaxSpeed.text = car.maxSpeed.ToString();
        carBraking.text = car.braking.ToString();
        carAcceleration.text = car.acceleration.ToString();
    }

    private void SelectCar()
    {
        GameManager.Instance.choosenCar = currentCarValue;
    }

    private void OnDestroy()
    {
        nextCarButton.onClick.RemoveAllListeners();
        previousCarButton.onClick.RemoveAllListeners();
    }
}
