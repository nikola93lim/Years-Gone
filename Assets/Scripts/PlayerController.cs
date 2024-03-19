using System;
using UnityEditor.PackageManager;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;

    private Vector3 _velocity;

    private InputReader _inputReader;

    private float _speed = 10f;

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
        _rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        SetVelocity(_inputReader.Move);
        _rb.MovePosition(_rb.position + _speed * Time.fixedDeltaTime * _velocity.normalized);
    }

    public void SetVelocity(Vector2 input)
    {
        _velocity = new Vector3(input.x, 0f, input.y);
    }

    public void LookAt(Vector3 lookAtPoint)
    {
        Vector3 adjustedLookAtPoint = new Vector3(lookAtPoint.x, transform.position.y, lookAtPoint.z);
        transform.LookAt(adjustedLookAtPoint);
    }
}
