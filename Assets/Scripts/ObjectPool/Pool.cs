using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    //Settings
    private string _name;
    private int _size;
    private bool _resize;
    private List<GameObject> _objects;
    private Transform _parent;

    /// <summary>
    /// �������� ����
    /// </summary>
    public string Name
    {
        get => _name;
        set => _name = value;
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    public int Size
    {
        get => _size;
        set => _size = value;
    }

    /// <summary>
    /// ��������� ������� ����
    /// </summary>
    public bool Resize
    {
        get => _resize;
        set => _resize = value;
    }

    /// <summary>
    /// ������ ��������
    /// </summary>
    public List<GameObject> Objects
    {
        get => _objects;
        set => _objects = value;
    }

    /// <summary>
    /// ������������ ������
    /// </summary>
    public Transform Parent
    {
        get => _parent;
        set => _parent = value;
    }


    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="name">��� ����</param>
    /// <param name="size">������ ����</param>
    /// <param name="parent">�������� ����</param>
    /// <param name="resize">��������� �������</param>
    public Pool(string name, int size, Transform parent, bool resize = false)
    {
        _name = name;
        _size = size;

        _objects = new List<GameObject>(size);
        _parent = parent;

        _resize = resize;
    }
}
