using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BreakInfinity;

[Serializable]
public class Data 
{
    public BigDouble flasks;

    public List<BigDouble> clickUpgradesLevel;
    public List<BigDouble> productionUpgradesLevel;
    public List<BigDouble> productionUpgradeGenerated;
    public List<BigDouble> generatorUpgradesLevel;

    public int Notation;

    // public BigDouble achlevel;    

    public Data()
    {
        flasks = 0;

        clickUpgradesLevel = new BigDouble[7].ToList();
        productionUpgradesLevel = new BigDouble[7].ToList();
        productionUpgradeGenerated = new BigDouble[7].ToList();
        generatorUpgradesLevel = new BigDouble[7].ToList();

        Notation = 0;
    }

    
}
