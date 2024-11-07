using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectLVL : MonoBehaviour
{
    public CarSelectionManager _carSelectionManager;
    

    public int lvlUP; // This value is set in the inspector for each button

    // Method to change the level of the currently selected car
    public void ApplyLevel()
    {
        _carSelectionManager.UpdateLevelFromUI(lvlUP);
    }
}