using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private const string FORWARD = "Forward";
    private const string SIDEWAYS = "Sideways";
    private Animator _animator;
    private Rigidbody _rb;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        Animate();
    }

    private void Animate()
    {
        if (_animator == null) return;

        // Get the local velocity relative to the character's forward direction
        Vector3 localVelocity = transform.InverseTransformDirection(_rb.velocity);

        _animator.SetFloat(FORWARD, Mathf.Clamp(localVelocity.z, -1f, 1f));
        _animator.SetFloat(SIDEWAYS, Mathf.Clamp(localVelocity.x, -1f, 1f));
    }
}
