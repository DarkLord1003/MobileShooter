using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Data")]
    [SerializeField] private BulletData _bulletData;

    [Header("Rigidbody")]
    private Rigidbody _rigidbody;

    //Properties
    public BulletData BulletData => _bulletData;
    public Rigidbody Rigidbody => _rigidbody;

    private void Awake()
    {
        GetRefernces();
    }


    private void OnCollisionEnter(Collision other)
    {
        if (_bulletData.DestroyOnImpact)
        {
            gameObject.SetActive(false);
            return;
        }

        Debug.Log(other.gameObject.name);
        StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(Random.Range(_bulletData.MinDestroyTime, _bulletData.MaxDestroyTime));

        gameObject.SetActive(false);
    }

    private void GetRefernces()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
}
