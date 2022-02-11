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

    public void BuyClickUpgrade() => UpgradesManager.Instance.BuyUpgrade("click", UpgradeID);
    public void BuyProductionUpgrade() => UpgradesManager.Instance.BuyUpgrade("production", UpgradeID);


}
