using System;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _speed = 10f;

    private Rigidbody _rb;
    private InputReader _inputReader;

    private Vector3 _velocity;

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        SetVelocity(_inputReader.Move);
        HandleLookInput();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _speed * Time.fixedDeltaTime * _velocity.normalized);
    }

    private void SetVelocity(Vector2 input)
    {
        _velocity = new Vector3(input.x, 0f, input.y);
    }

    private void HandleLookInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, Vector3.up);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Vector3 adjustedLookAtPoint = new Vector3(point.x, transform.position.y, point.z);
            transform.LookAt(adjustedLookAtPoint);
        }
    }
}
