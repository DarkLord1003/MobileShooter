using System.Collections;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [Header("Casing Data")]
    [SerializeField] private CasingData _casingData;

    [Header("Shells Sounds")]
    [SerializeField] private AudioCollection _shellsSounds;

    private bool _isCollisionDetected;


    private void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(
            Random.Range(_casingData.MinForceX, _casingData.MaxForceX),
            Random.Range(_casingData.MinForceY, _casingData.MaxForceY),
            Random.Range(_casingData.MinForceZ, _casingData.MaxForceZ));

        GetComponent<Rigidbody>().AddRelativeTorque(
            Random.Range(_casingData.MinRotationForce, _casingData.MaxRotationForce),
            Random.Range(_casingData.MinRotationForce, _casingData.MaxRotationForce),
            Random.Range(_casingData.MinRotationForce, _casingData.MaxRotationForce));
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.right * _casingData.SpinSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isCollisionDetected)
        {
            AudioManager.Instance.PlayOneShotSound("Casing", _shellsSounds[0], transform.position,
                         _shellsSounds.Volume, _shellsSounds.SpatialBlend, _shellsSounds.Priority);

            StartCoroutine(DestroyAfter());

            _isCollisionDetected = true;
        }

    }

    private IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(_casingData.DestroyAfter);

        _isCollisionDetected = false;
        gameObject.SetActive(false);
    }
}
