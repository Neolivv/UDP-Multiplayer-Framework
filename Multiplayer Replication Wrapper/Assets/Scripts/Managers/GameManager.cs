using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject playerprefab;

    Dictionary<string, GameObject> SpawnedPlayers = new Dictionary<string, GameObject>(); 

    float height = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }
    public bool PlayerExists(string playerID)
    {
        return SpawnedPlayers.ContainsKey(playerID);
    }

    public void SpawnPlayer(string playerID, Vector3 position = default)
    {
        if (SpawnedPlayers.ContainsKey(playerID)) return;

        GameObject go = Instantiate(playerprefab, new Vector3(0,height,0), Quaternion.identity);
        height += 2;
        SpawnedPlayers[playerID] = go;
    }

    public void RemovePlayer(string playerID)
    {
        if (!SpawnedPlayers.ContainsKey(playerID)) return;

        Destroy(SpawnedPlayers[playerID]);
        SpawnedPlayers.Remove(playerID);
    }
}
