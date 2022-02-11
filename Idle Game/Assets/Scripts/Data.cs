using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BreakInfinity;


public class Data 
{
    public BigDouble flasks;

    public List<int> clickUpgradesLevel;
    public List<int> productionUpgradesLevel;


    public Data()
    {
        flasks = 0;

        clickUpgradesLevel = new int[4].ToList();
        productionUpgradesLevel = new int[4].ToList();
    }

}
