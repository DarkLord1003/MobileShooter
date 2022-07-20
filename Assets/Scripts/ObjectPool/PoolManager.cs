using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    /// <summary>
    /// Словарь,который содежит все пулы объектов которые созданы в игре
    /// </summary>
    private static Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    /// <summary>
    /// Создание пула объектов
    /// </summary>
    /// <param name="namePool">Название пула</param>
    /// <param name="size">Размер пула</param>
    /// <param name="prefab">Объект</param>
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

    /// <summary>
    /// Получение объекта из пула
    /// </summary>
    /// <param name="namePool">Название пула</param>
    /// <param name="position">Позиция, которая будет присвоена объекту</param>
    /// <param name="rotation">Поворот объъекта</param>
    /// <returns></returns>
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

        if (_pools[namePool].Resize)
        {
            _pools[namePool].Objects.Add(AddObjectInPool(_pools[namePool].Objects[0],
                                         namePool, _pools[namePool].Objects.Count,
                                         _pools[namePool].Parent));

            return _pools[namePool].Objects[_pools[namePool].Objects.Count - 1];
        }

        return null;
    }

    /// <summary>
    /// Добавление объекта в пул
    /// </summary>
    /// <param name="prefab">Объект</param>
    /// <param name="name">Имя объекта</param>
    /// <param name="index">Индекс объекта</param>
    /// <param name="parent">Родитель объекта</param>
    /// <returns></returns>
    private static GameObject AddObjectInPool(GameObject prefab,string name,int index,Transform parent)
    {
        GameObject obj = GameObject.Instantiate(prefab);
        obj.name = name + " - " + index;
        obj.transform.parent = parent;
        obj.SetActive(false);

        return obj;
    }
}
