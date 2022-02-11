using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Navagation : MonoBehaviour
{
    public GameObject ClickUpgradesSelected;
    public GameObject ProductionUpgradesSelected;

    public TMP_Text ClickUpgradesTitleText;
    public TMP_Text ProductionUpgradesTitleText;

    public void SwitchUpgrades(string location)
    {
        UpgradesManager.Instance.clickUpgradesScroll.gameObject.SetActive(false);
        UpgradesManager.Instance.productionUpgradesScroll.gameObject.SetActive(false);

        ClickUpgradesSelected.SetActive(false);
        ProductionUpgradesSelected.SetActive(false);

        ClickUpgradesTitleText.color = Color.grey;
        ProductionUpgradesTitleText.color = Color.grey;

        switch(location)
        {
            case "Click":
                UpgradesManager.Instance.clickUpgradesScroll.gameObject.SetActive(true);
                ClickUpgradesSelected.SetActive(true);
                ClickUpgradesTitleText.color = Color.white;
                break;
            case "Production":
                UpgradesManager.Instance.productionUpgradesScroll.gameObject.SetActive(true);
                ProductionUpgradesSelected.SetActive(true);
                ProductionUpgradesTitleText.color = Color.white;
                break;
        }
    }

}
