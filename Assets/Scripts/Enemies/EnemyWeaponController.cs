using System;
using UnityEngine;

public class EnemyWeaponController : BaseWeaponController
{
    [SerializeField] private bool _isStaticEnemy;

    private float _nextShotTime;
    private float _timeBetweenShots;

    private void Update()
    {
        if (!_isStaticEnemy) return;

        if (Time.time > _nextShotTime)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        _currentPrimaryWeapon.Shoot();
        _nextShotTime = Time.time + _timeBetweenShots;
    }

    public override void EquipPrimaryWeapon(WeaponFactory weaponFactory)
    {
        base.EquipPrimaryWeapon(weaponFactory);
        _timeBetweenShots = _currentPrimaryWeapon.GetTimeBetweenShots();
    }
}
