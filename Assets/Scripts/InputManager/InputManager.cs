using UnityEngine;
using UnityEngine.InputSystem;
using System;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    private CharacterActions _characterActions;

    [Header("Settings")]
    [SerializeField] private bool _isMobilePlatform;

    //Events
    public event Action Jump;
    public event Action Crouch;
    public event Action Shoot;
    public event Action Aim;

    //Movement and view data
    private Vector2 _moveInput;
    private Vector2 _viewInput;

    //Private fields
    private bool _crouchTrigger;
    private bool _sprintTrigger;
    private bool _shootPressTrigger;
    private bool _shootReleaseTrigger;
    private bool _aimPressTrigger;
    private bool _aimReleaseTrigger;
    private bool _reloadTrigger;

    //Properties
    public Vector2 MoveInput => _moveInput;
    public Vector2 ViewInput => _viewInput;
    public bool IsMobilePlatform => _isMobilePlatform;
    public bool SprintTrigger => _sprintTrigger;
    public bool CrouchTrigger => _crouchTrigger;
    public bool ShootPressTrigger => _shootPressTrigger;
    public bool ShootReleaseTrigger => _shootReleaseTrigger;
    public bool AimPressTrigger => _aimPressTrigger;
    public bool AimReleaseTrigger => _aimReleaseTrigger;
    public bool ReloadTrigger => _reloadTrigger;


    private void Awake()
    {
        GetStartValues();
        SubscribingToEvents();
    }

    private void Update()
    {
        GetStatusMobileButtons();
    }

    private void SubscribingToEvents()
    {
        _characterActions.Player.Movement.performed += e => OnMove(e);
        _characterActions.Player.Movement.canceled += e => _moveInput = Vector2.zero;

        _characterActions.Player.View.performed += e => _viewInput = e.ReadValue<Vector2>();

        _characterActions.Player.Jump.performed += e => OnJump();
        _characterActions.Player.Sprint.started += e => _sprintTrigger = true;
        _characterActions.Player.Sprint.canceled += e => _sprintTrigger = false;

        _characterActions.Player.Crouch.performed += e => OnStartCrouch();

       
    }

    private void OnStartCrouch()
    {
        Crouch?.Invoke();
    }

    private void OnJump()
    {
        Jump?.Invoke();
    }

    private void OnMove(InputAction.CallbackContext callbackContext)
    {
        _moveInput = callbackContext.ReadValue<Vector2>();
    }

    private void OnShoot()
    {
        Shoot?.Invoke();
    }

    private void OnAim()
    {
        Aim?.Invoke();
    }


    #region - GetStatusMobileButtons -

    private void GetStatusMobileButtons()
    {
        if (_isMobilePlatform)
        {
            _viewInput = TouchField.Instance.InputAxis * 3;

            _moveInput = MobileControls.Instance.GetJoystick("LeftStick");

            if (MobileControls.Instance.GetButtonDown("Jump"))
            {
                Jump?.Invoke();
            }

            _shootPressTrigger = MobileControls.Instance.GetButton("Shoot");
            _aimPressTrigger = MobileControls.Instance.GetButtonDown("Aim");
            _reloadTrigger = MobileControls.Instance.GetButtonDown("Reload");

            _sprintTrigger = MobileControls.Instance.GetDistanceStick("LeftStick") > 95f;
        }
    }

    #endregion

    #region - GetStartValue

    private void GetStartValues()
    {
        _characterActions = new CharacterActions();
    }

    #endregion

    #region - OnEnable/OnDisable

    private void OnEnable()
    {
        _characterActions.Player.Enable();
    }

    private void OnDisable()
    {
        _characterActions.Player.Disable();
    }

    #endregion

}

