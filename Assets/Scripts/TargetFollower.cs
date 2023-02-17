using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TargetFollower : MonoBehaviour
{
    [SerializeField] private float _dampTime = 0.15f;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector2 _centerPoint = new Vector2(0.5f, 0.5f);

    private Camera _camera;
    private Vector3 _velocity = Vector3.zero;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (_target)
        {
            Vector3 point = _camera.WorldToViewportPoint(_target.position);
            Vector3 delta = _target.position - _camera.ViewportToWorldPoint(new Vector3(_centerPoint.x, _centerPoint.y, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, _dampTime);
        }

    }
}
