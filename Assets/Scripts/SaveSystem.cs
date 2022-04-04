using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SaveSystem : MonoBehaviour
{
    public TMP_InputField ImportField;
    public TMP_InputField ExportField;
    public WebGLNativeInputField ImportFieldWebGL;
    public WebGLNativeInputField ExportFieldWebGL;

    public Image CopyButton;
    public Image PasteButton;
    public Image CopyButtonWebGL;
    public Image PasteButtonWebGL;

    public TMP_Text CopyButtonText;
    public TMP_Text PasteButtonText;
    public TMP_Text CopyButtonTextWebGL;
    public TMP_Text PasteButtonTextWebGL;

    public GameObject SaveSystemObject;
    public GameObject SaveSystemObjectWebGL;
        
    private const string FileType = ".txt";
    private const string FileName = "PlayerData";
    private static string SavePath => Application.persistentDataPath + "/Saves/";
    private static string BackUpSavePath => Application.persistentDataPath + "/BackUps/";

    private static int SaveCount;

   
    private void Start()
    {
        #if Unity_WEBGL
            SaveSystemObject.SetActive(false);
            SaveSystemObjectWebGL.SetActive(true);
        #else
            SaveSystemObject.SetActive(true);
            SaveSystemObjectWebGL.SetActive(false);
        #endif
    }

    public static void SaveData<T>(T data, string fileName)
    {
        Directory.CreateDirectory(SavePath);
        Directory.CreateDirectory(BackUpSavePath);

        if (SaveCount % 5 == 0) Save(BackUpSavePath);
        Save(SavePath);

        SaveCount++;

        void Save(string path)
        {
            using (StreamWriter writer = new StreamWriter(path + fileName + FileType))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream memoryStream = new MemoryStream();
                formatter.Serialize(memoryStream, data);
                string dataToSave = Convert.ToBase64String(memoryStream.ToArray());
                writer.WriteLine(dataToSave);
                writer.Close();
            }
        }
    }

    public static T LoadData<T>(string fileName)
    {
        Directory.CreateDirectory(SavePath);
        Directory.CreateDirectory(BackUpSavePath);

        bool backUpNeeded = false;
        T dataToReturn;

        Load(SavePath);
        if (backUpNeeded) Load(BackUpSavePath);

        return dataToReturn;

        void Load(string path)
        {
            using (StreamReader reader = new StreamReader(path + fileName + FileType))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                string dataToLoad = reader.ReadToEnd();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(dataToLoad));

                try { dataToReturn = (T)formatter.Deserialize(memoryStream); }
                catch
                {
                    backUpNeeded = true;
                    dataToReturn = default;
                }
            }
        }
    }

    public static bool SaveExists(string fileName) =>
        File.Exists(SavePath + fileName + FileType)
        || File.Exists(BackUpSavePath + fileName + FileType);

    // If File.Exists gives an error, try System.IO.File


    public void Import()
    {
        Directory.CreateDirectory(SavePath);

        using (StreamWriter writer = new StreamWriter($"{SavePath}{FileName}{FileType}"))
        {
            #if Unity_WEBGL
            writer.WriteLine(ImportFieldWebGL.text);
            #else
            writer.WriteLine(ImportField.text);
            #endif
            writer.Close();
        }

        Controller.Instance.data = SaveSystem.SaveExists(FileName)
            ? SaveSystem.LoadData<Data>(FileName)
            : new Data();
    }

    public void Export()
    {
        Controller.Instance.Save();
        Directory.CreateDirectory(SavePath);

        using (StreamReader reader = new StreamReader($"{SavePath}{FileName}{FileType}"))
        {
            #if Unity_WEBGL
            ExportFieldWebGL.text = reader.ReadToEnd();
            #else
            ExportField.text = reader.ReadToEnd();
            #endif
            reader.Close();            
        }
    }

    public void Copy()
    {
        if (ExportField.text == "") return;
        #if Unity_WebGL
        GUIUtility.systemCopyBuffer = ExportFieldWebGL.text;
        CopyButtonWebGL.color = Color.green;
        CopyButtonTextWebGL.text = "Copied!";
        #else
        GUIUtility.systemCopyBuffer = ExportField.text;
        CopyButton.color = Color.green;
        CopyButtonText.text = "Copied!";
        #endif
        StartCoroutine(CopyPasteButtonsNormal());
    }

    public void Paste()
    {

#if Unity_WebGL
        ImportFieldWebGL.text = GUIUtility.systemCopyBuffer;
        PasteButtonWebGL.color = Color.green;
        PasteButtonTextWebGL.text = "Pasted!";
#else
        ImportField.text = GUIUtility.systemCopyBuffer;
        PasteButton.color = Color.green;
        PasteButtonText.text = "Pasted!";
        #endif
        StartCoroutine(CopyPasteButtonsNormal());
    }

    public void Clear(string type)
    {
        if (type == "Export")
        {
            ExportField.text = "";
            ExportFieldWebGL.text = "";
            return;
        }
        ImportField.text = "";
    }

    public IEnumerator CopyPasteButtonsNormal()
    {
        yield return new WaitForSeconds(2f);
        CopyButton.color = new Color(0.216f, 0.216f, 0.216f);
        CopyButtonText.text = "Copy to Clipboard";
        PasteButton.color = new Color(0.216f, 0.216f, 0.216f);
        PasteButtonText.text = "Paste Clipboard";

        CopyButtonWebGL.color = new Color(0.216f, 0.216f, 0.216f);
        CopyButtonTextWebGL.text = "Copy to Clipboard";
        PasteButtonWebGL.color = new Color(0.216f, 0.216f, 0.216f);
        PasteButtonTextWebGL.text = "Paste Clipboard";
    }

    
}
