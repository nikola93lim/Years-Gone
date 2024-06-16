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
        _currentWeapon.Shoot();
        _nextShotTime = Time.time + _timeBetweenShots;
    }

    public override void EquipWeapon(WeaponFactory weaponFactory)
    {
        base.EquipWeapon(weaponFactory);
        _timeBetweenShots = _currentWeapon.GetTimeBetweenShots();
    }
}
