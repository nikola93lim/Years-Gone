using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretGun : Weapon
{
    [SerializeField] private float _range;
    [SerializeField] private float _rotationSpeed = 1.0f;
    [SerializeField] private float _checkForTargetInterval = 1f;
    [SerializeField] private LayerMask _targetLayerMask;

    private Transform _target;
    private WaitForSeconds _waitInterval;

    protected override void Start()
    {
        base.Start();

        _waitInterval = new WaitForSeconds(_checkForTargetInterval);

        StartCoroutine(CheckForTarget());
    }

    private void Update()
    {
        if (_target != null)
        {
            // Calculate the direction from the current position to the target position
            Vector3 direction = _target.position - transform.position;

            // Ensure that the direction is not zero (to avoid division by zero)
            if (direction != Vector3.zero)
            {
                // Create a rotation that looks along the given direction
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Smoothly interpolate between the current rotation and the target rotation using Quaternion.Slerp
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * 0.1f * Time.deltaTime);
            }
        }
        else
        {
            transform.Rotate(_rotationSpeed * Time.deltaTime * Vector3.up);
        }
    }

    private IEnumerator CheckForTarget()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _range, _targetLayerMask);
            if (colliders.Length > 0 )
            {
                _target = colliders[0].transform;
            }
            else
            {
                _target = null;
            }

            yield return _waitInterval;
        }
    }

    public override void Shoot()
    {
        if (_target == null) return;

        foreach (Transform muzzle in _muzzles)
        {
            _weaponStrategy.Fire(muzzle, _shellEjector, _target, _muzzleVelocity);
            _muzzleFlash.Activate();
            _nextShotTime = Time.time + _timeBetweenShots;
            SoundManager.PlaySound(_fireSound, _muzzles[0].position);
        }
    }
}
