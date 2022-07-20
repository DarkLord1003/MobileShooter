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
    public float Speed => _speed;
    public bool DestroyOnImpact => _destroyOnImpact;
    public float MinDestroyTime => _minDestroyTime;
    public float MaxDestroyTime => _maxDestroyTime;

}
