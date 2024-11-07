using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMotor : MonoBehaviour
{
    public UpgradeCar currentUpgradeCar;
    public CarSelectionManager _carSelectionManager;


    public void ApplyLevel(int maxSpeed)
    {
        currentUpgradeCar = _carSelectionManager.currentUpgradeCar;
        currentUpgradeCar.maxSpeed = maxSpeed;
    }

    public void ApplyLevelTarget(int health)
    {
        currentUpgradeCar = _carSelectionManager.currentUpgradeCar;
        currentUpgradeCar.health = health;
    }

}
