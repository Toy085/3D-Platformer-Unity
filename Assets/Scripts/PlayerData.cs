using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int coins = 0;
    public float maxHealth = 100f;
    public Vector3 playerPosition = Vector3.zero;
    public int levelsCompleted = 0;
    public int checkpointSceneIndex = -1;
    public List<int> cosmetics = new List<int>();
    public int equippedCosmetic = -1;
    public List<int> abilities = new List<int>();
    public int equippedAbility = -1;

    [System.Serializable]
    public struct LevelTime
    {
        public string levelName;
        public float bestTime;
    }
    public List<LevelTime> levelRecords = new List<LevelTime>();

    public float GetBestTime(string levelName)
    {
        foreach (var record in levelRecords)
        {
            if (record.levelName == levelName) return record.bestTime;
        }
        return 0f;
    }

    public void UpdateBestTime(string levelName, float newTime)
    {
        for (int i = 0; i < levelRecords.Count; i++)
        {
            if (levelRecords[i].levelName == levelName)
            {
                if (levelRecords[i].bestTime <= 0 || newTime < levelRecords[i].bestTime)
                {
                    var updatedRecord = levelRecords[i];
                    updatedRecord.bestTime = newTime;
                    levelRecords[i] = updatedRecord;
                }
                return;
            }
        }
        levelRecords.Add(new LevelTime { levelName = levelName, bestTime = newTime });
    }
}
