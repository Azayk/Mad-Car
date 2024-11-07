#if UNITY_EDITOR
using UnityEditor; // Для работы с префабами в редакторе
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectionManager : MonoBehaviour
{
    public List<GameObject> cars; // Список автомобилей в сцене
    public List<GameObject> carPrefabs; // Префабы автомобилей
    public int currentCarIndex = 0; // Индекс текущего автомобиля
    public int lvl; // Переменная уровня, которая применяется к каждому автомобилю

    public GarageCarFlip _garageCarFlip;
    public UpgradeCar currentUpgradeCar; // Ссылка на UpgradeCar скрипт текущего автомобиля

    void Start()
    {
        // Инициализируем с первым автомобилем и его уровнем
        currentCarIndex = _garageCarFlip.currentCarIndex;
        currentUpgradeCar = cars[currentCarIndex].GetComponent<UpgradeCar>();
        lvl = currentUpgradeCar.lvl; // Инициализируем уровень текущего автомобиля
    }

    void Update()
    {
        // Обновляем, если изменился автомобиль
        int newCarIndex = _garageCarFlip.currentCarIndex;
        if (newCarIndex != currentCarIndex)
        {
            currentCarIndex = newCarIndex;
            currentUpgradeCar = cars[currentCarIndex].GetComponent<UpgradeCar>();
            lvl = currentUpgradeCar.lvl; // Синхронизируем уровень с новым автомобилем
        }
    }

    // Метод для обновления уровня через UI
    public void UpdateLevelFromUI(int newLvl)
    {
        if (currentUpgradeCar != null)
        {
            lvl = newLvl;
            currentUpgradeCar.lvl = lvl; // Применяем новый уровень к текущему автомобилю

            // Сохраняем изменения в префабе
            SaveChangesToPrefab(currentCarIndex);
        }
    }

    // Метод для сохранения изменений в префаб напрямую
    void SaveChangesToPrefab(int carIndex)
    {
#if UNITY_EDITOR
        string prefabPath = AssetDatabase.GetAssetPath(carPrefabs[carIndex]);

        if (!string.IsNullOrEmpty(prefabPath))
        {
            // Загружаем содержимое префаба для редактирования
            GameObject prefabContents = PrefabUtility.LoadPrefabContents(prefabPath);

            // Находим компонент UpgradeCar в загруженном префабе
            UpgradeCar prefabUpgradeCar = prefabContents.GetComponent<UpgradeCar>();
            if (prefabUpgradeCar != null)
            {
                // Обновляем уровень в префабе
                prefabUpgradeCar.lvl = currentUpgradeCar.lvl;

                // Сохраняем изменения обратно в префаб
                PrefabUtility.SaveAsPrefabAsset(prefabContents, prefabPath);

                Debug.Log("Level applied to prefab and saved: " + prefabUpgradeCar.lvl);
            }

            // Освобождаем загруженный контент префаба
            PrefabUtility.UnloadPrefabContents(prefabContents);
        }
        else
        {
            Debug.LogWarning("Prefab path is invalid or null.");
        }
#endif
    }
}