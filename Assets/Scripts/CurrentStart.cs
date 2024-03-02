using UIParty;
using UnityEngine;

public class CurrentStart : MonoBehaviour
{
    public static CurrentStart instance;

    [SerializeField] DisplayUI currentUI;

    public int currentValue { get; private set; }

    public void Awake()
    {
        instance = this;
        currentValue = PlayerPrefs.GetInt("currentValue",0);
        UpdateText();
    }

    public void AddValue(int value)
    {
        currentValue += value;
        UpdateText();
    }

    private void UpdateText()
    {
        currentUI.SetDisplay(currentValue.ToString("00"));
    }
}
