#if UNITY_EDITOR
using UnityEditor; // ��� ������ � ��������� � ���������
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectionManager : MonoBehaviour
{
    public List<GameObject> cars; // ������ ����������� � �����
    public List<GameObject> carPrefabs; // ������� �����������
    public int currentCarIndex = 0; // ������ �������� ����������
    public int lvl; // ���������� ������, ������� ����������� � ������� ����������

    public GarageCarFlip _garageCarFlip;
    public UpgradeCar currentUpgradeCar; // ������ �� UpgradeCar ������ �������� ����������

    void Start()
    {
        // �������������� � ������ ����������� � ��� �������
        currentCarIndex = _garageCarFlip.currentCarIndex;
        currentUpgradeCar = cars[currentCarIndex].GetComponent<UpgradeCar>();
        lvl = currentUpgradeCar.lvl; // �������������� ������� �������� ����������
    }

    void Update()
    {
        // ���������, ���� ��������� ����������
        int newCarIndex = _garageCarFlip.currentCarIndex;
        if (newCarIndex != currentCarIndex)
        {
            currentCarIndex = newCarIndex;
            currentUpgradeCar = cars[currentCarIndex].GetComponent<UpgradeCar>();
            lvl = currentUpgradeCar.lvl; // �������������� ������� � ����� �����������
        }
    }

    // ����� ��� ���������� ������ ����� UI
    public void UpdateLevelFromUI(int newLvl)
    {
        if (currentUpgradeCar != null)
        {
            lvl = newLvl;
            currentUpgradeCar.lvl = lvl; // ��������� ����� ������� � �������� ����������

            // ��������� ��������� � �������
            SaveChangesToPrefab(currentCarIndex);
        }
    }

    // ����� ��� ���������� ��������� � ������ ��������
    void SaveChangesToPrefab(int carIndex)
    {
#if UNITY_EDITOR
        string prefabPath = AssetDatabase.GetAssetPath(carPrefabs[carIndex]);

        if (!string.IsNullOrEmpty(prefabPath))
        {
            // ��������� ���������� ������� ��� ��������������
            GameObject prefabContents = PrefabUtility.LoadPrefabContents(prefabPath);

            // ������� ��������� UpgradeCar � ����������� �������
            UpgradeCar prefabUpgradeCar = prefabContents.GetComponent<UpgradeCar>();
            if (prefabUpgradeCar != null)
            {
                // ��������� ������� � �������
                prefabUpgradeCar.lvl = currentUpgradeCar.lvl;

                // ��������� ��������� ������� � ������
                PrefabUtility.SaveAsPrefabAsset(prefabContents, prefabPath);

                Debug.Log("Level applied to prefab and saved: " + prefabUpgradeCar.lvl);
            }

            // ����������� ����������� ������� �������
            PrefabUtility.UnloadPrefabContents(prefabContents);
        }
        else
        {
            Debug.LogWarning("Prefab path is invalid or null.");
        }
#endif
    }
}