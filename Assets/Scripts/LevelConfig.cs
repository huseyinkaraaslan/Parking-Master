using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Level / New Level", fileName = "New Level")]
public class LevelConfig : ScriptableObject
{
    public Vector3 carSpawnPoint;
    public Vector3 carParkingPoint;
}
