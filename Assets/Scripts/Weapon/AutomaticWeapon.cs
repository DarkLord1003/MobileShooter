using UnityEngine;

public class AutomaticWeapon : Firearms
{

    private void Awake()
    {
        FirearmsData = (FirearmsData)WeaponData;
    }

    private void Start()
    {
        InitBulletPool();
        Init();
    }

    private void Update()
    {
        CheckAmmoInClip();
        Shoot();
    }


    protected override void Shoot()
    {
        if (InputManager.ShootPressTrigger)
        {
            if (CanShoot && !IsOutOfAmmo && !IsReloading)
            {
                if (Time.time - LastShootTime > 1f / FirearmsData.FireRate)
                {
                    LastShootTime = Time.time;
                    CurrentClipSizeCount--;

                    WeaponAnimator.Play("Shoot", 1, 0f);

                    GameObject obj = PoolManager.GetObject(FirearmsData.NameGun + "_bullets",
                                                           BulletSpawnPoint.transform.position,
                                                           BulletSpawnPoint.transform.rotation);

                    Bullet bullet = obj.GetComponent<Bullet>();

                    bullet.Rigidbody.velocity = bullet.transform.forward * -bullet.BulletData.Speed;

                }
            }
        }
    }

    protected override void Aim()
    {
        
    }

    protected override void Reload()
    {
        
    }


    #region - OnEnable/OnDisable

    private void OnEnable()
    {
        
    }

    #endregion
}
