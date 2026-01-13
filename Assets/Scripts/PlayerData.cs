using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int coins = 0;
    public float maxHealth = 100f;
    public Vector3 playerPosition = Vector3.zero;
    public int levelsCompleted = 0;
    public int checkpointSceneIndex = -1;
}
