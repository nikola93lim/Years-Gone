using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    private PlayerController _playerController;
    private GunController _gunController;

    private float _speed = 10f;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _gunController = GetComponent<GunController>();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleLookInput();
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
            _playerController.LookAt(hitInfo.point);
        }
    }

    private void HandleMovementInput()
    {
        Vector3 inputValue = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = _speed * inputValue.normalized;
        _playerController.Move(moveVelocity);
    }
}
