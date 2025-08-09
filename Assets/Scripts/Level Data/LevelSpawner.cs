using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LevelSpawner
{
    public static LevelData LevelData = new LevelData();

    public static void SaveLevelData(string key)
    {
        string folderPath = Application.persistentDataPath + "/Levels/";
        string jsonPath = folderPath + key + ".json";
        Directory.CreateDirectory(folderPath);

        string jsonString = JsonUtility.ToJson(LevelData);
        File.WriteAllText(jsonPath, jsonString);
    }

    public static void LoadLevelData(string key)
    {
        string folderPath = Application.persistentDataPath + "/Levels/";
        string jsonPath = folderPath + key + ".json";
        Directory.CreateDirectory(folderPath);
        
        string jsonString = File.ReadAllText(jsonPath);
        LevelData = JsonUtility.FromJson<LevelData>(jsonString);
        //LevelData = (LevelData)JsonUtility.FromJson(jsonPath,typeof(LevelData));
    }

    public static void LoadLevelData(TextAsset textAsset)
    {
        string jsonString = textAsset.text;
        LevelData = JsonUtility.FromJson<LevelData>(jsonString);
    }


}