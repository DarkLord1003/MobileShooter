using UnityEngine;

public abstract class Firearms : Weapon
{
    [Header("Bullet Spawn Point")]
    [SerializeField] protected Transform BulletSpawnPoint;

    [Header("Casing Spawn Point")]
    [SerializeField] protected Transform CasingSpawnPoint;

    [Header("Bullet Prefab")]
    [SerializeField] protected GameObject BulletPrefab;

    [Header("Muzzle Flash")]
    [SerializeField] protected ParticleSystem MuzzleFlash;

    //Shoot settings
    protected ShootType ShootingType;
    protected int CurrentClipSizeCount;
    protected float LastShootTime;

    //WeaponData
    protected FirearmsData FirearmsData;

    //Aim settings
    protected float DefaultFov;

    //States
    protected bool IsShooting;
    protected bool IsAiming;
    protected bool IsReloading;
    protected bool IsOutOfAmmo;
    protected bool CanShoot;

    //Methods
    protected abstract void Shoot();
    protected abstract void Aim();
    protected abstract void Reload();
    protected virtual void CheckAmmoInClip()
    {
        if(CurrentClipSizeCount <= 0)
        {
            IsOutOfAmmo = true;
            CanShoot = false;
        }
    }
    protected virtual void Init()
    {
        CanShoot = true;
        CurrentClipSizeCount = FirearmsData.ClipSize;
        ShootingType = ShootType.Automatic; 
    }
    protected virtual void InitBulletPool()
    {
        PoolManager.CreatePool(FirearmsData.NameGun + "_bullets", FirearmsData.ClipSize, BulletPrefab);
    }

    protected enum ShootType
    {
        Single,
        Automatic
    }
}
