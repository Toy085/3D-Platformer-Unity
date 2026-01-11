using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string GetSavePath(int slot)
    {
        return Application.persistentDataPath + $"/save_slot_{slot}.save";
    }

    public static void SavePlayer(PlayerData data, int slot)
    {
        string path = GetSavePath(slot);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log($"Game Saved to Slot {slot} at: {path}");
    }

    public static PlayerData LoadPlayer(int slot)
    {
        string path = GetSavePath(slot);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log($"Game Loaded from Slot {slot} at: {path}");
            return data;
        }
        else
        {
            Debug.LogWarning($"No save file found in Slot {slot}");
            return new PlayerData();
        }
    }

    public static void DeleteSave(int slot)
    {
        string path = GetSavePath(slot);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Save file in Slot {slot} deleted");
        }
    }

    public static bool SaveExists(int slot)
    {
        return File.Exists(GetSavePath(slot));
    }
}
