using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public List<GameObject> unlockedCars = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
        LoadData();
    }

    public void SaveData()
    {
        SaveUnlockedCars save = new SaveUnlockedCars
        {
            tempCars = unlockedCars,
        };

        string json = JsonUtility.ToJson(save);
        SaveUnlockedCars loadedObject = JsonUtility.FromJson<SaveUnlockedCars>(json);
        File.WriteAllText(Application.dataPath + "/Data/UnlockedCarsData.json", json);
        
    } 

    private void LoadData()
    {
        if (File.Exists(Application.dataPath + "/Data/UnlockedCarsData.json"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/Data/UnlockedCarsData.json");
            SaveUnlockedCars saveUnlockedCars = JsonUtility.FromJson<SaveUnlockedCars>(saveString);
            GameManager.Instance.unLockedCars = saveUnlockedCars.tempCars;
        }
        else
            Debug.Log("Couldn't Find Data File");
    }
}

public class SaveUnlockedCars
{
    public List<GameObject> tempCars = new List<GameObject>();
}


