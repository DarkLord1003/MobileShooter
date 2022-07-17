using UnityEngine;
using System;

public static class ModelSettings
{
    [Serializable]
    public class PlayerSettings
    {
        [Header("Mouse Look")]
        [SerializeField] private float _sensetivityX;
        [SerializeField] private float _sensetivityY;
        [SerializeField] private bool _inverseX;
        [SerializeField] private bool _inverseY;

        [Header("Move")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _smoothingMovementSpeed;

        [Header("Sprint")]
        [SerializeField] private float _sprintSpeed;

        [Header("Stamina")]
        [SerializeField] private float _stamina;
        [SerializeField] private float _staminaRestore;
        [SerializeField] private float _staminaDrain;
        [SerializeField] private float _staminaExecutionDelay;

        [Header("Clamp Look")]
        [SerializeField] private float _minimumClampX;
        [SerializeField] private float _maximumClampX;

        [Header("jump")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _jumpStaminaDrain;

        [Header("Falling")]
        [SerializeField] private float _fallingThreashold;

        [Header("Change Stance Settings")]
        [SerializeField] private float _errorMarginCheck;
        [SerializeField] private LayerMask _checkCeilingMask;

        //Mouse look
        public float SensetivityX => _sensetivityX;
        public float SensetivityY => _sensetivityY;
        public bool InverseX => _inverseX;
        public bool InverseY => _inverseY;

        //Clamp look
        public float MinimumClampX => _minimumClampX;
        public float MaximumClampX => _maximumClampX;

        //Move
        public float MoveSpeed => _moveSpeed;
        public float SmotthingMovementSpeed => _smoothingMovementSpeed;

        //Sprint
        public float SprintSpeed => _sprintSpeed;
        public float JumpStaminaDrain => _jumpStaminaDrain;

        //Stamina
        public float Stamina => _stamina;
        public float StaminaRestore => _staminaRestore;
        public float StaminaDrain => _staminaDrain;
        public float StaminaExecutionDelay => _staminaExecutionDelay;

        //Jumping
        public float JumpForce => _jumpForce;

        //Falling
        public float FallingThreashold => _fallingThreashold;

        //Change Stance Check
        public float ErrorMarginCheck => _errorMarginCheck;
        public LayerMask CheckCeilingMask => _checkCeilingMask;

        
    }

    [Serializable]
    public class GravitySettings
    {
        [Header("Gravity")]
        [SerializeField] private float _gravity;
        [SerializeField] private float _gravityAmmount;
        [SerializeField] private float _maximumGravity;

        //Gravity
        public float Gravity => _gravity;
        public float GravityAmmount => _gravityAmmount;
        public float MaximumGravity => _maximumGravity;
    }


    [Serializable]
    public class PlayerStance
    {
        [Header("Setings")]
        [SerializeField] private Vector3 _centerCollider;
        [SerializeField] private float _heightCollider;
        [SerializeField] private float _heightCamera;
        [SerializeField] private float _smoothingStanceSpeed;

        //Properties
        public Vector3 CenterCollider => _centerCollider;
        public float HeightCollider => _heightCollider;
        public float HeightCamera => _heightCamera;
        public float SmoothingStanceSpeed => _smoothingStanceSpeed;
    }

    [Serializable]
    public enum PlayerBodyPosition
    {
        Stand,
        Crocuh,
        Prone
    }

}
