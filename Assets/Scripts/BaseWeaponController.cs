using System;
using UnityEngine;

public class BaseWeaponController : MonoBehaviour 
{
    [SerializeField] protected Transform _weaponSpawnTransform;
    [SerializeField] protected WeaponFactory _weaponFactory;
    [SerializeField] protected Weapon _currentPrimaryWeapon;
    [SerializeField] protected Weapon _currentSecondaryWeapon;

    public virtual void EquipPrimaryWeapon(WeaponFactory weaponFactory)
    {
        if (_currentPrimaryWeapon != null)
        {
            Destroy(_currentPrimaryWeapon.gameObject);
        }

        _currentPrimaryWeapon = weaponFactory.CreateWeapon(_weaponSpawnTransform);
    }

    public virtual void EquipSecondaryWeapon(WeaponFactory weaponFactory)
    {
        _currentSecondaryWeapon = weaponFactory.CreateWeapon(_weaponSpawnTransform);
    }

    protected virtual void Start()
    {
        EquipPrimaryWeapon(_weaponFactory);
    }
}

