using UnityEngine;

[CreateAssetMenu(fileName = "New Casing Data",menuName = "Cretate Casing Data")]
public class CasingData : ScriptableObject
{
    [Header("Force  Axis X")]
    [SerializeField] private float _minForceX;
    [SerializeField] private float _maxForceX;

    [Header("Force axis Y")]
    [SerializeField] private float _minForceY;
    [SerializeField] private float _maxForceY;

    [Header("Force axis Z")]
    [SerializeField] private float _minForceZ;
    [SerializeField] private float _maxForceZ;

    [Header("Rotation Force")]
    [SerializeField] private float _minRotationForce;
    [SerializeField] private float _maxRotationForce;

    [Header("Spin Speed")]
    [SerializeField] private float _spinSpeed;

    [Header("Destroy After Time")]
    [SerializeField] private float _destroyAfter;

    //Properties
    public float MinForceX => _minForceX;
    public float MaxForceX => _maxForceX;
    public float MinForceY => _minForceY;
    public float MaxForceY => _maxForceY; 
    public float MinForceZ => _minForceZ;
    public float MaxForceZ => _maxForceZ;
    public float MinRotationForce => _minRotationForce;
    public float MaxRotationForce => _maxRotationForce;
    public float SpinSpeed => _spinSpeed;
    public float DestroyAfter => _destroyAfter;
    
}
