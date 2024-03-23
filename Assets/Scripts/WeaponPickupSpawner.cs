using UnityEngine;

public class WeaponPickupSpawner : PickupSpawner
{
    [SerializeField] private WeaponFactory _weaponFactory;

    public override void Pickup(Collider other)
    {
        other.GetComponent<WeaponController>().EquipWeapon(_weaponFactory);
    }
}
