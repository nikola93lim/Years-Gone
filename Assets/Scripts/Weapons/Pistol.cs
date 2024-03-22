using UnityEngine;

public class Pistol : Weapon
{
    public override void Shoot()
    {
        if (Time.time > _nextShotTime)
        {
            if (!_triggerReleasedSinceLastShot) return;

            foreach (Transform muzzle in _muzzles)
            {
                _weaponStrategy.Fire(muzzle, _shellEjector, null, _muzzleVelocity);
            }
            _muzzleFlash.Activate();
            _nextShotTime = Time.time + _timeBetweenShots;
        }
    }
}
