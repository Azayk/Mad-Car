using UnityEngine;
using UnityEngine.UI;

public class EngineConfigManager : MonoBehaviour
{
    public UnityEngine.UI.Text fileValueText;
    public UnityEngine.UI.Button resetButton;

    void Start()
    {
        DisplayFileValue();
        resetButton.onClick.AddListener(ResetRecord);
    }

    void DisplayFileValue()
    {
        float maxRecord = PlayerPrefs.GetFloat("MaxRecord", 0f);
        fileValueText.text = maxRecord.ToString();
    }

    void ResetRecord()
    {
        PlayerPrefs.SetFloat("MaxRecord", 0f);
        PlayerPrefs.Save();
        DisplayFileValue();
    }
}