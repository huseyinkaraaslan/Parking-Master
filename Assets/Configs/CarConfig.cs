using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Car / New Car Information", fileName = "Car Information")]
public class CarConfig : ScriptableObject
{
    public int price;
    public int maxSpeed;
    public int braking;
    public int acceleration;
}
