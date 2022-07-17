using UnityEngine;

public class PlayerLock : MonoBehaviour
{
    
    public Transform playerBody;
    public float sens = 100f;
    float xRot;
    public bool lockCursor;

    public FixedTouchField fixedTouchField;
    [HideInInspector]
	public Vector2 LockAxis;
 

    void Start()
    {
       if (lockCursor) Cursor.lockState = CursorLockMode.Locked;
    }

  
    void Update()
    {
        LockAxis = fixedTouchField.TouchDist;

        float mouX = LockAxis.x * sens * Time.deltaTime;
        float mouY = LockAxis.y * sens * Time.deltaTime;

        xRot -= mouY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouX);

    }
    
}
