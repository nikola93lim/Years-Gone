using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(GunController))]
public class InputReader : MonoBehaviour
{
    private PlayerInput _playerInput;

    private InputAction _moveAction;

    public Vector2 Move => _moveAction.ReadValue<Vector2>();

    [SerializeField] private LayerMask _groundLayerMask;
    private GunController _gunController;

    private float _speed = 10f;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];

        _gunController = GetComponent<GunController>();
    }

    private void Update()
    {
       // HandleLookInput();
        HandleShootingInput();
    }

    private void HandleShootingInput()
    {
        if (Input.GetMouseButton(0))
        {
            _gunController.OnTriggerHold();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _gunController.OnTriggerRelease();
        }
    }

    private void HandleLookInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _groundLayerMask))
        {
            //_playerController.LookAt(hitInfo.point);
        }
    }
}
