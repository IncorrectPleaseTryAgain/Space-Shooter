using System;
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

    public void SaveBestRound(int bestRound, string bestTime)
    {
        if (RoundIsBetter(bestRound, bestTime))
        {
            saveObject.bestRound = bestRound;
            saveObject.bestTime = bestTime;
            string json = JsonUtility.ToJson(saveObject);
            File.WriteAllText(Application.dataPath + SAVE_FILE_PATH, json);
        }
    }

    private bool RoundIsBetter(int bestRound, string thisTime)
    {
        if (File.Exists(Application.dataPath + SAVE_FILE_PATH))
        {
            string saveString = File.ReadAllText(Application.dataPath + SAVE_FILE_PATH);
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);


            // Check Round
            if(saveObject.bestRound <= bestRound) { return true; }
            else { return false; }

            // Check Time
            string[] time = thisTime.Split(':');
            string[] currentBestTime = saveObject.bestTime.Split(":");
            if(currentBestTime.Length <  time.Length) { return true; }
            else if(currentBestTime.Length > time.Length) { return false; }
            else
            {
                for (int i = 0; i < time.Length; i++)
                {
                    if (float.Parse(currentBestTime[i]) < float.Parse(time[i])) { return false; }
                    else if(float.Parse(currentBestTime[i]) > float.Parse(time[i])) { return true; }
                }
                return false;
            }
        }
        else { return true; }
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
}
