using UnityEngine;

public class WeaponPickupSpawner : PickupSpawner
{
    [SerializeField] private WeaponFactory _weaponFactory;
    [SerializeField] private bool _isPrimaryWeaponSpawner = true;

    public override void Pickup(Collider other)
    {
        if (_isPrimaryWeaponSpawner)
        {
            other.GetComponent<WeaponController>().EquipPrimaryWeapon(_weaponFactory);
        }
        else
        {
            other.GetComponent<WeaponController>().EquipSecondaryWeapon(_weaponFactory);
        }
    }
}
