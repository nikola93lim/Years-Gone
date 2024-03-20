using UnityEngine;

[CreateAssetMenu(fileName = "RifleFactory", menuName = "Weapon Factory/Rifle")]
public class RifleFactory : WeaponFactory
{
    public override IWeapon CreateWeapon(Transform spawnPoint)
    {
        Rifle rifle = Instantiate(_weaponPrefab, spawnPoint.position, spawnPoint.rotation) as Rifle;
        rifle.transform.parent = spawnPoint;
        rifle.SetWeaponStrategy(_weaponStrategy);
        return rifle;
    }
}
