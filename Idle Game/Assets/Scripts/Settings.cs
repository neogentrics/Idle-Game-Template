using UnityEngine;
using TMPro;
using System;

public class Settings : MonoBehaviour
{
    public static Settings instance;
    public void Awake() => instance = this;

    public string[] NotationNames;

    public TMP_Text[] SettingText;

    public GameObject[] SettingsPanels;

    public void StartSettings()
    {
        NotationNames = new[] {"Standard", "Scientific", "Engineering", "Logarithmic" };
        Methods.Notation = Controller.Instance.data.Notation;
        SyncSettings();
    }

    public void ChangeSettings(string settingName)
    {
        var flame = Controller.Instance.data;

        switch (settingName)
        {
            case "Notation":
                flame.Notation++;
                if (flame.Notation > NotationNames.Length -1) flame.Notation = 0;
                Methods.Notation = flame.Notation;
            break;
        }

        SyncSettings(settingName);
       // UpgradesManager.upgradesManager.UpdateUpgradeUI();
    }

    public void SyncSettings(string settingName = "")
    {
        var flame = Controller.Instance.data;

        if (settingName == string.Empty)
        {
            SettingText[0].text = "Notation: \n" + NotationNames[flame.Notation];
            return;
        }

        switch (settingName)
        {
            case "Notation":
                SettingText[0].text = "Notation: \n" + NotationNames[flame.Notation];
                break;
        }
    }

    public void NavigateSettings(string location)
    {
        foreach (var panel in SettingsPanels) panel.SetActive(false);

        switch (location)
        {
            case "Save":
                SettingsPanels[0].SetActive(true);
                break;
            case "Main":
                SettingsPanels[1].SetActive(true);
                break;
        }
                       
    }

}
