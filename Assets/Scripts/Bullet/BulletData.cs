using UnityEngine;

[CreateAssetMenu(menuName ="Create Bullet")]
public class BulletData : ScriptableObject
{
    [Header("Speed")]
    [SerializeField] private float _speed;

    [Header("Destroy On Impact")]
    [SerializeField] private bool _destroyOnImpact;

    [Header("Destroy Random Time")]
    [SerializeField] private float _minDestroyTime;
    [SerializeField] private float _maxDestroyTime;


    //Properties
    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public bool DestroyOnImpact
    {
        get => _destroyOnImpact;
        set => _destroyOnImpact = value;
    }

    public float MinDestroyTime
    {
        get => _minDestroyTime;
        set => _minDestroyTime = value;
    }

    public float MaxDestroyTime
    {
        get => _maxDestroyTime;
        set => _maxDestroyTime = value;
    }
}
