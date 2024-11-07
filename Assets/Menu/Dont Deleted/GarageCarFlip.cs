using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageCarFlip : MonoBehaviour
{
    public GameObject[] cars; // Массив машин
    public int currentCarIndex = 0; // Индекс текущей машины
    public GarageCarFlip _garageCarFlip;


    void Start()
    {
        // Активируем первую машину при запуске
        ActivateCar(currentCarIndex);
    }

    // Метод для переключения на следующую машину
    public void NextCar()
    {
        // Деактивируем текущую машину
        cars[currentCarIndex].SetActive(false);

        // Увеличиваем индекс и проверяем границы массива
        currentCarIndex = (currentCarIndex + 1) % cars.Length;

        // Активируем следующую машину
        ActivateCar(currentCarIndex);
    }

    public void currentCar(bool increment)
    {
        _garageCarFlip.currentCarIndex = currentCarIndex;
    }

    // Метод для переключения на предыдущую машину
    public void PreviousCar()
    {
        // Деактивируем текущую машину
        cars[currentCarIndex].SetActive(false);

        // Уменьшаем индекс и проверяем границы массива
        currentCarIndex--;
        if (currentCarIndex < 0)
        {
            currentCarIndex = cars.Length - 1;
        }

        // Активируем предыдущую машину
        ActivateCar(currentCarIndex);
    }

    // Метод для активации машины по индексу
    private void ActivateCar(int index)
    {
        cars[index].SetActive(true);
    }
}
