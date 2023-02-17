using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(WheelJoint2D))]
public class WheelController : MonoBehaviour
{
    private int _groundLayerMask;
    private Rigidbody2D _rigidbody;
    private WheelJoint2D _wheelJoint;

    public bool IsGrounded { get; private set; }

    public Rigidbody2D Rigidbody => _rigidbody;
    public WheelJoint2D WheelJoint => _wheelJoint;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _wheelJoint = GetComponent<WheelJoint2D>();
        _groundLayerMask = LayerMask.NameToLayer(Constants.GroundLayerMask);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _groundLayerMask)
        {
            IsGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _groundLayerMask)
        {
            IsGrounded = false;
        }
    }
}