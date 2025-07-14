using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InstrumentManager : NetworkBehaviour
{
    [System.Serializable]
    public class NamedPrefab
    {
        public string name;
        public GameObject prefab;
        public Transform spawnLocation;
    }

    public List<NamedPrefab> prefabs;
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            gameObject.SetActive(false); // Or Destroy(gameObject); if you prefer
        }
    }

    public void SpawnByName(string prefabName)
    {
        if (!IsServer) return; // Just in case

        // handle extras
        if (prefabName == "maracas")
        {
            SpawnByName("extra_maraca");
        }
        else if (prefabName == "xylophone")
        {
            SpawnByName("extra_xylostick_1");
            SpawnByName("extra_xylostick_2");
        }
        else if (prefabName == "drums")
        {
            SpawnByName("extra_drumstick_1");
            SpawnByName("extra_drumstick_2");
        }

        foreach (var item in prefabs)
        {
            if (item.name == prefabName && item.prefab != null)
            {
                // If already spawned, destroy the old one
                if (spawnedObjects.TryGetValue(prefabName, out GameObject existing))
                {
                    if (existing != null)
                    {
                        NetworkObject existingNetObj = existing.GetComponent<NetworkObject>();
                        if (existingNetObj != null && existingNetObj.IsSpawned)
                        {
                            existingNetObj.Despawn(true); // Clean up networked object
                        }
                        else
                        {
                            Destroy(existing);
                        }
                    }

                    spawnedObjects.Remove(prefabName);
                }

                GameObject obj = Instantiate(item.prefab, item.spawnLocation.position, item.spawnLocation.rotation);

                NetworkObject netObj = obj.GetComponent<NetworkObject>();
                if (netObj != null)
                {
                    netObj.Spawn(true);
                    spawnedObjects[prefabName] = obj;
                }
                else
                {
                    Debug.LogError($"Prefab '{prefabName}' is missing a NetworkObject component.");
                }

                return;
            }
        }

        Debug.LogWarning($"Prefab with name '{prefabName}' not found.");
    }
}
