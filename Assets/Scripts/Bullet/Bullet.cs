using System.Collections;
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

    #region - OnCollisionEnter -
    private void OnCollisionEnter(Collision other)
    {
        if (_bulletData.DestroyOnImpact)
        {
            gameObject.SetActive(false);
            return;
        }

        StartCoroutine(DestroyTimer());
    }

    #endregion

    #region - GetReferences -
    private void GetRefernces()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    #endregion

    #region - Destroy Timers -
    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(Random.Range(_bulletData.MinDestroyTime, _bulletData.MaxDestroyTime));

        gameObject.SetActive(false);
    }

    private IEnumerator DestroyAfter(float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
    }

    #endregion

    #region - OnEnable -

    private void OnEnable()
    {
        StartCoroutine(DestroyAfter(8f));
    }

    #endregion

}
