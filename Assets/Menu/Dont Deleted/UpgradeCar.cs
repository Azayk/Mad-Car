using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCar : MonoBehaviour
{
    public int lvl;

    public GameObject turel1;
    public GameObject turel2;
    public GameObject turel3;
    public GameObject turel4;
    public float maxSpeed = 200f;

    public float health = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lvl == 1)
        {
            turel1.SetActive(true);
            turel2.SetActive(false);
            turel3.SetActive(false);
            turel4.SetActive(false);

        }

        else if (lvl == 2)
        {
            turel2.SetActive(true);
            turel1.SetActive(false);
            turel3.SetActive(false);
            turel4.SetActive(false);

        }

        else if (lvl == 3)
        {
            turel3.SetActive(true);
            turel2.SetActive(false);
            turel1.SetActive(false);
            turel4.SetActive(false);

        }

        else if (lvl == 4)
        {
            turel4.SetActive(true);
            turel2.SetActive(false);
            turel3.SetActive(false);
            turel1.SetActive(false);

        }
    }


}
