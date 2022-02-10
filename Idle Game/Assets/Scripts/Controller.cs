using System;
using BreakInfinity;
using UnityEngine;
using TMPro;

public class Controller : MonoBehaviour
{
    public Data data;


    public TMP_Text flasksTexts;


    public void Start()
    {
        data = new Data();
    }


    public void Update()
    {
        flasksTexts.text = data.flasks + " Flasks\n";
    }


    public void GenerateFlasks()
    {
        data.flasks += 1;

    }


}
