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
    /// Название пула
    /// </summary>
    public string Name
    {
        get => _name;
        set => _name = value;
    }

    /// <summary>
    /// Размер пула
    /// </summary>
    public int Size
    {
        get => _size;
        set => _size = value;
    }

    /// <summary>
    /// Изминение размера пула
    /// </summary>
    public bool Resize
    {
        get => _resize;
        set => _resize = value;
    }

    /// <summary>
    /// Список объектов
    /// </summary>
    public List<GameObject> Objects
    {
        get => _objects;
        set => _objects = value;
    }

    /// <summary>
    /// Родительский объект
    /// </summary>
    public Transform Parent
    {
        get => _parent;
        set => _parent = value;
    }


    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="name">Имя пула</param>
    /// <param name="size">Размер пула</param>
    /// <param name="parent">Родитель пула</param>
    /// <param name="resize">Изминение размера</param>
    public Pool(string name, int size, Transform parent, bool resize = false)
    {
        _name = name;
        _size = size;

        _objects = new List<GameObject>(size);
        _parent = parent;

        _resize = resize;
    }
}
