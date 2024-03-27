﻿using UnityEngine;

public class RPG : Weapon
{
    public override void Shoot()
    {
        if (Time.time > _nextShotTime)
        {
            foreach (Transform muzzle in _muzzles)
            {
                _weaponStrategy.Fire(muzzle, _shellEjector, null, _muzzleVelocity);
            }

            _muzzleFlash.Activate();
            _nextShotTime = Time.time + _timeBetweenShots;
            //SoundManager.PlaySound(_fireSound, _muzzles[0].position);

        }
    }
}
