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

                    AudioManager.Instance.PlayOneShotSound("Weapon", WeaponShoots[0], transform.position, 
                                                           WeaponShoots.Volume, WeaponShoots.SpatialBlend, 
                                                           WeaponShoots.Priority);

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
        if (InputManager.AimTrigger && !WeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName(FirearmsData.NameGun + "_Run")
            && CanAiming)
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
        else if(WeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName(FirearmsData.NameGun + "_Run"))
        {
            if (IsAiming)
            {
                StopCoroutine(_aimCoroutine);
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
                IsReloading = true;
                WeaponAnimator.Play("Ak47_ReloadLeftOfAmmo", 0, 0f);
            }

        }
        
    }

    #endregion

    #region - PlayReloadSounds -

    private void PlayPullingOutClipSound()
    {
        AudioManager.Instance.PlayOneShotSound("Weapon", WeaponReloading[0], transform.position,
                                               WeaponReloading.Volume, WeaponReloading.SpatialBlend,
                                               WeaponReloading.Priority);
    }

    private void PlayInsertingClipSound()
    {
        AudioManager.Instance.PlayOneShotSound("Weapon", WeaponReloading[1], transform.position,
                                               WeaponReloading.Volume, WeaponReloading.SpatialBlend,
                                               WeaponReloading.Priority);
    }

    private void PlayShutterDistortionSound()
    {
        AudioManager.Instance.PlayOneShotSound("Weapon", WeaponReloading[2], transform.position,
                                               WeaponReloading.Volume, WeaponReloading.SpatialBlend,
                                               WeaponReloading.Priority);
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

    #region - OnEnable/Disable -

    private void OnEnable()
    {
        EventManager.StartListening("AddAmmo", AddAmmoInClip);
        EventManager.StartListening("PlayPullingOutClipSound", PlayPullingOutClipSound);
        EventManager.StartListening("PlayInsertingClipSound", PlayInsertingClipSound);
        EventManager.StartListening("PlayShutterDistortionSound", PlayShutterDistortionSound);
    }

    private void OnDisable()
    {
        EventManager.StopListening("AddAmmo", AddAmmoInClip);
        EventManager.StopListening("PlayPullingOutClipSound", PlayPullingOutClipSound);
        EventManager.StopListening("PlayInsertingClipSound", PlayInsertingClipSound);
        EventManager.StopListening("PlayShutterDistortionSound", PlayShutterDistortionSound);
    }

    #endregion

}
