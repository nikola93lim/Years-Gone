using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunFactory", menuName = "Weapon Factory/Shotgun")]
public class ShotgunFactory : WeaponFactory
{
    public override IWeapon CreateWeapon(Transform spawnPoint)
    {
        Shotgun shotgun = Instantiate(_weaponPrefab, spawnPoint.position, spawnPoint.rotation) as Shotgun;
        shotgun.transform.parent = spawnPoint;
        shotgun.SetWeaponStrategy(_weaponStrategy);
        return shotgun;
    }
}