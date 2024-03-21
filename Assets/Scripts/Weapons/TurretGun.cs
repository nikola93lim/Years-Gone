using System;
using System.Collections;
using UnityEngine;

public class TurretGun : BaseWeapon
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
            transform.LookAt(_target);
        }
        else
        {
            transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
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
        }
    }
}
