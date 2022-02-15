using System;
using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager Instance;
    private void Awake() => Instance = this;

    public UpgradeHandler[] UpgradeHandlers;

    public void StartUpgradeManager()
    {
        var ice = Controller.Instance.data;

        Methods.UpgradeCheck(ice.clickUpgradesLevel, length: 7);
        Methods.UpgradeCheck(ice.productionUpgradesLevel, length: 7);
        Methods.UpgradeCheck(ice.generatorUpgradesLevel, length: 7);
        Methods.UpgradeCheck(ice.productionUpgradeGenerated, length: 7);

        
        UpgradeHandlers[0].UpgradeNames = new[]
        {
            "Click Power +1",
            "Click Power +5",
            "Click Power +10",
            "Click Power +25",
            "Click Power +50",
            "Click Power +100",
            "Click Power +1000"
        };

        UpgradeHandlers[1].UpgradeNames = new[]
        {
            "+2 Flask/s",
            "+5 Flask/s",
            "+10 Flask/s",
            "+100 Flask/s",
            "+200 Flask/s",
            "+400 Flask/s",
            "+800 Flask/s"
        };

        UpgradeHandlers[2].UpgradeNames = new[]
        {
            $"Produces +1 \" {UpgradeHandlers[1].UpgradeNames[0]}\" Upgrades/s",
            $"Produces +2 \" {UpgradeHandlers[1].UpgradeNames[1]}\" Upgrades/s",
            $"Produces +4 \" {UpgradeHandlers[1].UpgradeNames[2]}\" Upgrades/s",
            $"Produces +8 \" {UpgradeHandlers[1].UpgradeNames[3]}\" Upgrades/s",
            $"Produces +16 \" {UpgradeHandlers[1].UpgradeNames[4]}\" Upgrades/s",
            $"Produces +256 \" {UpgradeHandlers[1].UpgradeNames[5]}\" Upgrades/s",
            $"Produces +512 \" {UpgradeHandlers[1].UpgradeNames[6]}\" Upgrades/s"
        };

        // Click Upgrades
        UpgradeHandlers[0].UpgradeBaseCost = new BigDouble[] { 10, 50, 100, 1000, 1500, 2000, 2500 };
        UpgradeHandlers[0].UpgradeCostMulti = new BigDouble[] { 1.25, 1.35, 1.55, 2, 2.5, 3, 3.5 };
        UpgradeHandlers[0].UpgradesBasePower = new BigDouble[] { 1, 5, 10, 25, 50, 100, 1000 };
        UpgradeHandlers[0].UpgradesUnlock = new BigDouble[] { 0, 25, 50, 500, 750, 1000, 1250 };

        // Production Upgrades
        UpgradeHandlers[1].UpgradeBaseCost = new BigDouble[] { 25, 100, 1000, 2000, 4000, 8000, 10000 };
        UpgradeHandlers[1].UpgradeCostMulti = new BigDouble[] { 1.5, 1.75, 2, 2.5, 2.75, 3, 3.5 };
        UpgradeHandlers[1].UpgradesBasePower = new BigDouble[] { 2, 5, 10, 100, 200, 400, 800 };
        UpgradeHandlers[1].UpgradesUnlock = new BigDouble[] { 0, 50, 500, 1000, 2000, 4000, 5000 };

        // Generator Upgrades
        UpgradeHandlers[2].UpgradeBaseCost = new BigDouble[] { 5000, 1e4, 1e5, 1e6, 1e7, 1e8, 1e10 };
        UpgradeHandlers[2].UpgradeCostMulti = new BigDouble[] { 1.25, 1.5, 2, 2.5, 2.75, 3, 3.5 };
        UpgradeHandlers[2].UpgradesBasePower = new BigDouble[] { 1, 2, 4, 8, 16, 256, 512};
        UpgradeHandlers[2].UpgradesUnlock = new BigDouble[] { 2500, 5e3, 5e4, 5e5, 5e6, 5e7, 5e9 };

        CreateUpgrades(Controller.Instance.data.clickUpgradesLevel, index: 0);
        CreateUpgrades(Controller.Instance.data.productionUpgradesLevel, index: 1);
        CreateUpgrades(Controller.Instance.data.generatorUpgradesLevel, index: 2);

        void CreateUpgrades<T>(List<T> level, int index)
        {
            for (int i = 0; i < level.Count; i++)
            {
                Upgrades upgrade = Instantiate(UpgradeHandlers[index].UpgradesPrefab, UpgradeHandlers[index].UpgradesPanel);
                upgrade.UpgradeID = i;
                upgrade.gameObject.SetActive(false);
                UpgradeHandlers[index].Upgrades.Add(upgrade);
            }

            UpgradeHandlers[index].UpgradesScroll.normalizedPosition = new Vector2(0, 0);

        }
        UpdateUpgradeUI("click");
        UpdateUpgradeUI("production");
        UpdateUpgradeUI("generator");

    }

    public void Update()
    {
        UpgradeUnlockSystem(Controller.Instance.data.flasks, UpgradeHandlers[0].UpgradesUnlock, index: 0);
        UpgradeUnlockSystem(Controller.Instance.data.flasks, UpgradeHandlers[1].UpgradesUnlock, index: 1);
        UpgradeUnlockSystem(Controller.Instance.data.flasks, UpgradeHandlers[2].UpgradesUnlock, index: 2);

        void UpgradeUnlockSystem(BigDouble currency, BigDouble[] unlock, int index)
        {
            for (var i = 0; i < UpgradeHandlers[index].Upgrades.Count; i++)
                if (!UpgradeHandlers[index].Upgrades[i].gameObject.activeSelf)
                    UpgradeHandlers[index].Upgrades[i].gameObject.SetActive(currency >= unlock[i]);
        }

        UpgradeFillManager("click", 0);
        UpgradeFillManager("production", 1);
        UpgradeFillManager("generator", 2);

        void UpgradeFillManager(string type, int index)
        {
            if (!UpgradeHandlers[index].UpgradesScroll.gameObject.activeSelf) return;
            {
                for (var i = 0; i < UpgradeHandlers[index].Upgrades.Count; i++)
                {
                    UpgradeHandlers[index].Upgrades[i].Fill.fillAmount = Methods.Fill(Controller.Instance.data.flasks, UpgradeCost(type, i));

                    Methods.FillSmooth(ref UpgradeHandlers[index].Upgrades[i].SmoothValue, UpgradeHandlers[index].Upgrades[i].Fill.fillAmount);
                    UpgradeHandlers[index].Upgrades[i].FillSmooth.fillAmount = (float)UpgradeHandlers[index].Upgrades[i].SmoothValue;
                }
            }
        }

        

            if (UpgradeHandlers[1].UpgradesScroll.gameObject.activeSelf)
        {
            UpdateUpgradeUI("production");
        }        
    }


    public void UpdateUpgradeUI(string type, int UpgradeID = -1)
    {
        var data = Controller.Instance.data;

        switch (type)
        {
            case "click":
                UpgradeAllUI(UpgradeHandlers[0].Upgrades, data.clickUpgradesLevel, UpgradeHandlers[0].UpgradeNames, 0, UpgradeID, type);
                break;
            case "production":
                UpgradeAllUI(UpgradeHandlers[1].Upgrades, data.productionUpgradesLevel, UpgradeHandlers[1].UpgradeNames, 1, UpgradeID, type,
                    data.productionUpgradeGenerated);
                break;
            case "generator":
                UpgradeAllUI(UpgradeHandlers[2].Upgrades, data.generatorUpgradesLevel, UpgradeHandlers[2].UpgradeNames, 2, UpgradeID, type);
                break;
        }        
    }

    private void UpgradeAllUI(List<Upgrades> upgrades, List<BigDouble> upgradeLevels,string[] upgradeNames, int index, int UpgradeID,
        string type, List<BigDouble> upgradesGenerated = null)
    {
        if (UpgradeID == -1)
            for (int i = 0; i < UpgradeHandlers[index].Upgrades.Count; i++) UpdateUI(i);
        else UpdateUI(UpgradeID);

        void UpdateUI(int ID)
        {
            //BigDouble generated = upgradesGenerated == null ? 0 : upgradesGenerated[ID];
            upgrades[ID].LevelText.text = upgradeLevels[ID]/*+ generated*/.ToString("F0");
            upgrades[ID].CostText.text = $"Cost: {UpgradeCost(type, ID).Notate()} Flasks";
            upgrades[ID].NameText.text = upgradeNames[ID];
            upgrades[ID].Fill.fillAmount = Methods.Fill(Controller.Instance.data.flasks, UpgradeCost(type, ID));
        }
    }

    public BigDouble UpgradeCost(string type, int UpgradeID)
    {
        var data = Controller.Instance.data;

        switch (type)
        {
            case "click":
                return UpgradeCost_BigDouble(0, data.clickUpgradesLevel, UpgradeID);
            case "production":
                return UpgradeCost_BigDouble(1, data.productionUpgradesLevel, UpgradeID);
            case "generator":
                return UpgradeCost_BigDouble(2, data.generatorUpgradesLevel, UpgradeID);
        }
        return 0;
    }


    private BigDouble UpgradeCost_BigDouble(int index, List<BigDouble> levels, int UpgradeID)
    {
        return UpgradeHandlers[index].UpgradeBaseCost[UpgradeID]
            * BigDouble.Pow(UpgradeHandlers[index].UpgradeCostMulti[UpgradeID],
                power: levels[UpgradeID]);
    }

    /*
    private BigDouble UpgradeCost_Int(int index, List<int> levels, int UpgradeID)
    {
        return UpgradeHandlers[index].UpgradeBaseCost[UpgradeID]
            * BigDouble.Pow(UpgradeHandlers[index].UpgradeCostMulti[UpgradeID],
                power: (BigDouble)levels[UpgradeID]);
    }
    */

    public void BuyUpgrade(string type, int UpgradeID)
    {
        var data = Controller.Instance.data;

        switch (type)
        {
            case "click":
                Buy(data.clickUpgradesLevel, type, UpgradeID);
                break;
            case "production":
                Buy(data.productionUpgradesLevel, type, UpgradeID);
                break;
            case "generator":
                Buy(data.generatorUpgradesLevel, type, UpgradeID);
                break;
        }

    }



    private void Buy(List<int> upgradeLevels, string type, int UpgradeID)
    {
        var data = Controller.Instance.data;
        if (data.flasks >= UpgradeCost(type, UpgradeID))
        {
            data.flasks -= UpgradeCost(type, UpgradeID);
            upgradeLevels[UpgradeID] += 1;
        }

        UpdateUpgradeUI(type, UpgradeID);
    }

    private void Buy(List<BigDouble> upgradeLevels, string type, int UpgradeID)
    {
        var data = Controller.Instance.data;
        if (data.flasks >= UpgradeCost(type, UpgradeID))
        {
            data.flasks -= UpgradeCost(type, UpgradeID);
            upgradeLevels[UpgradeID] += 1;
        }

        UpdateUpgradeUI(type, UpgradeID);
    }
}