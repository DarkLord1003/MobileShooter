using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Data")] 
    [SerializeField] protected WeaponData WeaponData;

    [Header("Input Manager")]
    [SerializeField] protected InputManager InputManager;

    [Header("Weapon Holder Transform")]
    [SerializeField] protected Transform WeaponHolder;

    [Header("Camera")]
    [SerializeField] protected Camera WeaponCamera;

    [Header("Animator")]
    [SerializeField] protected Animator WeaponAnimator; 

}
