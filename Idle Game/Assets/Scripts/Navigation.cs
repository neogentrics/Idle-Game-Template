using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{
    public GameObject ClickUpgradesSelected;
    public GameObject ProductionUpgradesSelected;
    public GameObject GeneratorUpgradesSelected;

    public TMP_Text ClickUpgradesTitleText;
    public TMP_Text ProductionUpgradesTitleText;
    public TMP_Text GeneratorUpgradesTitleText;

    public GameObject HomeScreen;
    public GameObject SettingsScreen;

    public void SwitchUpgrades(string location)
    {
        UpgradesManager.Instance.UpgradeHandlers[0].UpgradesScroll.gameObject.SetActive(false);
        UpgradesManager.Instance.UpgradeHandlers[1].UpgradesScroll.gameObject.SetActive(false);
        UpgradesManager.Instance.UpgradeHandlers[2].UpgradesScroll.gameObject.SetActive(false);

        ClickUpgradesSelected.SetActive(false);
        ProductionUpgradesSelected.SetActive(false);
        GeneratorUpgradesSelected.SetActive(false);

        ClickUpgradesTitleText.color = Color.grey;
        ProductionUpgradesTitleText.color = Color.grey;
        GeneratorUpgradesTitleText.color = Color.grey;

        switch (location)
        {
            case "Click":
                UpgradesManager.Instance.UpgradeHandlers[0].UpgradesScroll.gameObject.SetActive(true);
                ClickUpgradesSelected.SetActive(true);
                ClickUpgradesTitleText.color = Color.white;
                break;
            case "Production":
                UpgradesManager.Instance.UpgradeHandlers[1].UpgradesScroll.gameObject.SetActive(true);
                ProductionUpgradesSelected.SetActive(true);
                ProductionUpgradesTitleText.color = Color.white;
                break;
            case "Generator":
                UpgradesManager.Instance.UpgradeHandlers[2].UpgradesScroll.gameObject.SetActive(true);
                GeneratorUpgradesSelected.SetActive(true);
                GeneratorUpgradesTitleText.color = Color.white;
                break;
        }
    }

    public void Navigate(string location)
    {
        HomeScreen.SetActive(false);
        SettingsScreen.SetActive(false);

        switch (location)
        {
            case "Home":
                HomeScreen.SetActive(true);
                break;
            case "Settings":
                SettingsScreen.SetActive(true);
                break ;
        }
    }
}
