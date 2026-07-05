using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectTable<T> : ScriptableObject where T : UnityEngine.Object
{
    [SerializeField]
    protected List<T> spawnItems;

    [SerializeField]
    protected List<float> weights;

    protected virtual void OnValidate()
    {
        if (weights.Count < spawnItems.Count)
        {
            weights.Add(0);
        }
        for (int i = weights.Count - 1; i >= 0 && weights.Count > spawnItems.Count; i--)
        {
            weights.RemoveAt(i);
        }
    }

    protected virtual List<T> GetTable()
    {
        return spawnItems;
    }

    protected virtual List<float> GetWeights()
    {
        return weights;
    }

    public virtual T SpawnObject(Vector3 position)
    {
        var spawnItem = RandomizeFromTable();
        //possibility to drop nothing
        if (spawnItem != null)
        {
            return Object.Instantiate(spawnItem, position, Quaternion.identity);
        }

        return null;
    }

    public virtual T RandomizeFromTable()
    {
        int randomIndex = Util.WeightedRandomIndex(GetWeights());
        if (randomIndex == -1)
            return null;

        List<T> Table = GetTable();

        T spawnItem = Table[randomIndex];

        return spawnItem;
    }
}
