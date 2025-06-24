using System.Collections.Generic;
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
                Instantiate(item.prefab, item.spawnLocation.position, item.spawnLocation.rotation);
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
