using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public List<GameObject> cars; // List of cars in the garage
    public int currentCarIndex = 0;
    public GarageCarFlip _garageCarFlip;
    public CarSelectionManager _carSelectionManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Voil()
    {
        currentCarIndex = _garageCarFlip.currentCarIndex;
        _carSelectionManager.currentUpgradeCar = cars[currentCarIndex].GetComponent<UpgradeCar>();
    }
}
