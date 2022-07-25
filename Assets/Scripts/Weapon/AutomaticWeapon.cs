using System.Collections;
using UnityEngine;


public class AutomaticWeapon : Firearms
{
    //Timer
    private float _aimingTimer;

    //Aim Coroutine
    private Coroutine _aimCoroutine;

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
        Aim();
        Shoot();
        Reload();
    }

    #region - Shoot -
    protected override void Shoot()
    {
        if (InputManager.ShootTrigger)
        {
            if (CanShoot && !IsOutOfAmmo && !IsReloading)
            {
                if (Time.time - LastShootTime > 1f / FirearmsData.FireRate)
                {
                    LastShootTime = Time.time;
                    CurrentClipSize--;

                    WeaponAnimator.Play("Ak47_Shoot", 1, 0f);

                    WeaponAudioSource.clip = ShootSound;
                    WeaponAudioSource.Play();

                    GameObject obj = PoolManager.GetObject(FirearmsData.NameGun + "_bullets",
                                                           BulletSpawnPoint.transform.position,
                                                           BulletSpawnPoint.transform.rotation);

                    Bullet bullet = obj.GetComponent<Bullet>();

                    bullet.Rigidbody.velocity = bullet.transform.forward * bullet.BulletData.Speed;

                }
            }
        }
    }

    #endregion

    #region - Aim -
    protected override void Aim()
    {
        if (InputManager.AimTrigger)
        {
            if (!IsAiming)
            {
                if(_aimCoroutine!= null)
                {
                    StopCoroutine(_aimCoroutine);
                }

                _aimCoroutine = StartCoroutine(Aiming(EndAimPosition, FirearmsData.AimFov));
                IsAiming = true;
            }
            else
            {
                if (_aimCoroutine != null)
                {
                    StopCoroutine(_aimCoroutine);
                }

                _aimCoroutine = StartCoroutine(Aiming(StartAimPosition, FirearmsData.DefaultFov));
                IsAiming = false;
            }
        }
        
    }

    #endregion

    #region - Reload -
    protected override void Reload()
    {
        if (InputManager.ReloadTrigger && !IsReloading && CanReload)
        {
            if (IsOutOfAmmo)
            {
                IsReloading = true;
                WeaponAnimator.Play("Ak47_ReloadOutOfAmmo", 0, 0f);
            }
            else
            {
                WeaponAnimator.Play("Ak47_ReloadLeftOfAmmo", 0, 0f);
            }

        }
        
    }

    #endregion

    #region AimingCoroutine -

    private IEnumerator Aiming(Vector3 aimPosition,float fov)
    {
        while (_aimingTimer < AimTime)
        {
            WeaponHolder.transform.localPosition = Vector3.Lerp(WeaponHolder.transform.localPosition,
                                                   aimPosition, FirearmsData.TimeEntryToScope * Time.deltaTime);

            WeaponCamera.fieldOfView = Mathf.Lerp(WeaponCamera.fieldOfView, fov, 
                                                 FirearmsData.TimeEntryToScope * Time.deltaTime);

            _aimingTimer += Time.deltaTime;

            yield return null;

        }

        _aimingTimer = 0f;
    }

    #endregion;

}
