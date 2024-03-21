﻿using UnityEngine;

public class RPG : BaseWeapon
{
    public override void Shoot()
    {
        if (Time.time > _nextShotTime)
        {
            foreach (Transform muzzle in _muzzles)
            {
                if (!_weaponStrategy.TryFire(muzzle, _shellEjector, _muzzleVelocity)) return;
                _muzzleFlash.Activate();
                _nextShotTime = Time.time + _timeBetweenShots;
            }
        }
    }
}
