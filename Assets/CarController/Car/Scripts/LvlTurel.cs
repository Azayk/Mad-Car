using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlTurel : MonoBehaviour
{
    public UpgradeCar _upgradeCar;

    public int lvl;


    public GameObject turel1;
    public GameObject turel2;
    public GameObject turel3;
    public GameObject turel4;

    public Transform camTarget;
    
    public Transform turelT1;
    public Transform turelT2;
    public Transform turelT3;
    public Transform turelT4;

    public CameraFollow cameraFollow;
    // Start is called before the first frame update
    void Start()
    {
        lvl = _upgradeCar.lvl;

        if (lvl == 1)
        {
            turel1.SetActive(true);
            camTarget = turelT1;
        }

        else if (lvl == 2)
        {
            turel2.SetActive(true);
            camTarget = turelT2;
        }

        else if (lvl == 3)
        {
            turel3.SetActive(true);
            camTarget = turelT3;
        }

        else if (lvl == 4)
        {
            turel4.SetActive(true);
            camTarget = turelT4;
        }

        cameraFollow.carTarget = camTarget;
    }

    
}
