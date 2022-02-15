using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Upgrades : MonoBehaviour
{
    public int UpgradeID;
    public Image UpgradeButton;
    public TMP_Text LevelText;
    public TMP_Text NameText;
    public TMP_Text CostText;

    public Image Fill;
    public Image FillSmooth;

    public double SmoothValue;

    public void BuyClickUpgrade() => UpgradesManager.Instance.BuyUpgrade("click", UpgradeID);
    public void BuyProductionUpgrade() => UpgradesManager.Instance.BuyUpgrade("production", UpgradeID);
    public void BuyGeneratorUpgrade() => UpgradesManager.Instance.BuyUpgrade("generator", UpgradeID);
}
