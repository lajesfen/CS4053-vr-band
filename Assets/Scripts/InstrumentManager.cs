using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using static Oculus.Interaction.Context;

public class InstrumentManager : MonoBehaviour
{
    [System.Serializable]
    public class NamedPrefab
    {
        public string name;
        public GameObject prefab;
        public Transform spawnLocation;
    }

    public List<NamedPrefab> prefabs;

    public void SpawnByName(string prefabName)
    {
        foreach (var item in prefabs)
        {
            if (item.name == prefabName && item.prefab != null)
            {
                GameObject instance = Instantiate(item.prefab, item.spawnLocation.position, item.spawnLocation.rotation);
             
                // Spawn it across the network
                NetworkObject netObj = instance.GetComponent<NetworkObject>();
                if (netObj != null)
                {
                    netObj.Spawn();  // Now it exists for all players
                }
                else
                {
                    Debug.LogError("Prefab is missing NetworkObject component.");
                }

                return;
            }
        }
        Debug.LogWarning($"Prefab with name '{prefabName}' not found.");
    }

    public void SpawnKeyboard()
    {
        SpawnByName("keyboard");
    }
}
