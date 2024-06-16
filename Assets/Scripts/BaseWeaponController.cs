using System;
using UnityEngine;

public class BaseWeaponController : MonoBehaviour 
{
    [SerializeField] protected Transform _weaponSpawnTransform;
    [SerializeField] protected WeaponFactory _weaponFactory;
    [SerializeField] protected Weapon _currentWeapon;

    public virtual void EquipWeapon(WeaponFactory weaponFactory)
    {
        if (_currentWeapon != null)
        {
            Destroy(_currentWeapon.gameObject);
        }

        _currentWeapon = weaponFactory.CreateWeapon(_weaponSpawnTransform);
    }

    protected virtual void Start()
    {
        EquipWeapon(_weaponFactory);
    }
}

