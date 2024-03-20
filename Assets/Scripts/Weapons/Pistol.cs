using UnityEngine;

public class Pistol : BaseWeapon, IWeapon
{
    public  void OnTriggerHold()
    {
        Shoot();
        _triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        _triggerReleasedSinceLastShot = true;
    }

    public void SetWeaponStrategy(WeaponStrategy weaponStrategy)
    {
        _weaponStrategy = weaponStrategy;
    }

    public void Shoot()
    {
        if (Time.time > _nextShotTime)
        {
            if (!_triggerReleasedSinceLastShot) return;

            foreach (Transform muzzle in _muzzles)
            {
                _weaponStrategy.Shoot(muzzle, _shellEjector, _muzzleVelocity);
            }
            _muzzleFlash.Activate();
            _nextShotTime = Time.time + _timeBetweenShots;
        }
        
    }
}
