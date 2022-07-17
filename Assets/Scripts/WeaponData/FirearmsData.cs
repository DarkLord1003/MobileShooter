using UnityEngine;

[CreateAssetMenu(menuName ="Create new firearms weapon")]
public class FirearmsData : WeaponData
{
    [Header("Clip Settings")]
    [SerializeField] private int _clipSize;
    [SerializeField] private int _clipCount;

    [Header("Fire Rate")]
    [SerializeField] private float _fireRate;

    [Header("Recoil Settings")]
    [SerializeField] private float _recoilForce;

    [Header("Aiming Settings")]
    [SerializeField] private float _aimFov;
    [SerializeField] private float _timeEntryToScope;

    //Properties
    public string NameGun => WeaponName;
    public int ClipSize => _clipSize;
    public int ClipCount => _clipCount;
    public float FireRate => _fireRate;
    public float RecoilForce => _recoilForce;
    public float AinFov => _aimFov;
    public float TimeEntryToScope => _timeEntryToScope;
}
