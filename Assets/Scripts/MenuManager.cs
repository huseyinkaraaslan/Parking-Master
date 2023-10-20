using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button nextCarButton;
    public Button previousCarButton;
    public GameObject carStand;
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

    private GameObject currentCar;
    public Vector3 carPoint;
    private void Start()
    {
        nextCarButton.onClick.AddListener(nextCar);
        previousCarButton.onClick.AddListener(previousCar);

        if (currentCar == null)
            currentCar = Instantiate(GameManager.Instance.lockedCars[0]);
  
        currentCar.transform.position = carPoint;

    }

    private void Update()
    {
        carStand.transform.Rotate(Vector3.up * Time.deltaTime * 10);
        if(currentCar != null)
            currentCar.transform.Rotate(Vector3.up * Time.deltaTime * 10);
    }

    private void previousCar()
    {
        currentCarValue = currentCarValue == 0 ? 12 : --currentCarValue;
    }

    private void nextCar()
    {
        currentCarValue = currentCarValue == 12 ? 0 : ++currentCarValue;
    }

    private void UpdateCarOnStand()
    {
        if (currentCar != null)
        {
            GameObject oldCar = currentCar;
            Destroy(currentCar);
            currentCar = Instantiate(GameManager.Instance.lockedCars[currentCarValue],carPoint, 
                oldCar.transform.rotation);
        }   
    }

    private void SelectCar()
    {
        GameManager.Instance.choosenCar = currentCarValue;
    }
}
