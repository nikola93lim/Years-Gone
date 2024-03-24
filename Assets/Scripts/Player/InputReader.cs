using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputReader : MonoBehaviour
{
    private PlayerInput _playerInput;

    private InputAction _moveAction;
    private InputAction _fireAction;

    public Vector2 Move => _moveAction.ReadValue<Vector2>();
    public bool Fire => _fireAction.ReadValue<float>() > 0f;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _fireAction = _playerInput.actions["Fire"];
    }
}
