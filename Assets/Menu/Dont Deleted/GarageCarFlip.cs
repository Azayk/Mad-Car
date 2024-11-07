using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageCarFlip : MonoBehaviour
{
    public GameObject[] cars; // ������ �����
    public int currentCarIndex = 0; // ������ ������� ������
    public GarageCarFlip _garageCarFlip;


    void Start()
    {
        // ���������� ������ ������ ��� �������
        ActivateCar(currentCarIndex);
    }

    // ����� ��� ������������ �� ��������� ������
    public void NextCar()
    {
        // ������������ ������� ������
        cars[currentCarIndex].SetActive(false);

        // ����������� ������ � ��������� ������� �������
        currentCarIndex = (currentCarIndex + 1) % cars.Length;

        // ���������� ��������� ������
        ActivateCar(currentCarIndex);
    }

    public void currentCar(bool increment)
    {
        _garageCarFlip.currentCarIndex = currentCarIndex;
    }

    // ����� ��� ������������ �� ���������� ������
    public void PreviousCar()
    {
        // ������������ ������� ������
        cars[currentCarIndex].SetActive(false);

        // ��������� ������ � ��������� ������� �������
        currentCarIndex--;
        if (currentCarIndex < 0)
        {
            currentCarIndex = cars.Length - 1;
        }

        // ���������� ���������� ������
        ActivateCar(currentCarIndex);
    }

    // ����� ��� ��������� ������ �� �������
    private void ActivateCar(int index)
    {
        cars[index].SetActive(true);
    }
}
