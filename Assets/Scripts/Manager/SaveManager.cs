using System.IO;
using TMPro;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private const string SAVE_FILE_PATH = "/save.txt";
    private SaveObject saveObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }

        saveObject = new SaveObject();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(Application.dataPath + SAVE_FILE_PATH, json);
    }

    public void LoadBestRound(TMP_Text bestRound, TMP_Text bestTime)
    {
        if (File.Exists(Application.dataPath + SAVE_FILE_PATH))
        {
            string saveString = File.ReadAllText(Application.dataPath + SAVE_FILE_PATH);

            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);
            bestRound.text = (saveObject.bestRound).ToString();
            bestTime.text = saveObject.bestTime;
        }
    }

    private class SaveObject
    {
        public int bestRound;
        public string bestTime;
    }
    public void SetBestRound(int bestRound, string bestTime)
    {
        saveObject.bestRound = bestRound;
        saveObject.bestTime = bestTime;
    }
}
