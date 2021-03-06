using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ModelSettings;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Input Manager")]
    [SerializeField] private InputManager _inputManager;

    [Header("Camera")] 
    [SerializeField] private Transform _camera;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    [Header("Feet Transform")]
    [SerializeField] private Transform _feetTransform;

    [Header("Player Setiings")] 
    [SerializeField] private PlayerSettings _playerSettings;

    [Header("Gravity Settings")]
    [SerializeField] private GravitySettings _gravitySettings;

    [Header("Player Stance")]
    [SerializeField] private PlayerStance _stand;
    [SerializeField] private PlayerStance _crouch;
    [SerializeField] private float _animationCrouchStandDuration;
    private PlayerStance _currentStance;
    private PlayerBodyPosition _currentBodyPosition;
    private Vector3 _colliderCenterVelocity;
    private float _crouchingStandingAnimationTime;
    private float _colliderHeightVelocity;
    private float _currentCameraHeight;
    private float _cameraHeightVelocity;

    [Header("Head Breathing")]
    [SerializeField] private CurveControlledBod _headBob = new CurveControlledBod();


    [Space(5)]
    [Header("Sounds")]
    [SerializeField] private AudioCollection _footsteps;

    //Animations
    private bool _crouchingAnimation;

    //Player Stats 
    private bool _isWalking;
    private bool _isRunning;
    private bool _isJumping;
    private bool _isFalling;

    //Character Controller
    private CharacterController _characterController;

    //Look
    private Vector3 _newCharcterRotation;
    private Vector3 _newCameraRotation;
    private Vector3 _localSpaceCameraPos;

    //Move
    private Vector3 _axis;
    private Vector3 _newCharacterPosition;
    private Vector3 _smoothingCharacterPositon;
    private Vector3 _smoothingCharacterPositionVelocity;

    //Stamina
    private float _currentStamina;
    private float _currentStaminaDelay;

    //Gravity
    private Vector3 _gravityDirection;
    private Vector3 _gravityMovement;
    private float _currentGravity;

    //Falling
    private float _fallingSpeed;

    //Falling speed
    private Vector3 _relativePlayerVelocity;


    //Properties
    public CharacterController CharacterController => _characterController;
    public bool IsRunning => _isRunning;

    private void Awake()
    {
        GetStartValues();
    }

    private void Start()
    {
        SetStartValues();
    }

    private void Update()
    {
        View();
        Sprinting();
        CalculateSprinting();
        CalculateGravity();
        Movement();
        CalculateFalling();
        UpdateAnimation();
    }


    #region  - Move and View
    private void Movement()
    {
        _relativePlayerVelocity = transform.InverseTransformDirection(_characterController.velocity);

        _axis = transform.right * _inputManager.MoveInput.x + transform.forward * _inputManager.MoveInput.y;
        _axis = _axis.normalized;

        if (_isRunning)
        {
            _newCharacterPosition = _axis * _playerSettings.SprintSpeed * Time.deltaTime;
        }
        else
        {
            _newCharacterPosition = _axis * _playerSettings.MoveSpeed * Time.deltaTime;
            _isWalking = true;
        }

        _smoothingCharacterPositon = Vector3.SmoothDamp(_smoothingCharacterPositon, _newCharacterPosition,
            ref _smoothingCharacterPositionVelocity, _playerSettings.SmotthingMovementSpeed);

        _characterController.Move(_smoothingCharacterPositon + _gravityMovement);

        if (_characterController.velocity.magnitude > 0.01f)
        {
            _camera.transform.localPosition = _localSpaceCameraPos +
                                              _headBob.GetVectorOffset(_characterController.velocity.magnitude);
        }
        else
        {
            _camera.transform.localPosition = _localSpaceCameraPos;
        }
    }

    private void View()
    {
        _newCameraRotation.x += _playerSettings.SensetivityY * (_playerSettings.InverseY ? _inputManager.ViewInput.y :
                                                               -_inputManager.ViewInput.y) * Time.deltaTime;

        _newCameraRotation.x = Mathf.Clamp(_newCameraRotation.x, -_playerSettings.MinimumClampX, _playerSettings.MaximumClampX);

        _newCharcterRotation.y += _playerSettings.SensetivityX * (_playerSettings.InverseX ? -_inputManager.ViewInput.x :
                                                                 _inputManager.ViewInput.x) * Time.deltaTime;

        _camera.localRotation = Quaternion.Euler(_newCameraRotation);

        transform.localRotation = Quaternion.Euler(_newCharcterRotation);

    }

    #endregion

    #region - Jumping -

    private void Jumping()
    {
        if (CanJamp() && IsGrounded())
        {
           _currentGravity = Mathf.Sqrt(_playerSettings.JumpForce * -2f * _gravitySettings.Gravity);
           _currentStamina -= _playerSettings.JumpStaminaDrain;
           _isJumping = true;
        }  
    }

    private bool CanJamp()
    {
        return _currentStamina > _playerSettings.Stamina / 4f;
    }

    #endregion

    #region - Sprinting -

    private void Sprinting()
    {
        if (_inputManager.SprintTrigger)
        {
            if (_currentStamina > (_playerSettings.Stamina / 4f))
            {
                _isRunning = true;
                _isWalking = false;
            }
        }
        else
        {
            _isRunning = false;
            _isWalking = true;
        }
    }

    private void CalculateSprinting()
    {
        if (_isRunning && _playerSettings.Stamina > 0)
        {
            if (!CanSprinting())
            {
                _isRunning = false;
                return;
            }

            _currentStamina -= _playerSettings.StaminaDrain * Time.deltaTime;

            if (_currentStamina <= 0)
            {
                _isRunning = false;
                _isWalking = true;
            }

            _currentStaminaDelay = _playerSettings.StaminaExecutionDelay;
        }
        else
        {
            if (_currentStaminaDelay <= 0)
            {
                if (_currentStaminaDelay < _playerSettings.Stamina)
                {
                    _currentStamina += _playerSettings.StaminaRestore * Time.deltaTime;

                    if (_currentStamina > _playerSettings.Stamina)
                    {
                        _currentStamina = _playerSettings.Stamina;
                    }
                }

            }
            else
            {
                _currentStaminaDelay -= Time.deltaTime;
            }
        }

    }

    private bool CanSprinting()
    {
        if(Mathf.Abs(_inputManager.MoveInput.y) > 0.25f || Mathf.Abs(_inputManager.MoveInput.x) > 0.25f)
        {
            return true;
        }

        return false;
    }

    #endregion

    #region - Crouch/Stand -

    //private void Crouch()
    //{
    //    if (_inputManager.CrouchTrigger)
    //    {
    //        if (_currentBodyPosition != PlayerBodyPosition.Crocuh)
    //        {
    //            _currentBodyPosition = PlayerBodyPosition.Crocuh;
    //            _currentStance = _crouch;

    //            StartCoroutine(CrouchingStanding());

    //            Debug.Log(4567893);
    //        }
    //        else
    //        {
    //            if (CheckCeiling(_stand.HeightCollider))
    //            {
    //                return;
    //            }

    //            _currentBodyPosition = PlayerBodyPosition.Stand;
    //            _currentStance = _stand;

    //            StartCoroutine(CrouchingStanding());
    //        }
    //    }

    //}


    private void Crouch()
    {
        if (_currentBodyPosition != PlayerBodyPosition.Crocuh)
        {
            _currentBodyPosition = PlayerBodyPosition.Crocuh;
            _currentStance = _crouch;

            StartCoroutine(CrouchingStanding());
        }
        else
        {
            if (CheckCeiling(_stand.HeightCollider))
            {
                return;
            }

            _currentBodyPosition = PlayerBodyPosition.Stand;
            _currentStance = _stand;

            StartCoroutine(CrouchingStanding());
        }
        
    }

    #endregion

    #region - GetStartValues/SetStarValues -

    private void GetStartValues()
    {
        _characterController = GetComponent<CharacterController>();

        _newCharcterRotation = transform.localRotation.eulerAngles;
        _newCameraRotation = _camera.localRotation.eulerAngles;
    }

    private void SetStartValues()
    {
        _gravityDirection = Vector3.down;
        _currentStamina = _playerSettings.Stamina;

        _currentBodyPosition = PlayerBodyPosition.Stand;
        _currentCameraHeight = 0.8f;
        _currentStance = _stand;

        _localSpaceCameraPos = _camera.transform.localPosition;

        _headBob.Initialize();
        _headBob.RegisterEventCallback(1.5f, PlayFootStepSound, CurveControlledBod.CurveControlledBobCallbackType.Vertical);
    }

    #endregion

    #region - Gravity -
    private void CalculateGravity()
    {
        if(IsGrounded() && !_isJumping)
        {
            _currentGravity = _gravitySettings.Gravity;
        }
        else
        {
            _currentGravity -= (9.8f * _gravitySettings.GravityAmmount * Time.deltaTime) * Time.deltaTime;

            if(_currentGravity < _gravitySettings.MaximumGravity)
            {
                _currentGravity = _gravitySettings.MaximumGravity;
            }
        }

        _gravityMovement = _gravityDirection * -_currentGravity * Time.deltaTime;
    }

    #endregion

    #region - CalculateFalling -

    private void CalculateFalling()
    {
        _fallingSpeed = _relativePlayerVelocity.y;

        if(IsFalling() && !IsGrounded() && !_isFalling)
        {
            _isFalling = true;
        }

        if (IsGrounded() && _isFalling && _fallingSpeed < -0.1f)
        {
            _isFalling = false;
            _isJumping = false;
        }

    }

    #endregion

    #region - CheckGround/CheckFalling -

    private bool IsGrounded()
    {
        return _characterController.isGrounded;
    }

    private bool IsFalling()
    {
        if(_fallingSpeed < _playerSettings.FallingThreashold)
        {
            return true;
        }

        return false;
    }

    #endregion

    #region - CheckMovement -

    private bool CheckMovement()
    {
        if(_axis.sqrMagnitude > 0.2)
            return true;

        return false;
    }

    #endregion

    #region - Crocuhing - 

    private IEnumerator CrouchingStanding()
    {
        _crouchingAnimation = true;

        while(_crouchingStandingAnimationTime < _animationCrouchStandDuration)
        {
            _currentCameraHeight = Mathf.SmoothDamp(_currentCameraHeight, _currentStance.HeightCamera,
                                   ref _cameraHeightVelocity, _currentStance.SmoothingStanceSpeed);

            _characterController.height = Mathf.SmoothDamp(_characterController.height, _currentStance.HeightCollider,
                                          ref _colliderHeightVelocity, _currentStance.SmoothingStanceSpeed);

            _characterController.center = Vector3.SmoothDamp(_characterController.center, _currentStance.CenterCollider,
                                          ref _colliderCenterVelocity, _currentStance.SmoothingStanceSpeed);

            _camera.localPosition = new Vector3(_camera.localPosition.x, _currentCameraHeight,
                                                _camera.localPosition.z);

            _crouchingStandingAnimationTime += Time.deltaTime;

            yield return null;
        }

        _crouchingStandingAnimationTime = 0;
        _crouchingAnimation = false;
    }

    private bool CheckCeiling(float stanceHeight)
    {
        Vector3 startPoint = new Vector3(_feetTransform.transform.position.x,
                             _feetTransform.transform.position.y + _characterController.radius
                             + _playerSettings.ErrorMarginCheck, _feetTransform.transform.position.z);

        Vector3 endPoint = new Vector3(_feetTransform.transform.position.x, _feetTransform.transform.position.y -
                           _characterController.radius - _playerSettings.ErrorMarginCheck + stanceHeight,
                           _feetTransform.transform.position.z);

        return Physics.CheckCapsule(startPoint, endPoint, _characterController.radius,_playerSettings.CheckCeilingMask);
    }

    #endregion

    #region - PlayFootstepSound -

    private void PlayFootStepSound()
    {
        if(AudioManager.Instance != null && _footsteps != null)
        {
            AudioClip audioClip;

            if (_isRunning)
            {
                audioClip = _footsteps[1];
            }
            else
            {
                audioClip = _footsteps[0];
            }

            AudioManager.Instance.PlayOneShotSound("Player", audioClip, transform.position, _footsteps.Volume,
                                                  _footsteps.SpatialBlend, _footsteps.Priority);
        }
    }

    #endregion

    #region - UpdateAnimation -

    private void UpdateAnimation()
    {
        _animator.SetFloat("Speed",_axis.magnitude);
        _animator.SetBool("IsRunning", _isRunning);
    }

    #endregion

    #region - OnEnebale/OnDisable

    private void OnEnable()
    {
        _inputManager.Jump += Jumping;
        _inputManager.Crouch += Crouch;
    }

    private void OnDisable()
    {
        _inputManager.Jump -= Jumping;
        _inputManager.Crouch -= Crouch;
    }

    #endregion
}

    #region - CurveControlledBob -

[System.Serializable]
public class CurveControlledBod
{
    [SerializeField]
    private AnimationCurve _bobcurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f),
                                                          new Keyframe(1f, 0f), new Keyframe(1.5f, -1f),
                                                          new Keyframe(2f, 0f));

    [SerializeField] private float _horizontalMultiplier;
    [SerializeField] private float _verticalMultiplier;
    [SerializeField] private float _verticalHorizontalSpeedRation;
    [SerializeField] private float _baseInternal;

    private float _prevXPlayHead;
    private float _prevYPlayHead;
    private float _xPlayHead;
    private float _yPlayHead;
    private float _curveEndTime;
    private List<CurveControlledBobEvent> _events = new List<CurveControlledBobEvent>();

    public void Initialize()
    {
        _curveEndTime = _bobcurve[_bobcurve.length - 1].time;
        _xPlayHead = 0f;
        _yPlayHead = 0f;
        _prevXPlayHead = 0f;
        _prevYPlayHead = 0f;
    }

    public void RegisterEventCallback(float time, CurveControlledBobCallback function, CurveControlledBobCallbackType type)
    {
        CurveControlledBobEvent bobEvent = new CurveControlledBobEvent();
        bobEvent.Time = time;
        bobEvent.Function = function;
        bobEvent.Type = type;

        _events.Add(bobEvent);
        _events.Sort(delegate(CurveControlledBobEvent t1, CurveControlledBobEvent t2)
            {
                return (t1.Time.CompareTo(t2.Time));
            }
        );
    }
    public Vector3 GetVectorOffset(float speed)
    {
        _xPlayHead += (speed * Time.deltaTime) / _baseInternal;
        _yPlayHead += ((speed * Time.deltaTime) / _baseInternal) * _verticalHorizontalSpeedRation;

        if (_xPlayHead > _curveEndTime)
            _xPlayHead -= _curveEndTime;

        if (_yPlayHead > _curveEndTime)
            _yPlayHead -= _curveEndTime;

        for(int i = 0; i < _events.Count; i++)
        {
            CurveControlledBobEvent ev = _events[i];

            if (ev != null)
            {
                if(ev.Type == CurveControlledBobCallbackType.Vertical)
                {
                    if((_prevYPlayHead < ev.Time && _yPlayHead >= ev.Time ||
                        (_prevYPlayHead > _yPlayHead && (ev.Time > _prevYPlayHead || ev.Time <= _yPlayHead))))
                    {
                        ev.Function();
                    }
                }
                else
                {
                    if ((_prevXPlayHead < ev.Time && _xPlayHead >= ev.Time ||
                       (_prevXPlayHead > _xPlayHead && (ev.Time > _prevXPlayHead || ev.Time <= _xPlayHead))))
                    {
                        ev.Function();
                    }
                }
            }
        }

        float xPos = _bobcurve.Evaluate(_xPlayHead) * _horizontalMultiplier;
        float yPos = _bobcurve.Evaluate(_yPlayHead) * _verticalMultiplier;

        _prevXPlayHead = _xPlayHead;
        _prevYPlayHead = _yPlayHead;

        return new Vector3(xPos, yPos, 0f);
    }

    #endregion

    #region - CurveControlledBobevent -

    public class CurveControlledBobEvent
    {
        public float Time;
        public CurveControlledBobCallback Function;
        public CurveControlledBobCallbackType Type = CurveControlledBobCallbackType.Vertical;

    }

    #endregion

    #region - CurveControlledBobCallbackType -

    public enum CurveControlledBobCallbackType
    {
        Horizontal,
        Vertical
    }

    #endregion

    public delegate void CurveControlledBobCallback(); 
}
