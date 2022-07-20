using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    /// <summary>
    /// �������,������� ������� ��� ���� �������� ������� ������� � ����
    /// </summary>
    private static Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    /// <summary>
    /// �������� ���� ��������
    /// </summary>
    /// <param name="namePool">�������� ����</param>
    /// <param name="size">������ ����</param>
    /// <param name="prefab">������</param>
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
    /// ��������� ������� �� ����
    /// </summary>
    /// <param name="namePool">�������� ����</param>
    /// <param name="position">�������, ������� ����� ��������� �������</param>
    /// <param name="rotation">������� ��������</param>
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
    /// ���������� ������� � ���
    /// </summary>
    /// <param name="prefab">������</param>
    /// <param name="name">��� �������</param>
    /// <param name="index">������ �������</param>
    /// <param name="parent">�������� �������</param>
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
