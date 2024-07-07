using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _speed = 10f;

    private Rigidbody _rb;
    private InputReader _inputReader;

    private Vector3 _inputDirection;

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        SetInputDirection(_inputReader.Move);
        HandleLookInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void SetInputDirection(Vector2 input)
    {
        _inputDirection = new Vector3(input.x, 0f, input.y);
    }

    private void MovePlayer()
    {
        Vector3 targetVelocity = _inputDirection * _speed;
        _rb.velocity = new Vector3(targetVelocity.x, _rb.velocity.y, targetVelocity.z);
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

    public Vector3 GetVelocity()
    {
        return _rb.velocity;
    }
}
