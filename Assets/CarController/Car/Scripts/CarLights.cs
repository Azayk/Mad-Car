using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarLights : MonoBehaviour
{
    public enum Side
    {
        Front,
        Back
    }

    [System.Serializable]
    public struct Light
    {
        public GameObject lightObj;
        public Material lightMat;
        public Side side;
    }

    

    public bool isFrontLightOn;
    public bool isBackLightOn;

    public Color frontLightOnColor;
    public Color frontLightOffColor;
    public Color backLightOnColor;
    public Color backLightOffColor;

    public List<Light> lights;

    void Start()
    {
        
        isBackLightOn = false;
    }

    public void OperateFrontLights()
    {
        isFrontLightOn = !isFrontLightOn;

        if (isFrontLightOn)
        {
            //Turn On Lights
            foreach (var light in lights)
            {
                if (light.side == Side.Front && light.lightObj.activeInHierarchy == false)
                {
                    light.lightObj.SetActive(true);
                    light.lightMat.color = frontLightOnColor;
                }
            }

            
        }
        else
        {
            //Turn Off Lights
            foreach (var light in lights)
            {
                if (light.side == Side.Front && light.lightObj.activeInHierarchy == true)
                {
                    light.lightObj.SetActive(false);
                    light.lightMat.color = frontLightOffColor;
                }
            }

            
        }
    }

    public void OperateBackLights()
    {
        if (isBackLightOn)
        {
            //Turn On Lights
            foreach (var light in lights)
            {
                if (light.side == Side.Back && light.lightObj.activeInHierarchy == false)
                {
                    light.lightObj.SetActive(true);
                    light.lightMat.color = backLightOnColor;
                }
            }
        }
        else
        {
            //Turn Off Lights
            foreach (var light in lights)
            {
                if (light.side == Side.Back && light.lightObj.activeInHierarchy == true)
                {
                    light.lightObj.SetActive(false);
                    light.lightMat.color = backLightOffColor;
                }
            }
        }
    }
}