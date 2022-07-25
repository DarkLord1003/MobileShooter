using UnityEngine;

public abstract class Firearms : Weapon
{
    [Header("Bullet Spawn Point")]
    [SerializeField] protected Transform BulletSpawnPoint;

    [Header("Casing Spawn Point")]
    [SerializeField] protected Transform CasingSpawnPoint;

    [Header("Aim Position")]
    [SerializeField] protected Vector3 StartAimPosition;
    [SerializeField] protected Vector3 EndAimPosition;

    [Header("Aim Time")]
    [SerializeField] protected float AimTime;

    [Header("Bullet Prefab")]
    [SerializeField] protected GameObject BulletPrefab;

    [Header("Muzzle Flash")]
    [SerializeField] protected ParticleSystem MuzzleFlash;

    [Header("Audio Source")]
    [SerializeField] protected AudioSource WeaponAudioSource;

    [Header("Sounds")]
    [SerializeField] protected AudioClip ShootSound;

    //Shoot settings
    protected int CurrentClipSize;
    protected int CurrentAmmo;
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
    protected bool CanReload;

    //Methods abstract
    protected abstract void Shoot();
    protected abstract void Aim();
    protected abstract void Reload();

    //Virtual methods
    protected virtual void CheckAmmoInClip()
    {
        if(CurrentClipSize == 0)
        {
            IsOutOfAmmo = true;
            CanShoot = false;
            return;
        }
        else if(CurrentClipSize < FirearmsData.ClipSize)
        {
            CanReload = true;
            return;
        }

        CanReload = false;
    }
    protected virtual void AddAmmoInClip()
    {
        if (CurrentAmmo == 0)
            return;

        int currentAmmoOutClip = FirearmsData.ClipSize - CurrentClipSize;

        if(CurrentAmmo >= currentAmmoOutClip)
        {
            CurrentAmmo -= currentAmmoOutClip;
            CurrentClipSize += currentAmmoOutClip;
        }
        else
        {
            CurrentClipSize += CurrentAmmo;
            CurrentAmmo -= CurrentAmmo;
        }

        IsOutOfAmmo = false;
    }
    protected virtual void Init()
    {
        CanShoot = true;

        CurrentClipSize = FirearmsData.ClipSize;
        CurrentAmmo = FirearmsData.ClipCount * FirearmsData.ClipSize;
    }
    protected virtual void InitBulletPool()
    {
        PoolManager.CreatePool(FirearmsData.NameGun + "_bullets", FirearmsData.ClipSize, BulletPrefab);
    }

}
