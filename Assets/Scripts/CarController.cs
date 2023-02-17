using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInfo))]
[RequireComponent(typeof(AudioSource))]
public class CarController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _carBody;
    [SerializeField] private WheelController _backWheel;
    [SerializeField] private WheelController _frontWheel;
    [SerializeField] private ToggleButton _speedDownButton;
    [SerializeField] private ToggleButton _speedUpButton;
    [SerializeField] private float _moveSpeed = 1000;
    [SerializeField] private float _moveSpeedMultiplier = 1.6f;
    [SerializeField] private float _rotationForce = 600f;

    private Inputs _inputs;
    private Func<bool> _speedDownFunc;
    private Func<bool> _speedUpFunc;

    private PlayerInfo _playerInfo;
    private AudioSource _engineSound;

    private void Start()
    {
        _playerInfo = GetComponent<PlayerInfo>();
        _engineSound = GetComponent<AudioSource>();

        if (Application.platform == RuntimePlatform.Android)
        {
            _speedDownFunc = () => _speedDownButton.IsPressed;
            _speedUpFunc = () => _speedUpButton.IsPressed;
        }
        else
        {
            _speedDownFunc = _inputs.PCActions.SpeedDown.IsPressed;
            _speedUpFunc = _inputs.PCActions.SpeedUp.IsPressed;
            _inputs.PCActions.Enable();
        }
    }

    private void Awake()
    {
        _inputs = new Inputs();
    }

    private void Update()
    {
        bool speedUp = _speedUpFunc();
        bool speedDown = _speedDownFunc();

        JointMotor2D backMotor = _backWheel.WheelJoint.motor;
        JointMotor2D frontMotor = _frontWheel.WheelJoint.motor;

        _playerInfo.IsActivelyUse = speedUp;

        if (speedUp || speedDown)
        {
            Vector2 direction = _carBody.gameObject.transform.rotation * Vector2.up;

            if (speedUp && speedDown)
            {
                // [TODO]
            }
            else if (speedUp)
            {
                backMotor.motorSpeed = _moveSpeed * _moveSpeedMultiplier;

                if (!_frontWheel.IsGrounded)
                {
                    _carBody.AddForceAtPosition(direction * _rotationForce * Time.deltaTime, _frontWheel.transform.position, ForceMode2D.Force);
                }
            }
            else if (speedDown)
            {
                backMotor.motorSpeed = -_moveSpeed;

                if (!_backWheel.IsGrounded)
                {
                    _carBody.AddForceAtPosition(direction * _rotationForce * Time.deltaTime, _backWheel.transform.position, ForceMode2D.Force);
                }
            }

            _backWheel.WheelJoint.motor = backMotor;
        }
        else
        {
            _backWheel.WheelJoint.useMotor = false;
        }

        if (_playerInfo.IsFuelEmpty)
        {
            _backWheel.WheelJoint.useMotor = false;
            _engineSound.pitch = MoveTo(0, _engineSound.pitch, Time.deltaTime * 2);
        }
        else
        {
            if (speedUp)
                _engineSound.pitch = MoveTo(UnityEngine.Random.Range(2, 2.4f), _engineSound.pitch, Time.deltaTime);
            else
                _engineSound.pitch = MoveTo(1, _engineSound.pitch, Time.deltaTime * 2);
        }
    }

    public static void Break(CarController carController)
    {
        carController._engineSound.pitch = 0;
        WheelJoint2D backJoint = carController._backWheel.WheelJoint;
        WheelJoint2D frontJoint = carController._frontWheel.WheelJoint;
        Destroy(carController._backWheel.GetComponent<WheelStabilizer>());
        Destroy(carController._frontWheel.GetComponent<WheelStabilizer>());
        Destroy(carController);
        Destroy(backJoint);
        Destroy(frontJoint);
    }

    private static float MoveTo(float target, float current, float distance)
    {
        if (target == current)
            return target;

        if (current > target)
            return current - distance;
        else 
            return current + distance;
    }
}
