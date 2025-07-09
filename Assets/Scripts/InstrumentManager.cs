using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

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
                GameObject obj = Instantiate(item.prefab, item.spawnLocation.position, item.spawnLocation.rotation);

                NetworkObject netObj = obj.GetComponent<NetworkObject>();
                if (netObj != null)
                {
                    netObj.Spawn(true);
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

    public void SpawnKeyboard()
    {
        SpawnByName("keyboard");
    }
}
