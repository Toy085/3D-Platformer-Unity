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
}
