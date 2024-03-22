﻿using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "HomingMissileStrategy", menuName = "Weapon Strategy/Homing Missile")]
public class HomingMissileWeaponStrategy : WeaponStrategy
{
    [SerializeField] private float _trackingSpeed = 1.0f;
    [SerializeField] private Projectile _missilePrefab;
    [SerializeField] private ParticleSystem _fireBackParticleSystem;
    [SerializeField] private float _range;

    public override void Fire(Transform projectileOrigin, Transform shellOrigin, Transform target, float muzzleVelocity)
    {
        Projectile missile = Instantiate(_missilePrefab, projectileOrigin.position, projectileOrigin.rotation);
        missile.SetSpeed(muzzleVelocity);
        if (target == null) return;

        missile.Callback += () =>
        {
            if (target == null) return;
            Vector3 directionToTarget = (target.position - missile.transform.position).normalized;

            Quaternion rotation = Quaternion.LookRotation(directionToTarget);
            missile.transform.rotation = Quaternion.Slerp(missile.transform.rotation, rotation, _trackingSpeed * Time.deltaTime);
        };
    }
}
