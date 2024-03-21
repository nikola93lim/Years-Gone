using UnityEngine;

public class TurretGun : BaseWeapon
{
    private Transform _target;

    protected override void Start()
    {
        base.Start();
        _target = FindObjectOfType<PlayerController>().transform;
    }

    public override void Shoot()
    {
        if (_target == null) return;
        if (Time.time > _nextShotTime)
        {
            foreach (Transform muzzle in _muzzles)
            {
                _weaponStrategy.Fire(muzzle, _shellEjector, _target, _muzzleVelocity);
                _muzzleFlash.Activate();
                _nextShotTime = Time.time + _timeBetweenShots;
            }
        }
    }
}
