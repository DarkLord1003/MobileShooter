using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    private static Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    public static void CreatePool(string namePool,int size,GameObject prefab)
    {
        if (string.IsNullOrEmpty(namePool) || size <= 0 || prefab == null)
            return;

        if (_pools.ContainsKey(namePool))
            return;

        Pool newPool = new Pool(namePool, size, new GameObject("Pool - " + namePool).transform);

        for(int i = 0; i < size; i++)
        {
            newPool.Objects.Add(AddObjectInPool(prefab, namePool, i, newPool.Parent));
        }

        _pools.Add(namePool,newPool);

    }

    public static GameObject GetObject(string namePool,Vector3 position,Quaternion rotation)
    {
        if (!_pools.ContainsKey(namePool))
            return null;

        foreach(GameObject obj in _pools[namePool].Objects)
        {
            if (!obj.activeSelf)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
        }

        return null;
    }

    private static GameObject AddObjectInPool(GameObject prefab,string name,int index,Transform parent)
    {
        GameObject obj = GameObject.Instantiate(prefab);
        obj.name = name + " - " + index;
        obj.transform.parent = parent;
        obj.SetActive(false);

        return obj;
    }
}
