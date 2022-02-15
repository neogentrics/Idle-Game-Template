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

    private const string dataFileName = "PlayerData";

    
    private void Start()
    {
        var fire = UpgradesManager.Instance;

        data = SaveSystem.SaveExists(dataFileName) 
            ? SaveSystem.LoadData<Data>(dataFileName)
            : new Data();
        fire.StartUpgradeManager();
        Settings.instance.StartSettings();
    }

    /*
    public void Start()
    {
        data = SaveSystem.SaveExists(dataFileName)
            ? SaveSystem.LoadData<Data>(dataFileName)
            : new Data();
        var accessTestA = Controller.Instance;
        var accessTestB = Controller.Instance.data;
        UpgradesManager.Instance.StartUpgradeManager();
        Settings.instance.StartSettings();
    }
    */

    private float SaveTime;

    public void Update()
    {
        flasksTexts.text = $"{data.flasks.Notate()} Flasks";
        flasksPerSecond.text = $"{FlasksPerSecond().Notate()}/s";
        flaskClickPowerText.text = "+" + ClickPower() + " Flasks";

        data.flasks += FlasksPerSecond() * Time.deltaTime;

        for (var i = 0; i < data.productionUpgradesLevel.Count; i++)
            data.productionUpgradeGenerated[i] += UpgradesPerSecond(i) * Time.deltaTime;

        SaveTime += Time.deltaTime * (1 / Time.timeScale);
        if (SaveTime >= 5)
        {
            Save();
            SaveTime = 0;
        }    
    }

    public void Save()
    {
        SaveSystem.SaveData(data, dataFileName);

    }


    public void GenerateFlasks()
    {
        data.flasks += ClickPower();

    }


}
