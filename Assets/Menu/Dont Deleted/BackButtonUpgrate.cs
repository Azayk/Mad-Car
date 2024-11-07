using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonUpgrate : MonoBehaviour
{
    public CarSelectionManager _carSelectionManager;

    // This method will be called when the back button is pressed
    public void Button(bool pressed)
    {
        if (pressed)
        {
            _carSelectionManager.currentUpgradeCar = null; // Clear the reference when going back
        }
    }
}