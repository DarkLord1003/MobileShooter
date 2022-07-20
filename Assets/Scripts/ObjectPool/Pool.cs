using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private string _name;
    private int _size;
    private bool _resize;
    private List<GameObject> _objects;
    private Transform _parent;

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public int Size
    {
        get => _size;
        set => _size = value;
    }

    public bool Resize
    {
        get => _resize;
        set => _resize = value;
    }

    public List<GameObject> Objects
    {
        get => _objects;
        set => _objects = value;
    }

    public Transform Parent
    {
        get => _parent;
        set => _parent = value;
    }

    public Pool(string name, int size, Transform parent, bool resize = false)
    {
        _name = name;
        _size = size;

        _objects = new List<GameObject>(size);
        _parent = parent;

        _resize = resize;
    }
}
