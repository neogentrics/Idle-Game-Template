using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager Instance;

    private void Awake() => Instance = this;

    public List<Upgrades> clickUpgrades;
    public Upgrades clickUpgradesPrefab;

    public List<Upgrades> productionUpgrades;
    public Upgrades productionUpgradesPrefab;

    public ScrollRect clickUpgradesScroll;
    public Transform clickUpgradesPanel;

    public ScrollRect productionUpgradesScroll;
    public Transform productionUpgradesPanel;

    public string[] clickUpgradesName;
    public string[] productionUpgradesName;
    
    public BigDouble[] clickUpgradesBaseCost;
    public BigDouble[] clickUpgradesCostMulti;
    public BigDouble[] clickUpgradesBasePower;

    public BigDouble[] productionUpgradesBaseCost;
    public BigDouble[] productionUpgradesCostMulti;
    public BigDouble[] productionUpgradesBasePower;


    public void StartUpgradeManager()
    {
        Methods.UpgradeCheck(Controller.Instance.data.clickUpgradesLevel, length: 4);

        clickUpgradesName = new[] { "Click Power +1", "Click Power +5", "Click Power +10", "Click Power +25"};

        productionUpgradesName = new[]
        {
            "+1 Flask/s",
            "+2 Flask/s",
            "+10 Flask/s",
            "+100 Flask/s",
        };

        clickUpgradesBaseCost = new BigDouble[] {10, 50, 100, 1000};
        clickUpgradesCostMulti = new BigDouble[] {1.25, 1.35, 1.55, 2};
        clickUpgradesBasePower = new BigDouble[] {1, 5, 10, 25};

        productionUpgradesBaseCost = new BigDouble[] {25, 100, 1000, 10000};
        productionUpgradesCostMulti = new BigDouble[] {1.5, 1.75, 2, 3};
        productionUpgradesBasePower = new BigDouble[] {1, 2, 10, 100};

        for (int i = 0; i < Controller.Instance.data.clickUpgradesLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(clickUpgradesPrefab, clickUpgradesPanel);
            upgrade.UpgradeID = i;
            clickUpgrades.Add(upgrade);
        }

        for (int i = 0; i < Controller.Instance.data.productionUpgradesLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(productionUpgradesPrefab, productionUpgradesPanel);
            upgrade.UpgradeID = i;
            productionUpgrades.Add(upgrade);
        }

        clickUpgradesScroll.normalizedPosition = new Vector2(0, 0);
        productionUpgradesScroll.normalizedPosition = new Vector2(0, 0);


        UpdateUpgradeUI("click");
        UpdateUpgradeUI("production");

    }


    public void UpdateUpgradeUI(string type, int UpgradeID = -1)
    {
        var data = Controller.Instance.data;

        switch (type)
        {
            case "click":
                if (UpgradeID == -1)
                    for (int i = 0; i < clickUpgrades.Count; i++) UpgradeUI(clickUpgrades, data.clickUpgradesLevel, clickUpgradesName, i);
                else UpgradeUI(clickUpgrades, data.clickUpgradesLevel, clickUpgradesName,  UpgradeID);
                break;
            case "production":
                if (UpgradeID == -1)
                    for (int i = 0; i < productionUpgrades.Count; i++) UpgradeUI(productionUpgrades, data.productionUpgradesLevel, productionUpgradesName, i);
                else UpgradeUI(productionUpgrades, data.productionUpgradesLevel, productionUpgradesName, UpgradeID);
                break;
        }

        void UpgradeUI(List<Upgrades> upgrades,List<int> upgradeLevels , string[] upgradeNames, int ID)
        {
            upgrades[ID].LevelText.text = upgradeLevels[ID].ToString();
            upgrades[ID].CostText.text = $"Cost: {UpgradeCost(type, ID):F2} Flasks";
            upgrades[ID].NameText.text = upgradeNames[ID];
        }


    }

    public BigDouble UpgradeCost(string type, int UpgradeID)
    {
        var data = Controller.Instance.data;
        
        switch (type)
        {
            case "click":
                return clickUpgradesBaseCost[UpgradeID]
            * BigDouble.Pow(clickUpgradesCostMulti[UpgradeID], power: (BigDouble)data.clickUpgradesLevel[UpgradeID]);
            case "production":
                return productionUpgradesBaseCost[UpgradeID]
            * BigDouble.Pow(productionUpgradesCostMulti[UpgradeID], power: (BigDouble)data.productionUpgradesLevel[UpgradeID]);

        }

        return 0;

   }
    public void BuyUpgrade(string type, int UpgradeID)
    {
        var data = Controller.Instance.data;       
        
        switch (type)
        {
            case "click": Buy(data.clickUpgradesLevel);
                break;
            case "production":
                Buy(data.productionUpgradesLevel);
                break;
        }

        void Buy(List<int> upgradeLevels)
        {
            if (data.flasks >= UpgradeCost(type, UpgradeID))
            {
                data.flasks -= UpgradeCost(type, UpgradeID);
                upgradeLevels[UpgradeID] += 1;
            }

            UpdateUpgradeUI(type, UpgradeID);
        }

    }
    

}
