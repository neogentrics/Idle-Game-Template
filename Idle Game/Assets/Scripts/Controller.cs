using System;
using BreakInfinity;
using UnityEngine;
using TMPro;

public class Controller : MonoBehaviour
{
    // public UpgradesManager upgradesManager;
    public static Controller Instance;

    private void Awake() => Instance = this;

    public Data data;

    [SerializeField] private TMP_Text flasksTexts;
    [SerializeField] private TMP_Text flasksPerSecond;
    [SerializeField] private TMP_Text flaskClickPowerText;

    public BigDouble ClickPower()
    {
        BigDouble total = 1;
        for (int i = 0; i < data.clickUpgradesLevel.Count; i++)
        {
            total += UpgradesManager.Instance.UpgradeHandlers[0].UpgradesBasePower[i] * data.clickUpgradesLevel[i];
  
        }
        return total;
    }

    public BigDouble FlasksPerSecond()
    {
        BigDouble total = 0;
        for (int i = 0; i < data.productionUpgradesLevel.Count; i++)
        {
            total += UpgradesManager.Instance.UpgradeHandlers[1].UpgradesBasePower[i] 
                * data.productionUpgradesLevel[i] + data.productionUpgradeGenerated[i];
        }
        return total;
    }

    public BigDouble UpgradesPerSecond(int index)
    {
        return UpgradesManager.Instance.UpgradeHandlers[2].UpgradesBasePower[index] * data.generatorUpgradesLevel[index];
    }

    public void Start()
    {
        data = new Data();

        UpgradesManager.Instance.StartUpgradeManager();
    }


    public void Update()
    {
        flasksTexts.text = $"{data.flasks:F2} Flasks";
        flasksPerSecond.text = $"{FlasksPerSecond():F2}/s";
        flaskClickPowerText.text = "+" + ClickPower() + " Flasks";

        data.flasks += FlasksPerSecond() * Time.deltaTime;

        for (var i = 0; i < data.productionUpgradesLevel.Count; i++)
            data.productionUpgradeGenerated[i] += UpgradesPerSecond(i) * Time.deltaTime;
    }


    public void GenerateFlasks()
    {
        data.flasks += ClickPower();

    }


}
