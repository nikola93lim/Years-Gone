using UnityEngine;

public class Rifle : BaseWeapon
{
    public override void Shoot()
    {
        if (Time.time > _nextShotTime)
        {
            foreach (Transform muzzle in _muzzles)
            {
                _weaponStrategy.TryFire(muzzle, _shellEjector, _muzzleVelocity);
            }
            _muzzleFlash.Activate();
            _nextShotTime = Time.time + _timeBetweenShots;
        }
    }
}
