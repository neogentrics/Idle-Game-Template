using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeHandler : MonoBehaviour
{
    
    public List<Upgrades> Upgrades;
    public Upgrades UpgradesPrefab;
    public ScrollRect UpgradesScroll;
    public Transform UpgradesPanel;
    public string[] UpgradeNames;
    public BigDouble[] UpgradeBaseCost;
    public BigDouble[] UpgradeCostMulti;
    public BigDouble[] UpgradesBasePower;
    public BigDouble[] UpgradesUnlock;

}