using UnityEngine;

public class Shotgun : BaseWeapon, IWeapon
{
    public void OnTriggerHold()
    {
        Shoot();
        _triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        _triggerReleasedSinceLastShot = true;
    }

    public void SetWeaponStrategy(WeaponStrategy strategy)
    {
        _weaponStrategy = strategy;
    }

    public void Shoot()
    {
        if (Time.time > _nextShotTime)
        {
            foreach (Transform muzzle in _muzzles)
            {
                _weaponStrategy.Shoot(muzzle, _shellEjector, _muzzleVelocity);
            }
            _muzzleFlash.Activate();
            _nextShotTime = Time.time + _timeBetweenShots;
        }
    }
}
